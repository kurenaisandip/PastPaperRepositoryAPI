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

    public Task<bool> Login(UsersLogin userLogin, CancellationToken token = default)
    {
       
    }

    public Task<bool> Register(UsersLogin userLogin, CancellationToken token = default)
    {
       
    }
}