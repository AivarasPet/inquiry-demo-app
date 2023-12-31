using Application.Repositories;
using Application.Services.InquiriesService;
using Application.Services.UserService;
using Application.Services.UserTokenService;
using Infrastructure.BaseDbContexts;
using Infrastructure.Repositories;
using InquiryStatusUpdater.Configuration;
using InquiryStatusUpdater.SignalRHubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InquiryStatusUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();
            builder.Services.AddControllers();

            builder.Services.Configure<AppConfiguration>(builder.Configuration);
            AppConfiguration config = builder.Configuration.Get<AppConfiguration>();
            builder.Services.AddSingleton<AppConfiguration>(config);

            var key = Encoding.ASCII.GetBytes(config.JwtSecretKey);

            // Copy the same auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (config.SignalRConfiguration.SignalRConnectionPath != null)
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddDbContext<InquiryDbContext>(options =>
                options.UseNpgsql(config.ConnectionString));

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IInquiriesRepository, InquiriesRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IInquiriesService, InquiriesService>();
            builder.Services.AddScoped<IUserTokenService, UserTokenService>();
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                // Configure SignalR endpoints
                endpoints.MapHub<NotifyHub>("/" + config.SignalRConfiguration.SignalRConnectionPath);
            });

            app.Run();
        }
    }
}