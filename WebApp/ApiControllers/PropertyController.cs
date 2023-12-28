using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;
using WebApp.dto;

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
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetProperties()
        {
          if (_context.Properties == null)
          {
              return NotFound();
          }

          var res = await _context.Properties
              .Include(x => x.Apartments!)
              .ThenInclude(x => x.CurrentLease)
              .ThenInclude(x => x!.AppUser)
              .ToListAsync();
          
          return Ok(res.Select(x => new PropertyDTO
          {
              Id = x.Id,
              Address = x.Address,
              PictureUrl = x.PictureUrl,
              Apartments = x.Apartments!.Select(a => new ApartmentDTO
              {
                  Id = a.Id,
                  PropertyId = null,
                  Property = null,
                  FloorNumber = a.FloorNumber,
                  RoomCount = a.RoomCount,
                  MonthlyRent = a.MonthlyRent,
                  Status = a.Status,
                  CurrentLeaseId = a.CurrentLeaseId,
                  CurrentLease = a.CurrentLease == null ? null : new LeaseDTO
                  {
                      Id = a.CurrentLease!.Id,
                      ApartmentId = a.CurrentLease.ApartmentId,
                      AppUserId = a.CurrentLease.AppUserId,
                      AppUser = a.CurrentLease.AppUser,
                      StartDate = a.CurrentLease.StartDate,
                      EndDate = a.CurrentLease.EndDate,
                      MonthlyRent = a.CurrentLease.MonthlyRent,
                      ServicesIncluded = a.CurrentLease.ServicesIncluded,
                      LeaseServices = a.CurrentLease.LeaseServices!
                  },
              }).ToList()
          }));
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
