using PastPaperRepository.Application.Models.EducationalEntities;
using PastPaperRepository.Application.Repositories;

namespace PastPaperRepository.Application.Services.EducationalEntities;

public class EducationalEntitiesService : IEducationalEntitiesService
{
    private readonly IEducationalEntitiesRepository _repository;

    public EducationalEntitiesService(IEducationalEntitiesRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default)
    {
        return _repository.CreateRoleAsync(roles, token);
    }

    public Task<bool> CreateSchoolAsync(School school, CancellationToken token = default)
    {
        return _repository.CreateSchoolAsync(school, token);
    }

    public Task<bool> CreateSubjectAsync(Subject subject, CancellationToken token = default)
    {
        return _repository.CreateSubjectAsync(subject, token);
    }

    public Task<bool> CreateCategoryAsync(Categories categories, CancellationToken token = default)
    {
        return _repository.CreateCategoryAsync(categories, token);
    }
}