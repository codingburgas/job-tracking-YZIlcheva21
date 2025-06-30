using System.Threading.Tasks;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.DataAccess;
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

    public async Task<UserResponseDTO?> AuthenticateAsync(string username, string password)
    {
        var hashedPassword = PasswordHasher.HashPassword(password);

        var userEntity = await _context.Users
            .Where(u => u.Username == username && u.Password == hashedPassword)
            .FirstOrDefaultAsync();

        if (userEntity is null)
        {
            return null;
        }

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