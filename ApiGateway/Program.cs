using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot_config.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://keycloak:8080/realms/AppRealm"; 
        options.Audience = "identityapi"; 
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "identityapi",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = "http://keycloak:8080/realms/AppRealm"
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Token Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "user"));
});

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();