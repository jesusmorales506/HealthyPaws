using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using System.Drawing;
using System.Drawing.Imaging;

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
        public async Task<IActionResult> Index(string searchInvApp)
        {
            var invApp = _context.AppointmentInventories
              .Include(p => p.Appointment)
              .Include(p => p.Inventory).AsQueryable();

            if (!string.IsNullOrEmpty(searchInvApp))
            {
                int.TryParse(searchInvApp, out int parsedInvAppId);
                invApp = invApp.Where(p => p.appointmentId == parsedInvAppId || p.inventoryID == parsedInvAppId || p.appointmentInventoryId == parsedInvAppId);
            }
            var hpContext = await invApp.ToListAsync();

            if (hpContext.Count == 0)
            {
                ViewBag.NoResultados = true;
            }
            else
            {
                ViewBag.NoResultados = false;
            }

            return View(hpContext);
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
            // ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "name");
            //ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");            
            ViewData["inventoryID"] = new SelectList(_context.Inventories.Where(i => i.category == "Medicamento"), "inventoryId", "name");

            ViewData["appointmentId"] = new SelectList(_context.Appointments
        .Select(a => new
        {
            AppointmentId = a.AppointmentId,
            DisplayName = $"{a.AppointmentId} - {a.PetFile.name} - {a.PetFile.idNumber}"
        }), "AppointmentId", "DisplayName");
            return View();
        }

        // POST: AppointmentInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose,status")] AppointmentInventory appointmentInventory)
        {
            appointmentInventory.status = true;

            if (ModelState.IsValid)
            {
                _context.Add(appointmentInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["inventoryID"] = new SelectList(_context.Inventories.Where(i => i.category == "Medicamento"),"inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);

            return View();
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
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);
            ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "brand", appointmentInventory.inventoryID);





            return View();
        }

        // POST: AppointmentInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose,status")] AppointmentInventory appointmentInventory)
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
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);
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

            appointmentInventory.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AppointmentInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var appointmentInventory = await _context.AppointmentInventories.FindAsync(id);
            if (appointmentInventory != null)
            {
                appointmentInventory.status = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
