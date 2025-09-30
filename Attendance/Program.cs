using Attendance.Application.Extension;
using Attendance.Domain.Models;
using Attendance.Infrastructure.Efcore.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var globalClass = new GlobalClass();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie( options =>
   {
       options.LoginPath = "/Login/Login";
       options.AccessDeniedPath = "/Home/AccessDenied";
       options.LogoutPath = "/Login/Logout";
       options.SlidingExpiration = true;
       options.Cookie.HttpOnly = true;
       options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
       options.Cookie.SameSite = SameSiteMode.Strict;
       //options.ExpireTimeSpan = TimeSpan.FromHours(24);
   });
builder.Services.AddAuthorization();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/app-{Date}.txt");

builder.Services.AddApplicationServices();
builder.Services.AddEfcoreInfrastructureService();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(globalClass);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:7000")
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .SetIsOriginAllowedToAllowWildcardSubdomains();
//        });
//});

var app = builder.Build();

app.Use(async (context, next) =>
 {

     var token = context.Request.Cookies["jwtToken"];
     globalClass.Token = token;
     await next.Invoke();
 });


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Dashboard}/{action=Dashboard}/{id?}");

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI(c =>
    //{
    //   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
    //});
}
app.MapControllers();
app.Run();
