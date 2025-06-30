using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Response;

public class JobApplicationResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JobAdId { get; set; }
    public string? Status { get; set; }
    
    public UserResponseDTO User { get; set; }
    public JobResponseDTO Job { get; set; }
}