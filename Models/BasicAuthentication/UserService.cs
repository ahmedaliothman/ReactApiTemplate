using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models.DB;

namespace Api.Models.BasicAuthentication

{
    public interface IUserService
    {
        Task<SystemUser> Authenticate(string username, string password);
        Task<IEnumerable<SystemUser>> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly ApiDBContext _context;

         public UserService( ApiDBContext context)
        {
            _context = context;
        }
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "paciuser", Password = "Lm$_F@rW!" }
        };

        public async Task<SystemUser> Authenticate(string username, string password)
        {
           // var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));
            var user = await Task.Run(() =>_context.SystemUsers.SingleOrDefault(x => x.Email == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<SystemUser>> GetAll()
        {
            return await Task.Run(() => _context.SystemUsers.WithoutPasswords());
        }
    }
}