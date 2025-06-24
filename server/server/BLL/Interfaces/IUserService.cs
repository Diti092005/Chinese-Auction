using server.Models;

namespace server.Bll.Interfaces
{
    public interface IUserService
    {
        Task<string> Login(string username, string password);
        Task Register(User user);
        Task<bool> UsernameExist(string username);
    }
}
