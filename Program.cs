using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// ? Add Identity services (User, Roles, etc.)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ? Add Authorization
builder.Services.AddAuthorization();

// ? Add Razor Pages (required for .WithStaticAssets() to work)
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// ? Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// ? Map Razor Pages
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
