using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models.SpacedRepetition;

namespace PastPaperRepository.Application.Repositories;

public class SpacedRepetitionRepository : ISpacedRepetitionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SpacedRepetitionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> AddToLearningDeckAsync(LearningDeck request, CancellationToken token = default)
    {
        try
        {
            using (var connection = await _connectionFactory.CreateConnectionAsync(token))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                INSERT INTO LearningDeck (pastpaperid, status, datetime, user_id, nextReviewDateTime)
                                VALUES (@PastPaperId, @Status, @AddedDate, @UserId, @NextReviewDate)
                            """, request, transaction, cancellationToken: token));

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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Failed to add to LearningDeck",
                e); // Wrap with context but preserve original exception
        }
    }


    public async Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token = default)
    {
        try
        {
            using (var connection = await _connectionFactory.CreateConnectionAsync(token))
            {
                var sql = @"
                SELECT 
                    p.PastPaperId,
                    p.Title,
                    COUNT(q.Id) AS TotalQuestions
                FROM LearningDeck ld
                JOIN PastPapers p ON ld.PastPaperId = p.PastPaperId
                JOIN QuestionAnswers q ON ld.PastPaperId = q.PastPaperId
                WHERE ld.user_id = @UserId
                GROUP BY p.PastPaperId, p.Title";

                var result =
                    await connection.QueryAsync<DeckViewModel>(new CommandDefinition(sql, new { UserId = userId },
                        cancellationToken: token));
                return result.ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<QuestionAnswers>> ShowQuestionAnswerAsync(long userId, string pastPaperId,
        CancellationToken token = default)
    {
        try
        {
            using (var connection = await _connectionFactory.CreateConnectionAsync(token))
            {
                var sql = @"
               select distinct q.question_number, q.question, q.answer, ld.score
FROM LearningDeck ld
join QuestionAnswers as q on ld.pastpaperid = q.pastpaperid
where ld.user_id = @UserId and ld.pastpaperid = @PastPaperId";

                var result = await connection.QueryAsync<QuestionAnswers>(
                    new CommandDefinition(sql, new { UserId = userId, PastPaperId = pastPaperId },
                        cancellationToken: token)
                );

                return result.ToList();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}