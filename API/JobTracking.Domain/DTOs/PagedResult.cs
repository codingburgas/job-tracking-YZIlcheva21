namespace JobTracking.Domain.DTOs;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int TotalItems { get; set; }
    public List<T> Items { get; set; }
}