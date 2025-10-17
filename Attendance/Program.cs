using Attendance.Application.Extension;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Infrastructure.Efcore.Extensions;
using Attendance.Infrastructure.Efcore.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Register GlobalClass as Scoped (per request)
builder.Services.AddScoped<GlobalClass>();

// Read API base URL from appsettings.json
var apiBaseUrl = builder.Configuration["APICredential:url"];
if (string.IsNullOrEmpty(apiBaseUrl))
    throw new Exception("API base URL is not configured");

// ? Authentication configuration (fixed)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LogoutPath = "/Login/Logout";
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;

        // ?? For local development:
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // allow HTTP
        options.Cookie.SameSite = SameSiteMode.Lax;            // allow redirects
    });

builder.Services.AddAuthorization();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/app-{Date}.txt");

// HttpClient factory with base URL
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register application services and infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddEfcoreInfrastructureService();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILoginAdaptor, LoginAdaptor>();

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ? Custom middleware: attach JWT token from cookie
app.Use(async (context, next) =>
{
    var globalClass = context.RequestServices.GetRequiredService<GlobalClass>();
    globalClass.Token = context.Request.Cookies["jwtToken"];
    Console.WriteLine($"JWT Token from Cookie: {globalClass.Token}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Dashboard}/{id?}");

app.MapControllers();

app.Run();
