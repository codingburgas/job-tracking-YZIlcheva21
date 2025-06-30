using JobTracking.Domain.Filters.Base;

namespace JobTracking.Domain.Filters;

public class JobFilter : IFilter
{
    public string? Title { get; set; }
    public string? CompanyName { get; set; }
    public DateTime? PublishedOn { get; set; }
    public bool? IsOpen { get; set; }
}