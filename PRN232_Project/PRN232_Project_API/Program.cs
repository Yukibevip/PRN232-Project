
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAccusationRepository, AccusationRepository>();
            builder.Services.AddScoped<IBlockListRepository, BlockListRepository>();
            builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
            builder.Services.AddScoped<IFriendListRepository, FriendListRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IAccusationService, AccusationService>();
            builder.Services.AddScoped<IBlockListService, BlockListService>();
            builder.Services.AddScoped<IFriendInvitationService, FriendInvitationService>();
            builder.Services.AddScoped<IFriendListService, FriendListService>();
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
