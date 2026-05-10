using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagementSystem.Data;
using TransportManagementSystem.Model;

namespace TransportManagementSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApDb DbContext;

        public VehicleController(ApDb apDb) {
            DbContext = apDb;
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            await DbContext.AddAsync(vehicle);
                    await DbContext.SaveChangesAsync();

            return Ok("Vehicle add successfuly");


        }
        [HttpGet]
        public async Task<IActionResult> ShowVehicle()
        {
            var vr = await DbContext.Vehicles.ToListAsync();
            return Ok(vr);
        }
    }
}
