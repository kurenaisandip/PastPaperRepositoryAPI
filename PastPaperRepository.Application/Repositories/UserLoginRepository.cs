using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public class UserLoginRepository: IUserLoginRepository
{
    private readonly List<UsersLogin> _users = new();
    
    public Task<bool> Login(UsersLogin userLogin)
    {
        if (_users.Any(x => x.Email == userLogin.Email && x.Password == userLogin.Password))
        {
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> Register(UsersLogin userLogin)
    {
       _users.Add(userLogin);
         return Task.FromResult(true);
    }
}