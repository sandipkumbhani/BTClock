using Attendance.Application.Extension;
using Attendance.Infrastructure.Efcore;
using Attendance.Infrastructure.Efcore.Extensions;
using Attendance.MapperProfile;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add authentication and cookie policy
builder.Services.AddAuthentication("Cookies")
   .AddCookie("Cookies", options =>
   {
      options.LoginPath = "/Auth/Login";
      options.AccessDeniedPath = "/Auth/Login";
   });

// Add other services
builder.Services.AddApplicationServices();
builder.Services.AddAttendanceServices();
builder.Services.AddAutoMapper(typeof(AutoMapperRegister));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
   ServiceLifetime.Transient);
builder.Services.AddControllersWithViews()
      .AddMvcOptions(options => {
         options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
      });

var app = builder.Build();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Auth}/{action=Login}/{id?}"); // Set default controller and action

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
