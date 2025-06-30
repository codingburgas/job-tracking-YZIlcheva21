using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts.Base;

public interface IAuthService
{
    public Task<UserResponseDTO?> AuthenticateAsync(string username, string password);
}