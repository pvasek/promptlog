using Microsoft.EntityFrameworkCore;
using PromptLog.App.Services;
using PromptLog.Db;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.GetSection("PromptLog").Get<AppConfiguration>();
if (configuration == null)
{
    throw new Exception("App configuration is missing");
}

// Add services to the container.
builder.Services.AddDbContext<PromptLogDbContext>(
        options => options.UseSqlServer("name=PromptLog:ConnectionString"));
builder.Services.AddOpenAIProxy(configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var startupScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var db = startupScope.ServiceProvider.GetRequiredService<PromptLogDbContext>();
    db.Database.Migrate();
}

// for local development we allow http (`appsettings.Development.json`)
if (configuration.AllowHttp == false)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapOpenAIProxy(configuration);

app.Run();

