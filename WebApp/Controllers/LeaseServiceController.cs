using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF.App;
using Domain;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LeaseServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaseServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaseService
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaseServices.Include(l => l.Lease).Include(l => l.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaseService/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.LeaseServices == null)
            {
                return NotFound();
            }

            var leaseService = await _context.LeaseServices
                .Include(l => l.Lease)
                .Include(l => l.Service)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (leaseService == null)
            {
                return NotFound();
            }

            return View(leaseService);
        }

        // GET: LeaseService/Create
        public IActionResult Create()
        {
            
            var vm = new LeaseServiceViewModel();
            vm.LeaseServiceVmodel = new LeaseService();
            vm.LeaseSelectList = new SelectList(_context.Leases,
                nameof(vm.LeaseServiceVmodel.Lease.Id),
                nameof(vm.LeaseServiceVmodel.Lease.Id));
            vm.ServiceSelectList = new SelectList(_context.Services,
                nameof(vm.LeaseServiceVmodel.Service.Id),
                nameof(vm.LeaseServiceVmodel.Service.Id));
            return View(vm);
            
            /*ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");*/
            return View();
        }

        // POST: LeaseService/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( LeaseServiceViewModel leaseService)
        {
            if (ModelState.IsValid)
            {
                leaseService.LeaseServiceVmodel.LeaseId = Guid.NewGuid();
                _context.Add(leaseService.LeaseServiceVmodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            /*ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", leaseService.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", leaseService.ServiceId);*/
            leaseService.LeaseSelectList = new SelectList(_context.Leases,
                nameof(leaseService.LeaseServiceVmodel.Lease.Id),
                nameof(leaseService.LeaseServiceVmodel.Lease.Id));
            leaseService.ServiceSelectList = new SelectList(_context.Services,
                nameof(leaseService.LeaseServiceVmodel.Service.Id),
                nameof(leaseService.LeaseServiceVmodel.Service.Id));
            return View(leaseService);
        }

        // GET: LeaseService/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.LeaseServices == null)
            {
                return NotFound();
            }

            var leaseService = await _context.LeaseServices.FindAsync(id);
            if (leaseService == null)
            {
                return NotFound();
            }
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", leaseService.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", leaseService.ServiceId);
            return View(leaseService);
        }

        // POST: LeaseService/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LeaseId,ServiceId")] LeaseService leaseService)
        {
            if (id != leaseService.LeaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaseService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseServiceExists(leaseService.LeaseId))
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
            ViewData["LeaseId"] = new SelectList(_context.Leases, "Id", "ServicesIncluded", leaseService.LeaseId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", leaseService.ServiceId);
            return View(leaseService);
        }

        // GET: LeaseService/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.LeaseServices == null)
            {
                return NotFound();
            }

            var leaseService = await _context.LeaseServices
                .Include(l => l.Lease)
                .Include(l => l.Service)
                .FirstOrDefaultAsync(m => m.LeaseId == id);
            if (leaseService == null)
            {
                return NotFound();
            }

            return View(leaseService);
        }

        // POST: LeaseService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.LeaseServices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LeaseServices'  is null.");
            }
            var leaseService = await _context.LeaseServices.FindAsync(id);
            if (leaseService != null)
            {
                _context.LeaseServices.Remove(leaseService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseServiceExists(Guid id)
        {
          return (_context.LeaseServices?.Any(e => e.LeaseId == id)).GetValueOrDefault();
        }
    }
}
