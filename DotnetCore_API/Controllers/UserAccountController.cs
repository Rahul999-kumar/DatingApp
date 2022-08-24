using DotnetCore_Database;
using DotnetCore_Database.DbEntities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotnetCore_BusinessModels;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DotnetCore_API.Controllers
{
    public class UserAccountController : BaseApiController
    {
        private readonly DbConnection _context;

        public UserAccountController(DbConnection context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Users>> Register(UserDTO model)
        {
            if (await UserExists(model.UserName))
            {
                return BadRequest("username is already taken");
            }
            using var hmac = new HMACSHA512();
            var user = new Users
            {
                UserName = model.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
