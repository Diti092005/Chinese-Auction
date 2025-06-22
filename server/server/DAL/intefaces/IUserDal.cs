using server.Models;

namespace server.DAL.intefaces
{
    public interface IUserDal
    {
        Task<User> GetUserByUsername(string username);
        Task AddUser(User user);

        Task<User> GetUserFromToken();
        Task<bool> UsernameExist(string username);
    }
}
