using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class PastPaperRepository : IPastPaperRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PastPaperRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    //TODOs: Handle the exception for item alerady exists
    public async Task<bool> CreatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var transaction = connection.BeginTransaction();
            
            pastPapers.PastPaperId = Guid.NewGuid().ToString();
            Console.WriteLine(pastPapers.PastPaperId);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                             insert into PastPapers (PastPaperId, Title, Slug, SubjectId, CategoryId, Year, ExamType, DifficultyLevel, ExamBoard, FilePath)
                                                                                 values (@PastPaperId, @Title, @Slug, @SubjectId, @CategoryId, @Year, @ExamType, @DifficultyLevel, @ExamBoard, @FilePath)
                                                                             """, pastPapers, cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }
    }

    public async Task<PastPapers?> GetPastPaperByIdAsync(string pastPaperId, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var pastPaper = await connection.QuerySingleOrDefaultAsync<PastPapers>(new CommandDefinition(@"
                select * from PastPapers where PastPaperId = @pastPaperId", new {pastPaperId}, cancellationToken: token));
            
            // if (pastPaper is null)
            // {
            //     return null;
            // }

            return pastPaper;  // this will return null if the pastPaper is null
        }

       
    }

    public async Task<PastPapers?> GetPastPaperBySlugAsync(string slug, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var pastPaper = await connection.QuerySingleOrDefaultAsync<PastPapers>(new CommandDefinition(@"
                select * from PastPapers where Slug = @slug", new { slug }, cancellationToken: token));
            
            return pastPaper;
        }
    }

    public async Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options,CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var pastPapers = await connection.QueryAsync<PastPapers>(new CommandDefinition(@"
            SELECT PastPaperId, Title, Slug, SubjectId, CategoryId, Year, ExamType, DifficultyLevel, ExamBoard, FilePath 
            FROM PastPapers", cancellationToken: token));
            
            return pastPapers;
        }
    }

 public async Task<bool> UpdatePastPaperAsync(PastPapers pastPapers, CancellationToken token = default)
{
    using (var connection = await _connectionFactory.CreateConnectionAsync(token))
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // if (!await ExistsById(pastPapers.PastPaperId))
                // {
                //     return false;
                // }

                var result = await connection.ExecuteAsync(new CommandDefinition(@"
                UPDATE PastPapers
                SET Title = @Title,
                    Slug = @Slug,
                    SubjectId = @SubjectId,
                    CategoryId = @CategoryId,
                    Year = @Year,
                    ExamType = @ExamType,
                    DifficultyLevel = @DifficultyLevel,
                    ExamBoard = @ExamBoard,
                    FilePath = @FilePath
                WHERE PastPaperId = @PastPaperId", pastPapers, transaction, cancellationToken: token));

                transaction.Commit();
                return result > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

   public async Task<bool> DeletePastPaperAsync(string pastPaperId, CancellationToken token = default)
{
    using (var connection = await _connectionFactory.CreateConnectionAsync(token))
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                if (!await ExistsById(pastPaperId))
                {
                    return false;
                }

                var result = await connection.ExecuteAsync(new CommandDefinition(@"
                    DELETE FROM PastPapers WHERE PastPaperId = @pastPaperId", new { pastPaperId }, transaction, cancellationToken: token));

                transaction.Commit();
                return result > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

    public async Task<bool> ExistsById(string pastPaperId, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var pastPaper = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                select count(1) from PastPapers where PastPaperId = @pastPaperId", new { pastPaperId }, cancellationToken: token));
            
            return pastPaper;
        }
    }
}