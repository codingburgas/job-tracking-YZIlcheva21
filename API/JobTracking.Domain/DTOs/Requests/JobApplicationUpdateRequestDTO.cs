using System.ComponentModel.DataAnnotations;
using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Request.Update;

public class JobApplicationUpdateRequestDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string? Status { get; set; }

    public bool? IsActive { get; set; }
}