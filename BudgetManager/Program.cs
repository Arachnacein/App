using BudgetManager.Mappers;
using BudgetManager.Repositories;
using BudgetManager.Services;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddScoped<ITransactionRespository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService,  TransactionService>();

//mappers
builder.Services.AddScoped<ITransactionMapper, TransactionMapper>();


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
