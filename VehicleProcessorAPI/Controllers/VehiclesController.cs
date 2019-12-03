using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleProcessorAPI.Models;

namespace VehicleProcessorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleContext _context;

        public VehiclesController(VehicleContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicles/new
        // Returns vehicles whose model year is the current or following year (and mileage < 500)
        [HttpGet("new")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetNewVehicles()
        {
            return await _context.Vehicles.Where(v => (v.modelYear >= DateTime.Now.Year) && (v.mileage < 500.0)).ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("{vin}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(string vin)
        {
            var vehicle = await _context.Vehicles.FindAsync(vin);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{vin}")]
        public async Task<IActionResult> PutVehicle(string vin, Vehicle vehicle)
        {
            if (vin != vehicle.VIN)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(vin))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Vehicles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VehicleExists(vehicle.VIN))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVehicle", new { vin = vehicle.VIN }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{vin}")]
        public async Task<ActionResult<Vehicle>> DeleteVehicle(string vin)
        {
            var vehicle = await _context.Vehicles.FindAsync(vin);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        private bool VehicleExists(string vin)
        {
            return _context.Vehicles.Any(e => e.VIN == vin);
        }
    }
}
