using IdentityManager.Middlewares;
using IdentityManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddHttpClient<ILoginservice, LoginService>();
builder.Services.AddHttpClient<IRegisterService, RegisterService>();
builder.Services.AddHttpClient<ITokenService, TokenService>();
builder.Services.AddHttpClient<IUserService, UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = "http://keycloak:8080/realms/AppRealm";
                    opt.Audience = "identityapi";
                    opt.RequireHttpsMetadata = false;
                });
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseMiddleware<ExceptionHandling>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
