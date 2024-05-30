using beadando_szoftech.Models;
using beadando_szoftech.Services;
using beadando_szoftech.DTOModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;



namespace beadando_szoftech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //összes felhasználó listázása
        [HttpGet("GetUsers")]
        public async Task<List<UserDTO>> Get()
        {
            var usersDB = await _userService.GetAsync();
            List<UserDTO> usersDTO = new List<UserDTO>();
            foreach (var user in usersDB)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Id = user.id;
                userDTO.Username = user.username;
                userDTO.Email = user.email;
                userDTO.Role = user.role;
                userDTO.JoinDate = user.joinDate;
                userDTO.LastOnlineDate = user.lastOnlineDate;

                usersDTO.Add(userDTO);
            }
            return usersDTO;
        }

        //felhasználó lekérése id alapján
        [HttpGet("GetUserByID{id:length(24)}")]
        public async Task<ActionResult<UserDTO>> Get(string id)
        {
            var userDB = await _userService.GetAsync(id);

            if (userDB is null)
            {
                return NotFound();
            }

            UserDTO userDTO = new UserDTO();
            userDTO.Id = userDB.id;
            userDTO.Username = userDB.username;
            userDTO.Email = userDB.email;
            userDTO.Role = userDB.role;
            userDTO.JoinDate = userDB.joinDate;
            userDTO.LastOnlineDate = userDB.lastOnlineDate;

            return userDTO;
        }

        //felhasználó lekérése email alapján
        [HttpGet("GetUser_BasedOnEmail{email}")]
        public async Task<ActionResult<UserDTO>> GetBasedOnEmail(string email)
        {
            var userDB = await _userService.GetBasedOnEmailAsync(email);

            if (userDB is null)
            {
                return null;
            }

            UserDTO userDTO = new UserDTO();
            userDTO.Id = userDB.id;
            userDTO.Username = userDB.username;
            userDTO.Email = userDB.email;
            userDTO.Role = userDB.role;
            userDTO.JoinDate = userDB.joinDate;
            userDTO.LastOnlineDate = userDB.lastOnlineDate;

            return userDTO;
        }

        //email ellenőrzés
        [HttpGet("is-registered/{email}")]
        public async Task<ActionResult<bool>> IsEmailRegistered(string email)
        {
            var userDB = await _userService.GetBasedOnEmailAsync(email);

            if (userDB is null)
            {
                return false;
            }

            return true;
        }

        //userDTO létrehozása
        [HttpPost]
        public async Task<IActionResult> Post(UserDTO newUserDTO)
        {
            User userDB = new User();
            userDB.username = newUserDTO.Username;
            userDB.email = newUserDTO.Email;
            userDB.role = newUserDTO.Role;
            userDB.joinDate = newUserDTO.JoinDate;
            userDB.lastOnlineDate = newUserDTO.LastOnlineDate;

            await _userService.CreateAsync(userDB);

            return CreatedAtAction(nameof(Get), new { id = userDB.id }, userDB);
        }

        //Megváltoztatja a felhasználó adatait 
        [HttpPatch("Update{id:length(24)}")]
        public async Task<ActionResult<string>> Update(string id, UserDTO updatedUserDTO)
        {
            var userDB = await _userService.GetAsync(id);

            if (userDB is null)
            {
                return NotFound();
            }

            var userByUsername = await _userService.GetBasedOnUsernameAsync(updatedUserDTO.Username);
            var userByEmail = await _userService.GetBasedOnEmailAsync(updatedUserDTO.Email);

            if (userByUsername != null && userByUsername.id != updatedUserDTO.Id)
            {
                return BadRequest("This username is already in use.");
            }

            if (userByEmail != null && userByEmail.id != updatedUserDTO.Id)
            {
                return BadRequest("This email address is already registered.");
            }

            userDB.username = updatedUserDTO.Username;
            userDB.email = updatedUserDTO.Email;
            userDB.role = updatedUserDTO.Role;
            userDB.joinDate = updatedUserDTO.JoinDate;
            userDB.lastOnlineDate = updatedUserDTO.LastOnlineDate;

            await _userService.UpdateAsync(id, userDB);

            return Ok("Sikeres módosítás!"); //NoContent()
        }

        //Kicseréli a felhasználó role ját
        [HttpGet("new-role/{id:length(24)}/{newRole}")]
        public async Task<ActionResult<string>> ChangeUserRole(string id, string newRole)
        {
            var userDB = await _userService.GetAsync(id);

            if (userDB is null)
            {
                return NotFound();
            }

            userDB.role = newRole;

            await _userService.UpdateAsync(id, userDB);

            return Ok("Sikeresen modosítottad!"); // NoContent()
        }

        [HttpDelete("Delete{id:length(24)}")]
        public async Task<ActionResult<string>> Delete(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await _userService.RemoveAsync(id);

            return Ok("Sikeres Törlés!"); //NoContent()
        }
    }
}
