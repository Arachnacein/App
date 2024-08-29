using BudgetManager.Data;
using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddScoped<ITransactionRespository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService,  TransactionService>();

//mappers
builder.Services.AddScoped<ITransactionMapper, TransactionMapper>();

//db configuration
var db_host = Environment.GetEnvironmentVariable("db_host");
var db_name = Environment.GetEnvironmentVariable("db_name");
var db_password = Environment.GetEnvironmentVariable("db_password");
var connString = $"Data Source={db_host};Initial Catalog={db_name};Persist Security Info=True;User ID=sa;Password={db_password};TrustServerCertificate=True;";

builder.Services.AddDbContext<TransactionDbContext>(options =>
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Automatyczne zastosowanie migracji przy starcie aplikacji
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
