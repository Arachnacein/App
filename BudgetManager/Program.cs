using BudgetManager.Data;
using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;
using BudgetManager.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddScoped<ITransactionRespository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService,  TransactionService>();
builder.Services.AddScoped<IPatternRepository, PatternRepository>();
builder.Services.AddScoped<IPatternService, PatternService>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IMonthPatternRepository, MonthPatternRepository>();
builder.Services.AddScoped<IMonthPatternService, MonthPatternService>();

//mappers
builder.Services.AddScoped<ITransactionMapper, TransactionMapper>();
builder.Services.AddScoped<IPatternMapper,  PatternMapper>();
builder.Services.AddScoped<IIncomeMapper, IncomeMapper>();
builder.Services.AddScoped<IMonthPatternMapper, MonthPatternMapper>();

//db configuration
var db_host = Environment.GetEnvironmentVariable("db_host");
var db_name = Environment.GetEnvironmentVariable("db_name");
var db_password = Environment.GetEnvironmentVariable("db_password");
//var connString = $"Server = {db_host}; Database = {db_name}; User Id = sa; Password = {db_password}";
var connString = $"Data Source={db_host};Initial Catalog={db_name};Persist Security Info=True;User ID=sa;Password={db_password};TrustServerCertificate=True;";


builder.Services.AddMediatR(config => 
                    config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

builder.Services.AddDbContext<BudgetDbContext>(options =>
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://keycloak:8080/realms/AppRealm"; //keycloak
                    options.Audience = "identityapi";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "http://keycloak:8080/realms/AppRealm",
                        ValidateAudience = true,
                        ValidAudience = "identityapi",
                        ValidateLifetime = true, 
                        ValidateIssuerSigningKey = true
                        //RoleClaimType = "realm_access.roles"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;

                            if (claimsIdentity != null)
                            {
                                var authenticatedIdentity = new ClaimsIdentity(
                                    claimsIdentity.Claims,
                                    JwtBearerDefaults.AuthenticationScheme
                                );

                                var resourceAccess = context.Principal?.FindFirst("resource_access")?.Value;
                                if (!string.IsNullOrEmpty(resourceAccess))
                                {
                                    var roles = JsonDocument.Parse(resourceAccess)
                                        .RootElement
                                        .GetProperty("identityapi")
                                        .GetProperty("roles")
                                        .EnumerateArray()
                                        .Select(role => role.GetString());

                                    foreach (var role in roles)
                                    {
                                        authenticatedIdentity.AddClaim(new Claim(ClaimTypes.Role, role ?? string.Empty));
                                        Console.WriteLine($"Added role: {role}");
                                    }
                                }

                                context.HttpContext.User = new ClaimsPrincipal(authenticatedIdentity);

                                Console.WriteLine("User updated in HttpContext.");
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Token Authentication Failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        }
                    };
                });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("user"));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//automiatic migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
    //db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandling>();
//app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"];
    Console.WriteLine($"Authorization Header: {token}");
    await next();
});
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        Console.WriteLine($"Authenticated User: {context.User.Identity.Name}");
        Console.WriteLine($"Claims: {string.Join(", ", context.User.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
    }
    else
    {
        Console.WriteLine("User is not authenticated.");
    }

    await next();
});


app.Use(async (context, next) =>
{
    Console.WriteLine("Before UseAuthentication:");
    Console.WriteLine($"IsAuthenticated: {context.User?.Identity?.IsAuthenticated}");
    await next();
    Console.WriteLine("After UseAuthentication:");
    Console.WriteLine($"IsAuthenticated: {context.User?.Identity?.IsAuthenticated}");
});

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"];
    Console.WriteLine($"Authorization Header in apigateway: {token}");
    await next();
});

app.UseAuthentication();

app.Use(async (context, next) =>
{
    Console.WriteLine("Before UseAuthorization:");
    Console.WriteLine($"IsAuthenticated: {context.User?.Identity?.IsAuthenticated}");
    await next();
    Console.WriteLine("After UseAuthorization:");
    Console.WriteLine($"IsAuthenticated: {context.User?.Identity?.IsAuthenticated}");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
