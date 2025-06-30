using System.ComponentModel.DataAnnotations;
using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Request.Create;

public class JobApplicationCreateRequestDTO
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int JobAdId { get; set; }

    [Required]
    public string? Status { get; set; }
}