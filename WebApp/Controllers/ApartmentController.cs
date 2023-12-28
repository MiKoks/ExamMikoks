using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;
using WebApp.dto;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ApartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Apartment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Apartments.Include(a => a.CurrentLease).Include(a => a.Property);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Apartment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.CurrentLease)
                .Include(a => a.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartment/Create
        public IActionResult Create()
        {
            /*ViewData["CurrentLeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded");
            ViewData["PropertyId"] = new SelectList(_context.Properties, "Id", "Address");*/
            
            var vm = new ApartmentViewModel();
            vm.ApartmentVmodel = new Apartment();
            vm.LeaseSelectList = new SelectList(_context.Leases,
                nameof(vm.ApartmentVmodel.CurrentLease.Id),
                nameof(vm.ApartmentVmodel.CurrentLease.Id));
            vm.PropertySelectList = new SelectList(_context.Properties,
                nameof(vm.ApartmentVmodel.Property.Id),
                nameof(vm.ApartmentVmodel.Property.Address));
            return View(vm);
        }

        // POST: Apartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ApartmentViewModel apartment)
        {
            if (ModelState.IsValid)
            {
                apartment.ApartmentVmodel.Id = Guid.NewGuid();
                _context.Add(apartment.ApartmentVmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            apartment.LeaseSelectList = new SelectList(_context.Leases,
                nameof(apartment.ApartmentVmodel.CurrentLease.Id),
                nameof(apartment.ApartmentVmodel.CurrentLease.Id));
            apartment.PropertySelectList = new SelectList(_context.Properties,
                nameof(apartment.ApartmentVmodel.Property.Id),
                nameof(apartment.ApartmentVmodel.Property.Address));
            
            /*ViewData["CurrentLeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", apartment.CurrentLeaseId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "Id", "Address", apartment.PropertyId);*/
            return View(apartment);
        }

        // GET: Apartment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            /*ViewData["CurrentLeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", apartment.CurrentLeaseId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "Id", "Address", apartment.PropertyId);*/
            var vm = new ApartmentViewModel();
            vm.ApartmentVmodel = new Apartment();
            vm.LeaseSelectList = new SelectList(_context.Leases,
                nameof(vm.ApartmentVmodel.CurrentLease.Id),
                nameof(vm.ApartmentVmodel.CurrentLease.Id));
            vm.PropertySelectList = new SelectList(_context.Properties,
                nameof(vm.ApartmentVmodel.Property.Id),
                nameof(vm.ApartmentVmodel.Property.Address));
            return View(vm);
        }

        // POST: Apartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,  ApartmentViewModel apartment)
        {
            if (id != apartment.ApartmentVmodel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(apartment.ApartmentVmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            var vm = new ApartmentViewModel();
            vm.ApartmentVmodel = new Apartment();
            vm.LeaseSelectList = new SelectList(_context.Leases,
                nameof(vm.ApartmentVmodel.CurrentLease.Id),
                nameof(vm.ApartmentVmodel.CurrentLease.Id));
            vm.PropertySelectList = new SelectList(_context.Properties,
                nameof(vm.ApartmentVmodel.Property.Id),
                nameof(vm.ApartmentVmodel.Property.Address));
            return View(vm);
            
            /*ViewData["CurrentLeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", apartment.CurrentLeaseId);
            ViewData["PropertyId"] = new SelectList(_context.Properties, "Id", "Address", apartment.PropertyId);*/
            //return View(apartment);
        }

        // GET: Apartment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.CurrentLease)
                .Include(a => a.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Apartments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Apartments'  is null.");
            }
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(Guid id)
        {
          return (_context.Apartments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
