using Attendance.Application.Extension;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Infrastructure.Efcore.Extensions;
using Attendance.Infrastructure.Efcore.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<GlobalClass>();
builder.Services.AddSingleton<GlobalClass>();
builder.Services.AddHttpContextAccessor();

var apiBaseUrl = builder.Configuration["APICredential:url"];
if (string.IsNullOrEmpty(apiBaseUrl))
    throw new Exception("API base URL is not configured");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LogoutPath = "/Login/Logout";
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;

        options.Cookie.SecurePolicy = CookieSecurePolicy.None; 
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/app-{Date}.txt");

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddApplicationServices();
builder.Services.AddEfcoreInfrastructureService();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILoginAdaptor, LoginAdaptor>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
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
