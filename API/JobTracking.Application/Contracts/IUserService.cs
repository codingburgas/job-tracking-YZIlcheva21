using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts;

public interface IUserService
{
    public Task<List<User>> GetAllUsers(int page, int pageCount);
    public Task<UserResponseDTO?> GetUser(int userId);
    public Task<UserResponseDTO> CreateUser(UserCreateRequestDTO dto);
    public Task<bool> UpdateUser(UserUpdateRequestDTO dto);
    public Task<bool> DeleteUser(int id);
}