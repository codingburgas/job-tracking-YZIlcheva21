using JobTracking.DataAccess.Data;

namespace JobTracking.Application;

public class DependencyProvider
{
    public DependencyProvider(AppDbContext dbContext)
    {
        Db = dbContext;
    }

    public AppDbContext Db { get; set; }
}