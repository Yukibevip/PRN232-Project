using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using PRN232_Project_MVC.AutoMapping;
using PRN232_Project_MVC.Hubs;
using PRN232_Project_MVC.Models;
using Repositories;
using Repositories.Interfaces;
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

            var connectionString = builder.Configuration.GetConnectionString("MyCallioDB")
                                   ?? builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'MyCallioDB' or 'DefaultConnection' is not configured.");

            builder.Services.AddDbContext<CallioTestContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IAccusationService, PRN232_Project_MVC.ServicesMVC.AccusationService>();
            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IFriendInvitationService, PRN232_Project_MVC.ServicesMVC.FriendInvitationService>();
            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IFriendListService, PRN232_Project_MVC.ServicesMVC.FriendListService>();
            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IBlockListService, PRN232_Project_MVC.ServicesMVC.BlockListService>();
            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.IMessageService, PRN232_Project_MVC.ServicesMVC.MessageService>();
            builder.Services.AddScoped<PRN232_Project_MVC.ServicesMVC.Interfaces.ILogService, PRN232_Project_MVC.ServicesMVC.LogService>();
            builder.Services.AddScoped<IFriendService, FriendService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
            builder.Services.AddScoped<IFriendListRepository, FriendListRepository>();
            builder.Services.AddScoped<IBlockListRepository, BlockListRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            builder.Services.AddScoped<FriendInvitationDAO>();
            builder.Services.AddScoped<FriendListDAO>();
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<BlockListDAO>();
            builder.Services.AddScoped<MessageDAO>();
            builder.Services.AddScoped<LogDAO>();


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
            
            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.IFriendInvitationService, PRN232_Project_MVC.ServicesMVC.FriendInvitationService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.IFriendListService, PRN232_Project_MVC.ServicesMVC.FriendListService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.IBlockListService, PRN232_Project_MVC.ServicesMVC.BlockListService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.IMessageService, PRN232_Project_MVC.ServicesMVC.MessageService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            });

            builder.Services.AddHttpClient<PRN232_Project_MVC.ServicesMVC.Interfaces.ILogService, PRN232_Project_MVC.ServicesMVC.LogService>(client =>
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
