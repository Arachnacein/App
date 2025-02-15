using BudgetManager.Data;
using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;
using BudgetManager.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddScoped<ITransactionRespository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
builder.Services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();

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
builder.Services.AddScoped<IRecurringTransactionMapper, RecurringTransactionMapper>();

//db configuration
var db_host = Environment.GetEnvironmentVariable("db_host");
var db_name = Environment.GetEnvironmentVariable("db_name");
var db_password = Environment.GetEnvironmentVariable("db_password");
var connString = $"Data Source={db_host};Initial Catalog={db_name};Persist Security Info=True;User ID=sa;Password={db_password};TrustServerCertificate=True;";


builder.Services.AddDbContext<BudgetDbContext>(options =>
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


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
