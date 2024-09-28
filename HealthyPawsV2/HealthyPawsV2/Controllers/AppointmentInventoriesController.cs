using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;

namespace HealthyPawsV2.Controllers
{
    public class AppointmentInventoriesController : Controller
    {
        private readonly HPContext _context;

        public AppointmentInventoriesController(HPContext context)
        {
            _context = context;
        }

        // GET: AppointmentInventories
        public async Task<IActionResult> Index()
        {
            var hPContext = _context.AppointmentInventories.Include(a => a.Appointment).Include(a => a.Inventory);
            return View(await hPContext.ToListAsync());
        }

        // GET: AppointmentInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories
                .Include(a => a.Appointment)
                .Include(a => a.Inventory)
                .FirstOrDefaultAsync(m => m.appointmentInventoryId == id);
            if (appointmentInventory == null)
            {
                return NotFound();
            }

            return View(appointmentInventory);
        }

        // GET: AppointmentInventories/Create
        public IActionResult Create()
        {
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Additional");
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand");
            return View();
        }

        // POST: AppointmentInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose")] AppointmentInventory appointmentInventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Additional", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);
            return View(appointmentInventory);
        }

        // GET: AppointmentInventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories.FindAsync(id);
            if (appointmentInventory == null)
            {
                return NotFound();
            }
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Additional", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);
            return View(appointmentInventory);
        }

        // POST: AppointmentInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose")] AppointmentInventory appointmentInventory)
        {
            if (id != appointmentInventory.appointmentInventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentInventoryExists(appointmentInventory.appointmentInventoryId))
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
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "Additional", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);
            return View(appointmentInventory);
        }

        // GET: AppointmentInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentInventory = await _context.AppointmentInventories
                .Include(a => a.Appointment)
                .Include(a => a.Inventory)
                .FirstOrDefaultAsync(m => m.appointmentInventoryId == id);
            if (appointmentInventory == null)
            {
                return NotFound();
            }

            return View(appointmentInventory);
        }

        // POST: AppointmentInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentInventory = await _context.AppointmentInventories.FindAsync(id);
            if (appointmentInventory != null)
            {
                _context.AppointmentInventories.Remove(appointmentInventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentInventoryExists(int id)
        {
            return _context.AppointmentInventories.Any(e => e.appointmentInventoryId == id);
        }
    }
}
