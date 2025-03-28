using BudgetManager.Data;
using BudgetManager.Installers;
using BudgetManager.Utils;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

//install services
builder.Services.InstallServiceInAssembly(builder.Configuration);

// Add services to the container.
builder.Services.AddMediatR(config => 
                    config.RegisterServicesFromAssemblies(typeof(Program).Assembly));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();