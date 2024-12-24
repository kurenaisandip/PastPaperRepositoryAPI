using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Repositories;

public interface IEducationalEntitiesRepository
{
    void CreateSchool();
    void CreateSubject();
    void Categories();

    Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default);
}