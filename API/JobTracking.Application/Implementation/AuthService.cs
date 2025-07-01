using System.Security.Cryptography;
using System.Text;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data;
using JobTracking.Domain.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementation;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }
    
    public static string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(bytes);
            
        StringBuilder builder = new StringBuilder();
        foreach (var b in hashBytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

    public async Task<UserResponseDTO?> AuthenticateAsync(string username, string password)
    {
        var hashedPassword = HashPassword(password);
        var userEntity = await _context.Users
            .Where(u => u.Username == username && u.Password == hashedPassword)
            .FirstOrDefaultAsync();

        if (userEntity is null) return null;
        
        return new UserResponseDTO
        {
            Id = userEntity.Id,
            FirstName = userEntity.FirstName,
            MiddleName = userEntity.MiddleName,
            LastName = userEntity.LastName,
            Username = userEntity.Username,
            Role = userEntity.Role
        };
    }
}