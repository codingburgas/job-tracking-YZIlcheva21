using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    
    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.AuthenticateAsync(request.Username, request.Password);
        if (user is null) return Unauthorized("Invalid credentials");
        
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var users = await _userService.GetAllUsers(page, pageCount);
        var response = users.Select(u => new UserResponseDTO
        {
            Id = u.Id,
            FirstName = u.FirstName,
            MiddleName = u.MiddleName,
            LastName = u.LastName,
            Username = u.Username,
            Role = u.Role
        });

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _userService.GetUser(id));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserCreateRequestDTO dto)
    {
        var user = await _userService.CreateUser(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not found");
        }

        var success = await _userService.UpdateUser(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _userService.DeleteUser(id);
        return success ? NoContent() : NotFound();
    }
}