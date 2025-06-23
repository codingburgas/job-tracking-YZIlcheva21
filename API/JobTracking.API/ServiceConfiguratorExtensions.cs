using JobTracking.Application;
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
            /*builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();*/
        }
 
        public static void AddCors(this WebApplicationBuilder builder)
        {
        }
    }
}
