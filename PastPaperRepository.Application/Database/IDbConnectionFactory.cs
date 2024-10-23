using System.Data;

namespace PastPaperRepository.Application.Database;

public interface IDbConnectionFactory
{
     Task<IDbConnection> CreateConnectionAsync();
    
}


