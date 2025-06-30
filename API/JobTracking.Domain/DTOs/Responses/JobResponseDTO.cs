namespace JobTracking.Domain.DTOs.Response;

public class JobResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public DateTime PublishedOn { get; set; }
    public bool IsOpen { get; set; }
}