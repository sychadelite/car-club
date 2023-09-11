using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Helpers;
using WebAppCarClub.Models;
using WebAppCarClub.Repository;
using WebAppCarClub.Service;
using WebAppCarRace.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Repository
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Cloudinary Images
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// SQL Server Integration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity Framework
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Configure password policy
    options.Password.RequireDigit = false;  // Don't require digits
    options.Password.RequireLowercase = false;  // Don't require lowercase letters
    options.Password.RequireUppercase = false;  // Don't require uppercase letters
    options.Password.RequireNonAlphanumeric = false;  // Don't require special characters
    options.Password.RequiredLength = 6;  // Set the minimum required length
})
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    // Seed if IdentityFramework already implemented on models
    await Seed.SeedUsersAndRolesAsync(app);

    // Starter Seed
    //Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // should be called if wanna use identity auth user

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
