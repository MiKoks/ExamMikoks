using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class ServiceConsumptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceConsumptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceConsumption
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServiceConsumptions.Include(s => s.Lease).Include(s => s.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServiceConsumption/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ServiceConsumptions == null)
            {
                return NotFound();
            }

            var serviceConsumption = await _context.ServiceConsumptions
                .Include(s => s.Lease)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceConsumption == null)
            {
                return NotFound();
            }

            return View(serviceConsumption);
        }

        // GET: ServiceConsumption/Create
        public IActionResult Create()
        {
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: ServiceConsumption/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaseId,ServiceId,QuantityUsed,Rate,TotalCost,Id")] ServiceConsumption serviceConsumption)
        {
            if (ModelState.IsValid)
            {
                serviceConsumption.Id = Guid.NewGuid();
                _context.Add(serviceConsumption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", serviceConsumption.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceConsumption.ServiceId);
            return View(serviceConsumption);
        }

        // GET: ServiceConsumption/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ServiceConsumptions == null)
            {
                return NotFound();
            }

            var serviceConsumption = await _context.ServiceConsumptions.FindAsync(id);
            if (serviceConsumption == null)
            {
                return NotFound();
            }
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", serviceConsumption.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceConsumption.ServiceId);
            return View(serviceConsumption);
        }

        // POST: ServiceConsumption/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LeaseId,ServiceId,QuantityUsed,Rate,TotalCost,Id")] ServiceConsumption serviceConsumption)
        {
            if (id != serviceConsumption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceConsumption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceConsumptionExists(serviceConsumption.Id))
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
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", serviceConsumption.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceConsumption.ServiceId);
            return View(serviceConsumption);
        }

        // GET: ServiceConsumption/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ServiceConsumptions == null)
            {
                return NotFound();
            }

            var serviceConsumption = await _context.ServiceConsumptions
                .Include(s => s.Lease)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceConsumption == null)
            {
                return NotFound();
            }

            return View(serviceConsumption);
        }

        // POST: ServiceConsumption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ServiceConsumptions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ServiceConsumptions'  is null.");
            }
            var serviceConsumption = await _context.ServiceConsumptions.FindAsync(id);
            if (serviceConsumption != null)
            {
                _context.ServiceConsumptions.Remove(serviceConsumption);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceConsumptionExists(Guid id)
        {
          return (_context.ServiceConsumptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
