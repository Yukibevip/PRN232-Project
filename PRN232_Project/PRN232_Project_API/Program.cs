using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using PRN232_Project_API.Services;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

namespace PRN232_Project_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // optional: load configuration file
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DB connection (use one registration only)
            var connectionString = builder.Configuration.GetConnectionString("MyCallioDB")
                                   ?? builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'MyCallioDB' or 'DefaultConnection' is not configured.");

            builder.Services.AddDbContext<CallioTestContext>(options =>
                options.UseSqlServer(connectionString));

            // Register DAOs (they depend on CallioTestContext)
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<BlockListDAO>();
            builder.Services.AddScoped<AccusationDAO>();
            builder.Services.AddScoped<FriendListDAO>();
            builder.Services.AddScoped<FriendInvitationDAO>();
            builder.Services.AddScoped<LogDAO>();
            builder.Services.AddScoped<MessageDAO>();
            // add other DAOs here if you have them

            // Register repositories (they depend on DAOs)
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
            builder.Services.AddScoped<IFriendInvitationService, FriendInvitationService>();
            builder.Services.AddScoped<IFriendListService, FriendListService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<QIUserService, QUserService>();


            // Admin service (typed http client) if needed

            var app = builder.Build();

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
            app.MapControllers();
            app.Run();
        }
    }
}