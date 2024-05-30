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
            /*
            List<House> housesList = new List<House>();
            foreach (var houseItem in housesDB)
            {
                House house = new House();
                house.id = houseItem.id;
                house.date_added = houseItem.date_added;
                house.date_sold = houseItem.date_sold;
                house.boughtBy = houseItem.boughtBy;
                house.price = houseItem.price;
                house.size = houseItem.size;
                house.county = houseItem.county;
                house.city = houseItem.city;
                house.address = houseItem.address;
                house.addedBy = houseItem.addedBy;


                housesList.Add(house);
            }
            */
            return housesDB; //housesList
        }


        //Ház lekérése id alapján
        [HttpGet("GetHouseByID{id:length(24)}")]
        public async Task<ActionResult<House>> Get(string id)
        {
            var houseDB = await _houseService.GetAsync(id);

            if (houseDB is null)
            {
                return NotFound();
            }

            /*
            House houseItem = new House();
            houseItem.id = houseDB.id;
            houseItem.date_added = houseDB.date_added;
            houseItem.date_sold = houseDB.date_sold;
            houseItem.boughtBy = houseDB.boughtBy;
            houseItem.price = houseDB.price;
            houseItem.size = houseDB.size;
            houseItem.county = houseDB.county;
            houseItem.city = houseDB.city;
            houseItem.address = houseDB.address;
            houseItem.addedBy = houseDB.addedBy;

            */
            return houseDB; // houseItem
        }
        
        //Ház létrehozása
        [HttpPost]
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
        [HttpPatch("Update{id:length(24)}")]
        public async Task<ActionResult<string>> UpdateHouse(string id, House updatedHouse)
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

        [HttpDelete("Delete{id:length(24)}")]
        public async Task<ActionResult<string>> Delete(string id)
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
