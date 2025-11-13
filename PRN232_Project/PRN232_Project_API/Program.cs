using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;
using PRN232_Project_API;

namespace PRN232_Project_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DAOs
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<FriendListDAO>();
            builder.Services.AddScoped<FriendInvitationDAO>();
            builder.Services.AddScoped<BlockListDAO>();
            builder.Services.AddScoped<MessageDAO>();

            // Register repositories
            builder.Services.AddScoped<IAccusationRepository, AccusationRepository>();
            builder.Services.AddScoped<IBlockListRepository, BlockListRepository>();
            builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
            builder.Services.AddScoped<IFriendListRepository, FriendListRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Register services
            builder.Services.AddScoped<IAccusationService, AccusationService>();
            builder.Services.AddScoped<IBlockListService, BlockListService>();
            builder.Services.AddScoped<IFriendService, FriendService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMvcApp",
                    policy => policy
                        .WithOrigins("https://localhost:7180") // MVC app URL
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials() // Required for SignalR
                );
            });
            // SignalR
            builder.Services.AddSignalR();

            // Database connection
            var connectionString = builder.Configuration.GetConnectionString("MyCallioDB");
            builder.Services.AddDbContext<CallioTestContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowMvcApp");

            app.MapControllers();

            // Map SignalR hub
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
