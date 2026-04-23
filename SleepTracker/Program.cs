using Microsoft.EntityFrameworkCore;
using SleepTracker.Data;
using SleepTracker.Models;
using SleepTracker.Services;

// Creates application builder
var builder = WebApplication.CreateBuilder(args);

// Registers MVC services
builder.Services.AddControllersWithViews();

// Registers database context with SQLite connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=sleeptracker.db"));

// Registers custom service for sleep logic
builder.Services.AddScoped<ISleepService, SleepService>();

// Builds the application
var app = builder.Build();

// Production error handling and security settings
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Enables static files
app.MapStaticAssets();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Creates scope to access services during startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Applies pending migrations automatically
    context.Database.Migrate();

    // Seeds default users if database is empty
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Name = "Admin User", Role = "Admin" },
            new User { Name = "Regular User", Role = "User" }
        );
        context.SaveChanges();
    }
}

// Runs the application
app.Run();