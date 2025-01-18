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
                                                                             """, pastPapers,
                cancellationToken: token));

            if (pastPapers.Questions?.Any() == true)
            {
                foreach (var question in pastPapers.Questions)
                {
                    if (question.Answers?.Any() == true)
                    {
                        foreach (var answer in question.Answers)
                        {
                            await connection.ExecuteAsync(
                                new CommandDefinition("""
                                                      INSERT INTO QuestionAnswers (
                                                          pastpaperId, question_number, question, answer
                                                      )
                                                      VALUES (
                                                          @PastPaperId, @QuestionNumber, @Question, @Answer
                                                      )
                                                      """,
                                    new
                                    {
                                        PastPaperId = pastPapers.PastPaperId,
                                        QuestionNumber = question.QuestionNumber,
                                        Question = question.Questtion,
                                        Answer = answer.Content
                                    },
                                    transaction: transaction,
                                    cancellationToken: token)
                            );
                        }
                    }
                }
            }


            transaction.Commit();
            return result > 0;
        }
    }

    public async Task<IEnumerable<QuestionAnswer>?> GetPastPaperByIdAsync(string pastpaperid, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            // var pastPaper = await connection.QuerySingleOrDefaultAsync<PastPapers>(new CommandDefinition(@"
            //     select * from PastPapers where PastPaperId = @pastPaperId", new { pastPaperId },
            //     cancellationToken: token));
            //
            var pastPapers = await connection.QueryAsync<QuestionAnswer>(new CommandDefinition(@"
                select question_number as id, pastpaperid as pastPaperId, question, answer from QuestionAnswers where pastpaperid = @pastpaperid order by question_number", new { pastpaperid },
                cancellationToken: token));
            
            return pastPapers; 
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

    public async Task<IEnumerable<PastPapers>> GetAllPastPapersAsync(GetAllPastPapersOptions options,
        CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var query = @"
        SELECT PastPaperId, Title, Slug, SubjectId, CategoryId, Year, ExamType, DifficultyLevel, ExamBoard, FilePath
        FROM PastPapers
        WHERE (@Year IS NULL OR Year = @Year)
        AND (@Title IS NULL OR Title LIKE '%' || @Title || '%')";

            if (!string.IsNullOrEmpty(options.SortField))
            {
                var sortOrder = options.SortOrder == SortOrder.Descending ? "DESC" : "ASC";
                query += $" ORDER BY {options.SortField} {sortOrder}";
            }

            query += " LIMIT @pageSize OFFSET @pageOffSet";

            var pastPapers = await connection.QueryAsync<PastPapers>(new CommandDefinition(query,
                new
                {
                    options.Year, options.Title, pageSize = options.PageSize,
                    pageOffSet = (options.Page - 1) * options.PageSize
                }, cancellationToken: token));

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
                    if (!await ExistsById(pastPaperId)) return false;

                    var result = await connection.ExecuteAsync(new CommandDefinition(@"
                    DELETE FROM PastPapers WHERE PastPaperId = @pastPaperId", new { pastPaperId }, transaction,
                        cancellationToken: token));

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
                select count(1) from PastPapers where PastPaperId = @pastPaperId", new { pastPaperId },
                cancellationToken: token));

            return pastPaper;
        }
    }

    public async Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            return await connection.QuerySingleAsync<int>(new CommandDefinition(@"
            select count(1) from PastPapers where (@Year IS NULL OR Year = @Year) AND (@Title IS NULL OR Title LIKE '%' || @Title || '%')",
                new { Year = year, Title = title }, cancellationToken: token));
        }
    }

    public async Task<IEnumerable<DynamicPastPaperModal>> GetDynamicPastPapersAsync(int id,
        CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var query = @"
              SELECT p.PastPaperId, p.Title, p.Year, s.name as Subject
FROM PastPapers as p
         JOIN Semester ON p.semester_id = Semester.semester_id
         JOIN LoggedInUser AS l ON Semester.semester_id = l.optional_subject
JOIN Subject as s on p.SubjectId = s.subject_id
WHERE l.user_id = @id;
            ";

                    var result = await connection.QueryAsync<DynamicPastPaperModal>(
                        new CommandDefinition(query, new { id }, transaction, cancellationToken: token));

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}