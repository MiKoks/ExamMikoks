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
    public class LeaseServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaseServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaseService
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaseService>>> GetLeaseServices()
        {
          if (_context.LeaseServices == null)
          {
              return NotFound();
          }
            return await _context.LeaseServices.ToListAsync();
        }

        // GET: api/LeaseService/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaseService>> GetLeaseService(Guid id)
        {
          if (_context.LeaseServices == null)
          {
              return NotFound();
          }
            var leaseService = await _context.LeaseServices.FindAsync(id);

            if (leaseService == null)
            {
                return NotFound();
            }

            return leaseService;
        }

        // PUT: api/LeaseService/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaseService(Guid id, LeaseService leaseService)
        {
            if (id != leaseService.LeaseId)
            {
                return BadRequest();
            }

            _context.Entry(leaseService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseServiceExists(id))
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

        // POST: api/LeaseService
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LeaseService>> PostLeaseService(LeaseService leaseService)
        {
          if (_context.LeaseServices == null)
          {
              return Problem("Entity set 'ApplicationDbContext.LeaseServices'  is null.");
          }
            _context.LeaseServices.Add(leaseService);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LeaseServiceExists(leaseService.LeaseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLeaseService", new { id = leaseService.LeaseId }, leaseService);
        }

        // DELETE: api/LeaseService/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaseService(Guid id)
        {
            if (_context.LeaseServices == null)
            {
                return NotFound();
            }
            var leaseService = await _context.LeaseServices.FindAsync(id);
            if (leaseService == null)
            {
                return NotFound();
            }

            _context.LeaseServices.Remove(leaseService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaseServiceExists(Guid id)
        {
            return (_context.LeaseServices?.Any(e => e.LeaseId == id)).GetValueOrDefault();
        }
    }
}
