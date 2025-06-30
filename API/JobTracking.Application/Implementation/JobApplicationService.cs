using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.DataAccess;
using JobTracking.Domain.DTOs;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums;
using JobTracking.Domain.Filters.Base;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementation;

public class JobApplicationService : IJobApplicationService
{
    protected DependencyProvider Provider { get; set; }
    
    public JobApplicationService(DependencyProvider provider)
    {
        Provider = provider;
    }

    public async Task<List<JobApplication>> GetAllJobApplications(int page, int pageCount)
    {
        return await Provider.Db.JobApplications
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }
    
    public async Task<JobApplicationResponseDTO?> GetJobApplication(int jobApplicationId)
    {
        return await Provider.Db.JobApplications
            .Include(j => j.User)
            .Include(j => j.Job)
            .Where(j => j.Id == jobApplicationId)
            .Select(j => new JobApplicationResponseDTO
            {
                Id = j.Id,
                UserId = j.UserId,
                JobAdId = j.JobAdId,
                Status = j.Status,
                User = new UserResponseDTO
                {
                    Id = j.User.Id,
                    FirstName = j.User.FirstName,
                    MiddleName = j.User.MiddleName,
                    LastName = j.User.LastName,
                    Username = j.User.Username,
                    Role = j.User.Role
                },
                Job = new JobResponseDTO
                {
                    Id = j.Job.Id,
                    Title = j.Job.Title,
                    CompanyName = j.Job.CompanyName,
                    Description = j.Job.Description,
                    PublishedOn = j.Job.PublishedOn,
                    IsOpen = j.Job.IsOpen
                }
            })
            .FirstOrDefaultAsync();
    }

    
    public async Task<PagedResult<JobApplicationResponseDTO>> GetFilteredJobApplications(BaseFilter<JobApplicationFilter> filter)
    {
        IQueryable<JobApplication> query = Provider.Db.JobApplications;

        var jobApplicationFilter = filter.Filters;

        if (jobApplicationFilter is not null)
        {
            if (!string.IsNullOrEmpty(jobApplicationFilter.Status))
            {
                query = query.Where(j => j.Status == jobApplicationFilter.Status);
            }
            if (jobApplicationFilter.UserId.HasValue)
            {
                query = query.Where(j => j.UserId == jobApplicationFilter.UserId.Value);
            }
            if (jobApplicationFilter.JobAdId.HasValue)
            {
                query = query.Where(j => j.JobAdId == jobApplicationFilter.JobAdId.Value);
            }
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize)
            .Select(x => new JobApplicationResponseDTO
            {
                Id = x.Id,
                Status = x.Status,
                UserId = x.UserId,
                JobAdId = x.JobAdId,
                User = new UserResponseDTO()
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    MiddleName = x.User.MiddleName,
                    LastName = x.User.LastName,
                    Username = x.User.Username,
                    Role = x.User.Role,
                },
                Job = new JobResponseDTO()
                {
                    Id = x.Job.Id,
                    Title = x.Job.Title,
                    CompanyName = x.Job.CompanyName,
                }
            })
            .ToListAsync();

        return new PagedResult<JobApplicationResponseDTO>
        {
            TotalItems = totalItems,
            Items = items
        };
    }
    
    public async Task<bool> HasUserAlreadyApplied(int jobId, int userId)
    {
        if (jobId <= 0 || userId <= 0)
        {
            throw new ArgumentException("Invalid jobId or userId");
        }

        if (Provider?.Db?.JobApplications is null)
        {
            throw new InvalidOperationException("Database context or JobApplications set is null");
        }

        return await Provider.Db.JobApplications
            .AnyAsync(a => a.JobAdId == jobId && a.UserId == userId);
    }

    public async Task<JobApplicationResponseDTO> CreateJobApplication(JobApplicationCreateRequestDTO dto)
    {
        var exists = await Provider.Db.JobApplications
            .AnyAsync(a => a.UserId == dto.UserId && a.JobAdId == dto.JobAdId);

        if (exists)
        {
            throw new InvalidOperationException("User has already applied to this job.");
        }
        
        var entity = new JobApplication
        {
            UserId = dto.UserId,
            JobAdId = dto.JobAdId,
            Status = dto.Status,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "system",
            IsActive = true
        };

        Provider.Db.JobApplications.Add(entity);
        await Provider.Db.SaveChangesAsync();

        return new JobApplicationResponseDTO
        {
            Id = entity.Id,
            UserId = entity.UserId,
            JobAdId = entity.JobAdId,
            Status = entity.Status
        };
    }

    public async Task<bool> UpdateJobApplication(JobApplicationUpdateRequestDTO dto)
    {
        var entity = await Provider.Db.JobApplications.FindAsync(dto.Id);
        if (entity is null)
        {
            return false;
        }
        
        entity.Status = dto.Status;

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        entity.UpdatedOn = DateTime.UtcNow;
        entity.UpdatedBy = "system";

        await Provider.Db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteJobApplication(int id)
    {
        var entity = await Provider.Db.JobApplications.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        Provider.Db.JobApplications.Remove(entity);
        await Provider.Db.SaveChangesAsync();
        return true;
    }
}