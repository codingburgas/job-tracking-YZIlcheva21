using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums;
using JobTracking.Domain.Filters.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    
    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _jobApplicationService.GetJobApplication(id));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var result = await _jobApplicationService.GetAllJobApplications(page, pageCount);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetFiltered([FromBody] BaseFilter<JobApplicationFilter> jobApplicationFilter)
    {
        var result = await _jobApplicationService.GetFilteredJobApplications(jobApplicationFilter);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] JobApplicationCreateRequestDTO dto)
    {
        if (await _jobApplicationService.HasUserAlreadyApplied(dto.JobAdId, dto.UserId))
        {
            return BadRequest(new { message = "User has already applied to this job." });
        }

        var created = await _jobApplicationService.CreateJobApplication(dto);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobApplicationUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not found");
        }

        var success = await _jobApplicationService.UpdateJobApplication(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _jobApplicationService.DeleteJobApplication(id);
        return success ? NoContent() : NotFound();
    }
}