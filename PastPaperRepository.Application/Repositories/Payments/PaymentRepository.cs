using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models.Payment;

namespace PastPaperRepository.Application.Repositories.Payments;

public class PaymentRepository: IPayementRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PaymentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreatePaymentAsync(CreatePaymentModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                INSERT INTO Payments (amount, user_id, valid_until, product_name)
                                VALUES (@Price, @UserId, @ValidUntil, @ProductName)
                            """, model, transaction: transaction, cancellationToken: cancellationToken));

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
}