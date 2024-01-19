using BizLand.DAL;
using BizLand.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    //opt.Password.RequireDigit = true;
    //opt.Password.RequireLowercase = true;
    //opt.Password.RequireUppercase = true;
    //opt.Password.RequiredLength = 8;


    //opt.User.RequireUniqueEmail=true;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);
    opt.Lockout.MaxFailedAccessAttempts = 3;
    
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(cfg => { cfg.LoginPath = $"/Admin/Account/Login/{cfg.ReturnUrlParameter}"; });

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("default","{controller=home}/{action=index}/{id?}");

app.Run();
