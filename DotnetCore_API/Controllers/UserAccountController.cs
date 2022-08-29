using DotnetCore_Database;
using DotnetCore_Database.DbEntities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotnetCore_BusinessModels;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DotnetCore_IRepositories;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace DotnetCore_API.Controllers
{
    public class UserAccountController : BaseApiController
    {
        private readonly DbConnection _context;
        private readonly ITokenService _tokenSerive;

        public UserAccountController(DbConnection context, ITokenService tokenSerive)
        {
            _context = context;
            _tokenSerive = tokenSerive;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
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
            return new UserDTO
            {
                username = user.UserName,
                token = _tokenSerive.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == model.Username);
            if (user == null) return Unauthorized("Invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDTO
            {
                username = user.UserName,
                token = _tokenSerive.CreateToken(user)
            };
        }

        [HttpGet("UserList")]
        public async Task<ActionResult<IEnumerable>> UserList()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> UserList(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
