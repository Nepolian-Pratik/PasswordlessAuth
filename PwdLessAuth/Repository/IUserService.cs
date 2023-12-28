using PwdLessAuth.Models;

namespace PwdLessAuth.Repository;

public interface IUserService
{
    public Task<User> GetUserByUserName(string userName);

    //Insert new user
    public Task CreateUser(User model);

    public Task UpdateUserLastAnswer(User model);
}