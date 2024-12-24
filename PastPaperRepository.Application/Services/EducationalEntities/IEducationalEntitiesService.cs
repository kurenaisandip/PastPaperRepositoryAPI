using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Services.EducationalEntities;

public interface IEducationalEntitiesService
{
    Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default);
}