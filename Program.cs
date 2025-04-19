using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Serilog.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization; // Добавьте этот using
using System.Globalization; // Добавьте этот using

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку локализации
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

#region Login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
#endregion

builder.Services.AddAuthorization();

#region Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/thelab.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341/")
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

// Настройка локализации
var supportedCultures = new[] { "en", "ru", "kk" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// Добавляем провайдер куки для хранения выбранного языка
localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider()
{
    CookieName = "lang",
    Options = localizationOptions
});

builder.Services.AddSingleton(localizationOptions);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Локализация должна быть после UseRouting()
app.UseRequestLocalization(localizationOptions);

// Аутентификация и авторизация должны быть после UseRouting()
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();