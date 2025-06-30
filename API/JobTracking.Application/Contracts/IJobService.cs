using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Filters;
using JobTracking.Domain.Filters.Base;

namespace JobTracking.Application.Contracts.Base;

public interface IJobService
{
    public Task<List<Job>> GetAllJobAds(int page, int pageCount);
    public Task<JobResponseDTO?> GetJobAd(int userId);
    public Task<JobResponseDTO> CreateJobAd(JobCreateRequestDTO dto);
    public Task<bool> UpdateJobAd(JobUpdateRequestDTO dto);
    public Task<bool> DeleteJobAd(int id);
    public Task<IQueryable<JobResponseDTO>> GetFilteredJobAds(BaseFilter<JobFilter> filter);
}