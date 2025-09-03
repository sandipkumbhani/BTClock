using Attendance.Application.Extension;
using Attendance.Domain.Models;
using Attendance.Infrastructure.Efcore.Extensions;

var builder = WebApplication.CreateBuilder(args);
var globalClass = new GlobalClass();
builder.Services.AddAuthentication("Cookies")
   .AddCookie("Cookies", options =>
   {
      options.LoginPath = "/Login/Login";
      //options.AccessDeniedPath = "/Login/Login";
   });
builder.Services.AddAuthorization();
// Configure logging
builder.Logging.ClearProviders(); // Clear default providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging
builder.Logging.AddFile("Logs/app-{Date}.txt"); // Add file logging (requires a file logging provider)

builder.Services.AddApplicationServices();
builder.Services.AddEfcoreInfrastructureService();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(globalClass);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:7191")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

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
   pattern: "{controller=Login}/{action=Login}/{id?}");

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
