using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = "http://keycloak:8080/realms/IdentityRealm";
                    opt.Audience = "identityapi";
                    opt.RequireHttpsMetadata = false;
                });
builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
