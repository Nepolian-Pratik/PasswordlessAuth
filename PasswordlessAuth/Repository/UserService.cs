using Dapper;
using PasswordlessAuth.Models;

namespace PasswordlessAuth.Repository
{
    public class UserService : IUserRepository
    {
        private readonly DapperContext _context;

        public UserService(DapperContext dapperContext)
        {
            _context = dapperContext;
        }

        public Task CreateUser(User model)
        {
            throw new NotImplementedException();
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

        public Task UpdateUser(User model)
        {
            throw new NotImplementedException();
        }
    }
}