using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Services.EducationalEntities;

public interface IEducationalEntitiesService
{
    Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default);
    Task<bool> CreateSchoolAsync(School school, CancellationToken token = default);
    Task<bool> CreateSubjectAsync(Subject subject, CancellationToken token = default);
    Task<bool> CreateCategoryAsync(Categories categories, CancellationToken token = default);
}