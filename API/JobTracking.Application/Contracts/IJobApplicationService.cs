using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums;
using JobTracking.Domain.Filters.Base;

namespace JobTracking.Application.Contracts.Base;

public interface IJobApplicationService
{
    public Task<List<JobApplication>> GetAllJobApplications(int page, int pageCount);
    public Task<JobApplicationResponseDTO?> GetJobApplication(int jobApplicationId);
    public Task<bool> HasUserAlreadyApplied(int jobId, int userId);
    public Task<JobApplicationResponseDTO> CreateJobApplication(JobApplicationCreateRequestDTO dto);
    public Task<bool> UpdateJobApplication(JobApplicationUpdateRequestDTO dto);
    public Task<bool> DeleteJobApplication(int id);
    public Task<PagedResult<JobApplicationResponseDTO>> GetFilteredJobApplications(
        BaseFilter<JobApplicationFilter> filter);
}