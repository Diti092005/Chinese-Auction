using server.Models;

namespace server.BLL.Intefaces
{
    public interface IUserService
    {
        Task<string> Login(string username, string password);
        Task Register(User user);
        Task<bool> UsernameExist(string username);
    }
}
