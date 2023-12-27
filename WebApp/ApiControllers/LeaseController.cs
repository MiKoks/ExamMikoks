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
    public class LeaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lease
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lease>>> GetLeases()
        {
          if (_context.Leases == null)
          {
              return NotFound();
          }
            return await _context.Leases.ToListAsync();
        }

        // GET: api/Lease/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lease>> GetLease(Guid id)
        {
          if (_context.Leases == null)
          {
              return NotFound();
          }
            var lease = await _context.Leases.FindAsync(id);

            if (lease == null)
            {
                return NotFound();
            }

            return lease;
        }

        // PUT: api/Lease/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLease(Guid id, Lease lease)
        {
            if (id != lease.Id)
            {
                return BadRequest();
            }

            _context.Entry(lease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaseExists(id))
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

        // POST: api/Lease
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lease>> PostLease(Lease lease)
        {
          if (_context.Leases == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Leases'  is null.");
          }
            _context.Leases.Add(lease);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLease", new { id = lease.Id }, lease);
        }

        // DELETE: api/Lease/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLease(Guid id)
        {
            if (_context.Leases == null)
            {
                return NotFound();
            }
            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }

            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaseExists(Guid id)
        {
            return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
