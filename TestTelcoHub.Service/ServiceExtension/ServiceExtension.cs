using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Text;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Infastruture.Repository;
using TestTelcoHub.Model.Data;


namespace TestTelcoHub.Service.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApiDbContext>(ops
                => ops.UseSqlServer(configuration.GetConnectionString("DefaultConnect")));
            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>()
                .AddDefaultTokenProviders();

            //var logger = new LoggerConfiguration()
            //   .WriteTo.MSSqlServer(
            //       connectionString: configuration.GetConnectionString("DefaultConnect"),
            //       tableName: "ChangeLog",
            //       autoCreateSqlTable: false
            //   ).CreateLogger();
            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddSerilog(logger);
            //});

            //Authetication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? ""))
                };
            })
            .AddCookie(options =>
            {
                // Cấu hình Cookie Authentication
                options.ExpireTimeSpan = TimeSpan.FromDays(2); // Thời gian sống của Cookie
                options.SlidingExpiration = true; // Tự động gia hạn thời gian sống khi có hoạt động
            });
            //services.AddScoped<IPurchaseHistoryRepository, PurchaseHistoryRepository>();
            //services.AddScoped<IBillingPlanRepository, BillingPlanRepository>();
            //services.AddScoped<IRefreshTokenRepository, RefreshTokenRipository>();
            //services.AddScoped<ILogRepository, LogRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.Scan(scan => scan
                .FromAssemblyOf<GenericRepository<object>>()
                .AddClasses(classes => classes
                    .Where(type => type.IsClass
                        && !type.IsAbstract
                        && type.GetInterfaces().Any(i =>
                            i.IsGenericType
                            && i.GetGenericTypeDefinition() == typeof(IGenericRepository<>))))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            services.AddTransient<SmtpClient>();
            services.AddTransient<HttpClient>();

            //services.AddSingleton<DapperContext>();
            return services;
        }
    }
}

