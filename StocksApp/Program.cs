using CRUDExample.Middleware;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Service;
using ServiceContracts;
using StocksApp;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();
builder.Services.AddScoped<IStocksService,StocksService>();
builder.Services.AddScoped<IStocksRepository,StocksRepository>();
builder.Services.AddScoped<IFinnhubRepository,FinnhubRepository>();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection(nameof(TradingOptions)));

builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

if (!builder.Environment.IsEnvironment("Test"))
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot/", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }