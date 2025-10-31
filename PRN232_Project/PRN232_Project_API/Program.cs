using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
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

            // Register EF DbContext (replace connection name and provider as needed)
            builder.Services.AddDbContext<CallioTestContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAccusationRepository, AccusationRepository>();
            builder.Services.AddScoped<IBlockListRepository, BlockListRepository>();
            builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
            builder.Services.AddScoped<IFriendListRepository, FriendListRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IAccusationService, AccusationService>();
            builder.Services.AddScoped<IBlockListService, BlockListService>();
            builder.Services.AddScoped<IFriendService, FriendService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            //db connection
            var connectionString = builder.Configuration.GetConnectionString("MyCallioDB");
            builder.Services.AddDbContext<CallioTestContext>(options =>
            options.UseSqlServer(connectionString));
            //end db connection
            // Add Swagger for API testing
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // End Swagger configuration
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
