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
    public class ApartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Apartment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApartmentDTO>>> GetApartments()
        {
          if (_context.Apartments == null)
          {
              return NotFound();
          }
          var res =  (await _context.Apartments
              .Include(prop => prop.Property)
              .Include(prop => prop.CurrentLease!)
              .ThenInclude(prop => prop.AppUser)
              //.Where(prop => prop.CurrentLease.AppUserId == User.GetUserId())
              .Distinct()
              .ToListAsync());
              return Ok( res.Select(x => new ApartmentDTO
          {
              Id = x.Id,
              PropertyId = x.PropertyId,
              Property = x.Property == null ? null: new PropertyDTO
              {
                  Id = x.Property.Id,
                  Address = x.Property.Address,
                  PictureUrl = x.Property.Address
              },
              FloorNumber = x.FloorNumber,
              RoomCount = x.RoomCount,
              MonthlyRent = x.MonthlyRent,
              Status = x.Status,
              CurrentLeaseId = x.CurrentLeaseId,
              CurrentLease = x.CurrentLease == null ? null: new LeaseDTO
              {
                  Id = x.CurrentLease!.Id,
                  ApartmentId = x.CurrentLease.ApartmentId,
                  AppUserId = x.CurrentLease.AppUserId,
                  AppUser = x.CurrentLease.AppUser,
                  StartDate = x.CurrentLease.StartDate,
                  EndDate = x.CurrentLease.EndDate,
                  MonthlyRent = x.CurrentLease.MonthlyRent,
                  ServicesIncluded = x.CurrentLease.ServicesIncluded,
                  LeaseServices = x.CurrentLease.LeaseServices
              }
          }));
        }

        // GET: api/Apartment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDTO>> GetApartment(Guid id)
        {
          if (_context.Apartments == null)
          {
              return NotFound();
          }
          
          var apartment = await _context.Apartments.FindAsync(id);

          if (apartment == null)
          {
              return NotFound();
          }

          var res = new ApartmentDTO
          {
              Id = apartment.Id,
              PropertyId = apartment.PropertyId,
              Property = new PropertyDTO
              {
                  Id = apartment.Property!.Id,
                  Address = apartment.Property.Address,
                  PictureUrl = apartment.Property.Address
              },
              FloorNumber = apartment.FloorNumber,
              RoomCount = apartment.RoomCount,
              MonthlyRent = apartment.MonthlyRent,
              Status = apartment.Status,
              CurrentLeaseId = apartment.CurrentLeaseId,
              CurrentLease = new LeaseDTO()
          };
          return res;
        }

        // PUT: api/Apartment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(Guid id, ApartmentDTO apartment)
        {
            if (id != apartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
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

        // POST: api/Apartment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apartment>> PostApartment(Apartment apartment)
        {
          if (_context.Apartments == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Apartments'  is null.");
          }
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment);
        }

        // DELETE: api/Apartment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            if (_context.Apartments == null)
            {
                return NotFound();
            }
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApartmentExists(Guid id)
        {
            return (_context.Apartments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
