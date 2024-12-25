using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models.EducationalEntities;

namespace PastPaperRepository.Application.Repositories;

public class EducationalEntitiesRepository: IEducationalEntitiesRepository
{
   private readonly IDbConnectionFactory _dbConnectionFactory;

   public EducationalEntitiesRepository(IDbConnectionFactory dbConnectionFactory)
   {
       _dbConnectionFactory = dbConnectionFactory;
   }

    public async Task<bool> CreateSchoolAsync(School school, CancellationToken token = default)
    {
        using (var connection = await _dbConnectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = "Insert into School (name, location) values (@name, @location)";
                    
                    var result = await connection.ExecuteAsync(new CommandDefinition(query, new {name = school.Name, location = school.Address}, transaction, cancellationToken:token));
                    transaction.Commit();
                    
                    return result > 0;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    public void CreateSubject()
    {
        throw new NotImplementedException();
    }

    public void Categories()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateRoleAsync(Roles roles, CancellationToken token = default)
    {
        using (var connection = await _dbConnectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = "Insert into Roles (role) values (@role)";
                    
                    var result = await connection.ExecuteAsync(new CommandDefinition(query, new {role = roles.Name}, transaction, cancellationToken:token));
                    transaction.Commit();
                    
                    return result > 0;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}