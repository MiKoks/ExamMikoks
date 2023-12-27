using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LeaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LeaseController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Lease
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Leases.Include(l => l.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Lease/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // GET: Lease/Create
        public IActionResult Create()
        {
            //var apartments = _context.Apartments.Select(a => new { a.Id }).ToList();
            
            var vm = new LeaseVM();
            vm.LeaseVmodel = new Lease();
            vm.UserSelectList = new SelectList(_userManager.Users,
                nameof(vm.LeaseVmodel.AppUser.Id),
                nameof(vm.LeaseVmodel.AppUser.FirstName));
            vm.AppartmentList = new SelectList(_context.Apartments,
                nameof(vm.LeaseVmodel.Apartment.Id),
                nameof(vm.LeaseVmodel.Apartment.Id));
            return View(vm);
        }

        // POST: Lease/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( LeaseVM lease)
        {
            if (ModelState.IsValid)
            {
                lease.LeaseVmodel.Id = Guid.NewGuid();
                _context.Add(lease.LeaseVmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            lease.UserSelectList = new SelectList(_userManager.Users,
                nameof(lease.LeaseVmodel.AppUser.Id),
                nameof(lease.LeaseVmodel.AppUser.FirstName));
            lease.AppartmentList = new SelectList(_context.Apartments,
                nameof(lease.LeaseVmodel.Apartment.Id),
                nameof(lease.LeaseVmodel.Apartment.Id));
            return View(lease);
        }

        // GET: Lease/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Address");
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", lease.AppUserId);
            return View(lease);
        }

        // POST: Lease/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartmentId,AppUserId,StartDate,EndDate,MonthlyRent,ServicesIncluded,Id")] Lease lease)
        {
            if (id != lease.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseExists(lease.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Address");
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", lease.AppUserId);
            return View(lease);
        }

        // GET: Lease/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var lease = await _context.Leases
                .Include(l => l.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lease == null)
            {
                return NotFound();
            }

            return View(lease);
        }

        // POST: Lease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Leases == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Leases'  is null.");
            }
            var lease = await _context.Leases.FindAsync(id);
            if (lease != null)
            {
                _context.Leases.Remove(lease);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseExists(Guid id)
        {
          return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
