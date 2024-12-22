using System.Transactions;
using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models.SpacedRepetition;
using QuestionAnswers = PastPaperRepository.Application.Services.QuestionAnswers;

namespace PastPaperRepository.Application.Repositories;

public class SpacedRepetitionRepository: ISpacedRepetitionRepository
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
                            """, request, transaction: transaction, cancellationToken: token));

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
            throw new Exception("Failed to add to LearningDeck", e); // Wrap with context but preserve original exception
        }
    }


    public Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<QuestionAnswers>> ShowQuestionAnswerAsync(string UserId, long pastPaperId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}