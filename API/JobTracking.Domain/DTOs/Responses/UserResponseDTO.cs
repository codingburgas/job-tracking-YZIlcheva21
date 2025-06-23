namespace JobTracking.Domain.DTOs.Response;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public String Role { get; set; }
}