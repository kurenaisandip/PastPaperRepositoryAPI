using System.Transactions;
using Dapper;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class UserLoginRepository: IUserLoginRepository
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
            var transaction =  connection.BeginTransaction();
            try
            {
                transaction.Commit();
                var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND Password = @Password";
                var result = await connection.ExecuteScalarAsync<int>(new CommandDefinition(query, new { userLogin.Email, userLogin.Password }, transaction, cancellationToken: token));

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

    public Task<bool> Register(UsersLogin userLogin, CancellationToken token = default)
    {
       
    }
}