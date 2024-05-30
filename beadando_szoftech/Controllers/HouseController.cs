using beadando_szoftech.DTOModels;
using beadando_szoftech.Models;
using beadando_szoftech.Services;
using Microsoft.AspNetCore.Mvc;

namespace beadando_szoftech.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly HouseService _houseService;

        public HouseController(HouseService houseService)
        {
            _houseService = houseService;
        }

        //összes ház listázása
        [HttpGet("GetHouses")]
        public async Task<List<House>> Get()
        {
            var housesDB = await _houseService.GetAsync();

            return housesDB;
        }


        //Ház lekérése id alapján
        [HttpGet("GetHouseByID{id}")]
        public async Task<ActionResult<House>> Get(int id)
        {
            var houseDB = await _houseService.GetAsync(id);

            if (houseDB is null)
            {
                return NotFound();
            }

            return houseDB; 
        }
        
        //Ház létrehozása
        [HttpPost("HouseAdd")]
        public async Task<ActionResult<User>> CreateHouse(House NewHouse)
        {
            House houseDB = new House();
            houseDB.date_added = NewHouse.date_added;
            houseDB.date_sold = NewHouse.date_sold;
            houseDB.boughtBy = NewHouse.boughtBy;
            houseDB.price = NewHouse.price;
            houseDB.size = NewHouse.size;
            houseDB.county = NewHouse.county;
            houseDB.city = NewHouse.city;
            houseDB.address = NewHouse.address;
            houseDB.addedBy = NewHouse.addedBy;

            await _houseService.CreateAsync(houseDB);

            return Ok(houseDB);
        }

        //Megváltoztatja a ház adatait 
        [HttpPatch("Update{id}")]
        public async Task<ActionResult<string>> UpdateHouse(int id, House updatedHouse)
        {
            var houseDB = await _houseService.GetAsync(id);

            if (houseDB is null)
            {
                return NotFound();
            }

            houseDB.id = updatedHouse.id;
            houseDB.date_added = updatedHouse.date_added;
            houseDB.date_sold = updatedHouse.date_sold;
            houseDB.boughtBy = updatedHouse.boughtBy;
            houseDB.price = updatedHouse.price;
            houseDB.size = updatedHouse.size;
            houseDB.county = updatedHouse.county;
            houseDB.city = updatedHouse.city;
            houseDB.address = updatedHouse.address;
            houseDB.addedBy = updatedHouse.addedBy;

            await _houseService.UpdateAsync(id, houseDB);

            return Ok("Sikeres módosítás!"); //NoContent()
        }

        [HttpDelete("Delete{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var user = await _houseService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await _houseService.RemoveAsync(id);

            return Ok("Sikeres Törlés!"); //NoContent()
        }
    }
}
