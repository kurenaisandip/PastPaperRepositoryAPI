using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public interface IUserLoginRepository
{
    Task<bool> Login(UsersLogin userLogin, CancellationToken token = default);
    
    Task<bool> Register(UsersLogin userLogin, CancellationToken token = default);
}