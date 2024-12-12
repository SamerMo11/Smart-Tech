using E_CommerceWeb.Data;
using E_CommerceWeb.Models;
using E_CommerceWeb.Repository.Interface;
using E_CommerceWeb.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_CommerceWeb.ViewModels.Email;
using Stripe;

namespace E_CommerceWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //Register DbContext
            builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(
                builder.Configuration.GetConnectionString("conn"))
                );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.Password.RequireLowercase = false;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
                op.Password.RequiredLength = 4;
                op.Password.RequireDigit = false;
            }).AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();

            // Add Services 
            builder.Services.AddScoped<IHomeRepo, HomeRepo>();
            builder.Services.AddScoped<IEmailRepo, EmailRepo>();
            builder.Services.AddScoped<IAccountRepo, AccountRepo>();
            builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();

            //configure Cookie
            builder.Services.ConfigureApplicationCookie(op => {
                op.ExpireTimeSpan = TimeSpan.FromDays(7);
                op.LoginPath = "/Account/Login";
                op.AccessDeniedPath = "/Account/AccessDenied";
            });

            // configure Session
            builder.Services.AddSession(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always in production
                options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin
            });

            //configure Stripe
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:secretkey").Get<string>();
            builder.Services.Configure<EmailConfigureModel>(builder.Configuration.GetSection("EmailConfigure"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Home}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
