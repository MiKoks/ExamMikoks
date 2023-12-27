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
    public class PropertyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Property
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
          if (_context.Properties == null)
          {
              return NotFound();
          }
            return await _context.Properties.ToListAsync();
        }

        // GET: api/Property/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(Guid id)
        {
          if (_context.Properties == null)
          {
              return NotFound();
          }
            var @property = await _context.Properties.FindAsync(id);

            if (@property == null)
            {
                return NotFound();
            }

            return @property;
        }

        // PUT: api/Property/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty(Guid id, Property @property)
        {
            if (id != @property.Id)
            {
                return BadRequest();
            }

            _context.Entry(@property).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
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

        // POST: api/Property
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Property>> PostProperty(Property @property)
        {
          if (_context.Properties == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Properties'  is null.");
          }
            _context.Properties.Add(@property);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProperty", new { id = @property.Id }, @property);
        }

        // DELETE: api/Property/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            if (_context.Properties == null)
            {
                return NotFound();
            }
            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }

            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PropertyExists(Guid id)
        {
            return (_context.Properties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
