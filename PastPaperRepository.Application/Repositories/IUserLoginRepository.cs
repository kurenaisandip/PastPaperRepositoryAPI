using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Repositories;

public interface IUserLoginRepository
{
    Task<bool> Login(UsersLogin userLogin);
    
    Task<bool> Register(UsersLogin userLogin);
}