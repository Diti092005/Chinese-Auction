using server.Models;

namespace server.Dal.Interfaces
{
    public interface IUserDal
    {
        Task<User> GetUserByUsername(string username);
        Task AddUser(User user);

        Task<User> GetUserFromToken();
        Task<bool> UsernameExist(string username);
    }
}
