using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using beadando_szoftech.DTOModels;
using beadando_szoftech.Models;
using beadando_szoftech.Services;
using System.Security.Cryptography;
using Amazon.Runtime.Internal;
using System.Security.Claims;

namespace beadando_szoftech.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var userByUsername = await _userService.GetBasedOnUsernameAsync(request.Username);
            var userByEmail = await _userService.GetBasedOnEmailAsync(request.Email);

            if (userByUsername != null)
            {
                return BadRequest("This username is already in use.");
            }

            if (userByEmail != null)
            {
                return BadRequest("This email address is already registered.");
            }

            User userDB = new User();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userDB.username = request.Username;
            userDB.passwordHash = passwordHash;
            userDB.passwordSalt = passwordSalt;
            userDB.email = request.Email;
            userDB.role = request.Role;
            userDB.joinDate = request.JoinDate;
            userDB.lastOnlineDate = request.LastOnlineDate;

            await _userService.CreateAsync(userDB);

            return Ok(userDB);
        }

        [HttpPost("register/{username}/{password}/{email}/{role}")]
        public async Task<ActionResult<User>> Register(string username,string password, string email, string role)
        {
            var userByUsername = await _userService.GetBasedOnUsernameAsync(username);
            var userByEmail = await _userService.GetBasedOnEmailAsync(email);

            if (userByUsername != null)
            {
                return BadRequest("This username is already in use.");
            }

            if (userByEmail != null)
            {
                return BadRequest("This email address is already registered.");
            }

            User userDB = new User();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            userDB.username = username;
            userDB.passwordHash = passwordHash;
            userDB.passwordSalt = passwordSalt;
            userDB.email = email;
            userDB.role = role;
            userDB.joinDate = DateTime.Now.ToString();
            userDB.lastOnlineDate = DateTime.Now.ToString();

            await _userService.CreateAsync(userDB);

            return Ok(userDB);
        }

        [HttpPost("login/{username}/{password}")]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            User userDB = await _userService.GetBasedOnUsernameAsync(username);

            if (userDB == null)
            {
                return BadRequest("Error.");
            }

            if (!VerifyPasswordHash(password, userDB.passwordHash, userDB.passwordSalt))
            {
                return BadRequest("Error.");
            }

            return Ok("Sikeres bejelentkezés");
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
