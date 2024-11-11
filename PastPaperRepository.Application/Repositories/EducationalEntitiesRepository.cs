using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Repositories;

public class EducationalEntitiesRepository: IEducationalEntitiesRepository
{
    private new List<Roles> data = new();
    
    public void CreateSchool()
    {
        throw new NotImplementedException();
    }

    public void CreateSubject()
    {
        throw new NotImplementedException();
    }

    public void Categories()
    {
        throw new NotImplementedException();
    }

    public  Task<bool> CreateRoleAsync(Roles roles)
    {
        if (roles is null)
        {
            return Task.FromResult(false);
        }
         data.Add(roles);

        return Task.FromResult(true);;
    }
}