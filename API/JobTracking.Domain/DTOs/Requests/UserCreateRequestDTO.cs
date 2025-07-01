using System.ComponentModel.DataAnnotations;

namespace JobTracking.Domain.DTOs.Request.Create;

public class UserCreateRequestDTO
{
    [Required, StringLength(64)]
    public string FirstName { get; set; }

    [Required, StringLength(64)]
    public string MiddleName { get; set; }

    [Required, StringLength(64)]
    public string LastName { get; set; }

    [Required, StringLength(32)]
    public string Username { get; set; }

    [Required, StringLength(128)]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }
}