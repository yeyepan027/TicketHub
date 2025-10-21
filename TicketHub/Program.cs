using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TicketHub.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add EF Core DbContext
builder.Services.AddDbContext<TicketHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketHubContext")
    ?? throw new InvalidOperationException("Connection string 'TicketHubContext' not found.")));

// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// Add User Secrets 
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // redirect HTTP to HTTPS
app.UseStaticFiles(); // for serving static files like CSS, JS, images

app.UseRouting(); // for routing

app.UseAuthentication(); // for cookie authentication
app.UseAuthorization(); // for authorization

app.MapControllerRoute( // default route
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
