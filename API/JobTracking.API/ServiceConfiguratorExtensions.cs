using JobTracking.Application;
using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.Application.Implementation;
using JobTracking.DataAccess.Data;

namespace JobTracking.API
{
    public static class ServiceConfiguratorExtensions
    {
        public static void AddContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<AppDbContext>();
        }
 
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
        }
 
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DependencyProvider>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
 
        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient",
                    policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }
    }
}
 