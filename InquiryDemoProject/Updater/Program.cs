using Application.Repositories;
using Application.Services.UserService;
using Infrastructure.BaseDbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Updater.Configuration;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<AppConfiguration>(builder.Configuration);
            AppConfiguration config = builder.Configuration.Get<AppConfiguration>();

            builder.Services.AddDbContext<InquiryDbContext>(options =>
            {
                options.UseNpgsql(config.ConnectionString);
            });

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            var key = Encoding.ASCII.GetBytes(config.JwtSecretKey);

            var app = builder.Build();

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