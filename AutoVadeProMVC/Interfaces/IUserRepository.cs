using AutoVadeProMVC.Models;

namespace AutoVadeProMVC.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByLoginNoTracking(string login);
        Task<User> GetUserByLoginPassword(string login, string password);
        Task<User> GetUserByIdNoTracking(int id);
        bool Add(User user);
        bool Update(User user);
        bool Delete(User user);
        bool Save();
    }
}
