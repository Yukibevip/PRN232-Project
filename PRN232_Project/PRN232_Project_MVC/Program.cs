using Microsoft.EntityFrameworkCore;
using PRN232_Project_MVC.Hubs;
using PRN232_Project_MVC.Models;
using Services;
using Services.Interfaces;
using System;

namespace PRN232_Project_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IAccusationService, PRN232_Project_MVC.ServicesMVC.AccusationService>();
            builder.Services.AddScoped<IBlockListService, BlockListService>();
            builder.Services.AddScoped<IFriendInvitationService, FriendInvitationService>();
            builder.Services.AddScoped<IFriendListService, FriendListService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddSignalR();

            // APIService registration (if present)
            var apiBase = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7098";
            builder.Services.AddHttpClient<APIService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            // Register AdminService as a typed HTTP client implementing IAdminService
            builder.Services.AddHttpClient<IAdminService, AdminService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.IAccusationService, PRN232_Project_MVC.ServicesMVC.AccusationService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            // Enable session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // <-- must be before UseAuthorization

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<VideoChatHub>("/videoChatHub");

            app.Run();
        }
    }
}
