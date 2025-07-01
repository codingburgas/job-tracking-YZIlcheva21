using JobTracking.Application.Contracts;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementation;

public class UserService : IUserService
{
    protected DependencyProvider Provider { get; set; }

    public UserService(DependencyProvider provider)
    {
        Provider = provider;
    }
    
    public async Task<List<User>> GetAllUsers(int page, int pageCount)
    {
        return await Provider.Db.Users
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }

    public Task<UserResponseDTO?> GetUser(int userId)
    {
        return Provider.Db.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Username = u.Username,
                Role = u.Role
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserResponseDTO> CreateUser(UserCreateRequestDTO dto)
    {
        if (await Provider.Db.Users.AnyAsync(u => u.Username == dto.Username))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var entity = new User
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Username = dto.Username,
            Password = AuthService.HashPassword(dto.Password),
            Role = dto.Role,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "system",
            IsActive = true
        };

        Provider.Db.Users.Add(entity);
        await Provider.Db.SaveChangesAsync();

        return new UserResponseDTO
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            MiddleName = entity.MiddleName,
            LastName = entity.LastName,
            Username = entity.Username,
            Role = entity.Role
        };
    }

    public async Task<bool> UpdateUser(UserUpdateRequestDTO dto)
    {
        var entity = await Provider.Db.Users.FindAsync(dto.Id);

        if (entity is null)
        {
            return false;
        }

        entity.FirstName = dto.FirstName;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Username = dto.Username;
        entity.Password = dto.Password;
        entity.Role = dto.Role;
        entity.UpdatedOn = DateTime.UtcNow;
        entity.UpdatedBy = "system";
        entity.IsActive = dto.IsActive;

        await Provider.Db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var entity = await Provider.Db.Users.FindAsync(userId);

        if (entity is null)
        {
            return false;
        }

        Provider.Db.Users.Remove(entity);
        await Provider.Db.SaveChangesAsync();
        return true;
    }
}