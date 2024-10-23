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

    public Task<IEnumerable<PastPapers>> GetAllPastPapersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdatePastPaperAsync(PastPapers pastPapers)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeletePastPaperAsync(Guid pastPaperId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsById(Guid pastPaperId)
    {
        throw new NotImplementedException();
    }
}