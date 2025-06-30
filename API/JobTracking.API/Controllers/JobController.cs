using JobTracking.Application.Contracts.Base;
using JobTracking.Domain.DTOs;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Filters;
using JobTracking.Domain.Filters.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobController : Controller
{
    private readonly IJobService _jobService;
    
    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _jobService.GetJobAd(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var result = await _jobService.GetAllJobAds(page, pageCount);

        var response = result.Select(j => new JobResponseDTO
        {
            Id = j.Id,
            Title = j.Title,
            CompanyName = j.CompanyName,
            Description = j.Description,
            PublishedOn = j.PublishedOn,
            IsOpen = j.IsOpen
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> GetFiltered([FromBody] BaseFilter<JobFilter> jobFilter)
    {
        var query = await _jobService.GetFilteredJobAds(jobFilter);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip(jobFilter.PageSize * (jobFilter.Page - 1))
            .Take(jobFilter.PageSize)
            .ToListAsync();

        var response = new PagedResult<JobResponseDTO>
        {
            TotalCount = totalCount,
            Items = items
        };

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] JobCreateRequestDTO dto)
    {
        var created = await _jobService.CreateJobAd(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var success = await _jobService.UpdateJobAd(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _jobService.DeleteJobAd(id);
        return success ? NoContent() : NotFound();
    }
}