using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagementSystem.Data;
using TransportManagementSystem.Model;

namespace TransportManagementSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVehicle(int id , Vehicle vehicle)
        {
            var vr = await DbContext.Vehicles.FindAsync(id);
            if (vr == null)
            {
                return NotFound("vehicle is not found");
            }

         
                vr.VehicleNumber = vehicle.VehicleNumber;
                vr.vehicleType = vehicle.vehicleType;
                vr.IsActive = vehicle.IsActive;
                await DbContext.SaveChangesAsync();
                return Ok("Vehicle is updated");
           
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteVehicle(int id)
        {
            var vr = await DbContext.Vehicles.FindAsync(id);
            if(vr == null)
            {
                return NotFound("this is not exit");
            }
             DbContext.Vehicles.Remove(vr);
            await DbContext.SaveChangesAsync();
            return Ok(vr.VehicleNumber+"Deleted");
        }
        [HttpGet("{vehicleName}")]
        public async Task<IActionResult> SearchbyName( string vehicleName)
        {
            var vr = await DbContext.Vehicles.FirstOrDefaultAsync(x => x.VehicleNumber.ToLower() ==vehicleName.ToLower() );
            if(vr == null)
            {
                return NotFound( vehicleName+" "+"this vehicle is not exit");
            }
            return Ok(vr);
        }
    }

    

   
}
