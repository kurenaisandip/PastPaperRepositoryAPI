using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repository;

public class PastPaperRepository : IPastPaperRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PastPaperRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    //TODOs: Handle the exception for item alerady exists
    public async Task<bool> CreatePaspaperAsync(PastPapers pastPapers)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                             insert into PastPapers (PastPaperId, Title, Slug, SubjectId, CategoryId, Year, ExamType, DifficultyLevel, ExamBoard, FilePath)
                                                                                 values (@PastPaperId, @Title, @Slug, @SubjectId, @CategoryId, @Year, @ExamType, @DifficultyLevel, @ExamBoard, @FilePath)
                                                                             """, pastPapers));

            transaction.Commit();
            return result > 0;
        }
    }

    public async Task<PastPapers?> GetPastPaperByIdAsync(Guid pastPaperId)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var pastPaper = await connection.QuerySingleOrDefaultAsync<PastPapers>(new CommandDefinition(@"
                select * from PastPapers where PastPaperId = @PastPaperId", new {pastPaperId}));
            
            // if (pastPaper is null)
            // {
            //     return null;
            // }

            return pastPaper;  // this will return null if the pastPaper is null
        }

       
    }

    public async Task<PastPapers?> GetPastPaperBySlugAsync(string slug)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var pastPaper = await connection.QuerySingleOrDefaultAsync<PastPapers>(new CommandDefinition(@"
                select * from PastPapers where Slug = @slug", new { slug }));
            
            return pastPaper;
        }
    }

    public async Task<IEnumerable<PastPapers>> GetAllPastPapersAsync()
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var pastPapers = await connection.QueryAsync<PastPapers>(new CommandDefinition(@"
                select * from PastPapers "));
            
            return pastPapers;
        }
    }

 public async Task<bool> UpdatePastPaperAsync(PastPapers pastPapers)
{
    using (var connection = await _connectionFactory.CreateConnectionAsync())
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                if (!await ExistsById(pastPapers.PastPaperId))
                {
                    return false;
                }

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
                WHERE PastPaperId = @PastPaperId", pastPapers, transaction));

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

   public async Task<bool> DeletePastPaperAsync(Guid pastPaperId)
{
    using (var connection = await _connectionFactory.CreateConnectionAsync())
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
                    DELETE FROM PastPapers WHERE PastPaperId = @pastPaperId", new { pastPaperId }, transaction));

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

    public async Task<bool> ExistsById(Guid pastPaperId)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        {
            var pastPaper = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                select count(1) from PastPapers where PastPaperId = @pastPaperId", new { pastPaperId }));
            
            return pastPaper;
        }
    }
}