using Microsoft.AspNetCore.Mvc.Razor;
using MudBlazor.Services;
using UI;
using UI.Models.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddRazorPages()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();
builder.Services.AddMudServices();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<GlobalInfoClass>();
builder.Services.AddTransient<IncomeViewModelValidator>();
builder.Services.AddTransient<TransactionViewModelValidator>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://apigateway:8080") });

var app = builder.Build();

var cultures = new[] { "pl", "en" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(cultures.First())
                                    .AddSupportedCultures(cultures)
                                    .AddSupportedUICultures(cultures);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization(localizationOptions);
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();