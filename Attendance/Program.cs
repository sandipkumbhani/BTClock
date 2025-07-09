using Attendance.Application.Extension;
using Attendance.Application.Interface;
using Attendance.Application.Provider;
using Attendance.Infrastructure.Efcore;
using Attendance.Infrastructure.Efcore.Extensions;
using Attendance.MapperProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Cookies")
   .AddCookie("Cookies", options =>
   {
      options.LoginPath = "/Auth/Login";
      options.AccessDeniedPath = "/Auth/Login";
   });

// Configure logging
builder.Logging.ClearProviders(); // Clear default providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging
builder.Logging.AddFile("Logs/app-{Date}.txt"); // Add file logging (requires a file logging provider)


// Add other services
builder.Services.AddSingleton<IAttendanceSettingsProvider, Attendance.Infrastructure.Efcore.Providers.AttendanceSettingsProvider>();
builder.Services.AddSingleton(typeof(Attendance.Application.Interface.IAttendanceSettingsProvider),
    provider => new Attendance.Infrastructure.Efcore.Providers.AttendanceSettingsProvider(
        provider.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

// Register Quartz
builder.Services.AddQuartz(q =>
{
    // Define the job and trigger
    var jobKey = new JobKey("AttendanceQuartzJob");
    q.AddJob<AttendanceQuartzJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("AttendanceQuartzJob-trigger")
        .WithCronSchedule("0 0 0 * * ?", x => x.InTimeZone(TimeZoneInfo.Local))); // Every day at midnight (12:00 AM) local time
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddApplicationServices();
builder.Services.AddAttendanceServices();
builder.Services.AddAutoMapper(typeof(AutoMapperRegister));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
   ServiceLifetime.Scoped); 
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
