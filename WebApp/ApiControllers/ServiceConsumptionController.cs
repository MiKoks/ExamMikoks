using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceConsumptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceConsumptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceConsumption
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceConsumption>>> GetServiceConsumptions()
        {
          if (_context.ServiceConsumptions == null)
          {
              return NotFound();
          }
            return await _context.ServiceConsumptions.ToListAsync();
        }

        // GET: api/ServiceConsumption/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceConsumption>> GetServiceConsumption(Guid id)
        {
          if (_context.ServiceConsumptions == null)
          {
              return NotFound();
          }
            var serviceConsumption = await _context.ServiceConsumptions.FindAsync(id);

            if (serviceConsumption == null)
            {
                return NotFound();
            }

            return serviceConsumption;
        }

        // PUT: api/ServiceConsumption/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceConsumption(Guid id, ServiceConsumption serviceConsumption)
        {
            if (id != serviceConsumption.Id)
            {
                return BadRequest();
            }

            _context.Entry(serviceConsumption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceConsumptionExists(id))
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

        // POST: api/ServiceConsumption
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceConsumption>> PostServiceConsumption(ServiceConsumption serviceConsumption)
        {
          if (_context.ServiceConsumptions == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ServiceConsumptions'  is null.");
          }
            _context.ServiceConsumptions.Add(serviceConsumption);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceConsumption", new { id = serviceConsumption.Id }, serviceConsumption);
        }

        // DELETE: api/ServiceConsumption/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceConsumption(Guid id)
        {
            if (_context.ServiceConsumptions == null)
            {
                return NotFound();
            }
            var serviceConsumption = await _context.ServiceConsumptions.FindAsync(id);
            if (serviceConsumption == null)
            {
                return NotFound();
            }

            _context.ServiceConsumptions.Remove(serviceConsumption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceConsumptionExists(Guid id)
        {
            return (_context.ServiceConsumptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
