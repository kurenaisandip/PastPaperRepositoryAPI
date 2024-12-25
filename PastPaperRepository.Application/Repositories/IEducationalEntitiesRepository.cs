using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Repositories;

public interface IEducationalEntitiesRepository
{
    Task<bool> CreateSchoolAsync(School school, CancellationToken token = default);
    void CreateSubject();
    void Categories();

    Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default);
}