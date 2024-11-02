using System.Transactions;
using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class UserLoginRepository : IUserLoginRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserLoginRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> Login(UsersLogin userLogin, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            var transaction = connection.BeginTransaction();
            try
            {
                transaction.Commit();
                var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND Password = @Password";
                var result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(query,
                    new { userLogin.Email, userLogin.Password }, transaction, cancellationToken: token));

                if (result == 0)
                {
                    return false;
                }

                return result > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<bool> Register(UsersLogin userLogin, CancellationToken token = default)
    {
        using (var connection = await _connectionFactory.CreateConnectionAsync(token))
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Check if the email already exists
                    var checkQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    var userExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
                            checkQuery,
                            new { Email = userLogin.Email },
                            transaction,
                            cancellationToken: token
                        )
                    );

                    if (userExists > 0)
                    {
                        return false; // User already exists
                    }

                    // Insert the new user
                    var insertQuery =
                        "INSERT INTO Users (user_name, email, password) VALUES (@Name, @Email, @Password)";
                    var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(
                            insertQuery,
                            new
                            {
                                Name = userLogin.Name,
                                Email = userLogin.Email,
                                Password = userLogin.Password
                            },
                            transaction,
                            cancellationToken: token
                        )
                    );

                    // Commit the transaction if the insertion is successful
                    transaction.Commit();
                    return rowsAffected > 0;
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

