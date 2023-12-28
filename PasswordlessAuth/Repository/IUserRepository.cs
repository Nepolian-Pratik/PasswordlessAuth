using PasswordlessAuth.Models;

namespace PasswordlessAuth.Repository;

public interface IUserRepository
{
    public Task<User> GetUserByUserName(string userName);

    //Insert new user
    public Task CreateUser(User model);

    public Task UpdateUser(User model);
}