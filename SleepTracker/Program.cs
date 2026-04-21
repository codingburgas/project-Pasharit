using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.Models;
using SleepTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=sleeptracker.db"));

builder.Services.AddScoped<ISleepService, SleepService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Name = "Admin User", Role = "Admin" },
            new User { Name = "Regular User", Role = "User" }
        );
        context.SaveChanges();
    }
}

app.Run();