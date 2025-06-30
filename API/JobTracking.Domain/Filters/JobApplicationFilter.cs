using JobTracking.Domain.Enums;

namespace JobTracking.Domain.Filters.Base;

public class JobApplicationFilter : IFilter
{
    public int? UserId { get; set; }
    public int? JobAdId { get; set; }
    public string? Status { get; set; }
}