using System.ComponentModel.DataAnnotations;

namespace JobTracking.Domain.DTOs.Request.Update;

public class JobUpdateRequestDTO
{
    [Required]
    public int Id { get; set; }

    [Required, StringLength(128)]
    public string? Title { get; set; }

    [Required, StringLength(128)]
    public string? CompanyName { get; set; }

    [Required, StringLength(1024)]
    public string? Description { get; set; }

    public DateTime? PublishedOn { get; set; }

    public bool? IsOpen { get; set; }

    public bool? IsActive { get; set; }
}