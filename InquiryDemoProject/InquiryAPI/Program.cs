using Application.Repositories;
using Application.Services.InquiriesService;
using Application.Services.UserService;
using Application.Services.UserTokenService;
using Infrastructure.BaseDbContexts;
using Infrastructure.Repositories;
using InquiryAPI.Configuration;
using InquiryAPI.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InquiryAPI
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
            builder.Services.AddSingleton<AppConfiguration>(config);

            builder.Services.AddDbContext<InquiryDbContext>(options =>
            {
                options.UseNpgsql(config.ConnectionString);
            });

            var key = Encoding.ASCII.GetBytes(config.JwtSecretKey);

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
            });

            builder.Services.AddDbContext<InquiryDbContext>(options =>
                options.UseNpgsql(config.ConnectionString));

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IInquiriesRepository, InquiriesRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IInquiriesService, InquiriesService>();
            builder.Services.AddScoped<IUserTokenService, UserTokenService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseUserIdMiddleware();

            app.MapControllers();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                InquiryDbContext dbContext = services.GetRequiredService<InquiryDbContext>();
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}