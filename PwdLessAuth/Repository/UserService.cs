using Dapper;
using PwdLessAuth.Models;

namespace PwdLessAuth.Repository;

public class UserService : IUserService
{
    private readonly DapperContext _context;

    public UserService(DapperContext dapperContext)
    {
        _context = dapperContext;
    }

    public async Task CreateUser(User model)
    {
        string query = "Insert into Users (UserName, PublicKey, Email, SecurityQuestion, SecQuestionAnswer)" +
            " values (@UName, @PKey, @mail, @sQuestion, @sQanswer)";
        using (var connection = _context.CreateConnection())
        {
            var user = await connection.QueryAsync<User>(query, new
            {
                UName = model.Username,
                PKey = model.PublicKey,
                mail = model.Email,
                sQuestion = model.SecurityQuestion,
                sQanswer = model.SecQuestionAnswer
            });
        }
    }

    public async Task<User> GetUserByUserName(string userName)
    {
        var query = "SELECT * FROM Users where Username = @name";
        using (var connection = _context.CreateConnection())
        {
            var user = await connection.QueryAsync<User>(query, new { name = userName });
            return user.FirstOrDefault();
        }
    }

    public async Task UpdateUserLastAnswer(User model)
    {
        var query = "Update Users Set LastAnswer = @answer where Username = @name";
        using (var connection = _context.CreateConnection())
        {
            var user = await connection.QueryAsync<User>(query, new { answer = model.LastAnswer, name = model.Username });
        }
    }
}