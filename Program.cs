using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Serilog.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization; // �������� ���� using
using System.Globalization; // �������� ���� using

var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� �����������
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

// ��������� �����������
var supportedCultures = new[] { "en", "ru", "kk" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// ��������� ��������� ���� ��� �������� ���������� �����
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

// ����������� ������ ���� ����� UseRouting()
app.UseRequestLocalization(localizationOptions);

// �������������� � ����������� ������ ���� ����� UseRouting()
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