using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Identity;
using HealthyPawsV2.Utils;

namespace HealthyPawsV2.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppointmentsController(HPContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string searchAppointment)
        {
            var appointments = _context.Appointments
                .Include(a => a.PetFile)
                .Include(a => a.owner)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchAppointment))
            {
                appointments = appointments.Where(a =>
                    a.AppointmentId.ToString().Contains(searchAppointment) ||
                    a.PetFile.name.Contains(searchAppointment) ||
                    a.owner.UserName.Contains(searchAppointment) ||
                    a.veterinario.UserName.Contains(searchAppointment)
                );
            }

            var appointmentList = await appointments.ToListAsync();


            ViewData["Users"] = new SelectList(_context.ApplicationUser
                .Select(u => new
                {
                    Id = u.Id,
                    DisplayName = $"{u.name} {u.surnames} - {u.idNumber}"
                }), "Id", "DisplayName");


            ViewData["Pets"] = new SelectList(
            from pet in _context.PetFiles
            join user in _context.ApplicationUser on pet.idNumber equals user.Id 
            select new
            {
            Id = pet.petFileId,
            DisplayName = $"{pet.name} - {user.name} {user.surnames} - {user.idNumber}"
            },
            "Id",
            "DisplayName"
            );

            //Get users with "User" role for Dueño dropdown in create page
            var ownersTask = RolesUtils.GetUsersPerRole(_roleManager, _userManager, "User");
            ownersTask.Wait();
            var owners = ownersTask.Result;
            var ownerList = owners.Select(user => new
            {
                Id = user.Id, // Asegúrate de que la propiedad sea "Id"
                DisplayName = $"{user.name} {user.surnames} - {user.idNumber}" // Asegúrate de usar las propiedades correctas
            }).ToList();
            ViewData["Owners"] = new SelectList(ownerList, "Id", "DisplayName");


            ////Get users with "Vet" role for Veterinario dropdown in create page
            var vetsTask = RolesUtils.GetUsersPerRole(_roleManager, _userManager, "Vet");
            vetsTask.Wait();
            var vets = vetsTask.Result.Where(v => v.status);
            var vetList = vets.Select(vet => new
            {
                Id = vet.Id,
                DisplayName = $"{vet.name} {vet.surnames} - {vet.idNumber}" 
            }).ToList();
            ViewData["Vets"] = new SelectList(vetList, "Id", "DisplayName");

            ViewBag.NoResultados = appointmentList.Count == 0;

            return View(appointmentList);
        }


        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.PetFile)
                .Include(a => a.owner)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "name");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name");

            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,petFileId,vetId,ownerId,Date,description,status,diagnostic,Additional")] Appointment appointment)
        {
            appointment.diagnostic = "Aun no hay Diagnostico"; // I Added this just to add Something
            // Verificar si la fecha de la cita es anterior a la fecha actual
            if (appointment.Date < DateTime.Now)
            {
                ModelState.AddModelError("date", "La fecha de la cita no puede ser anterior a la fecha actual.");
            }

            if (ModelState.IsValid)
            {
                appointment.status = "Agendada";
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "name");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name");
            return View(appointment);
        }



        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Cargar opciones de estado
            ViewBag.StatusOptions = new SelectList(new List<string>
    {
        "Completada",
        "Agendada",
        "Cancelada",
        "Pendiente"
    });

            // Cargar datos de mascotas y usuarios
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "name");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name");

            // Cargar opciones de medicamentos
            ViewData["Medicamentos"] = new SelectList(_context.Inventories
                .Where(i => i.category == "Medicamento"), "inventoryId", "name");


            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,petFileId,vetId,ownerId,documentId,Date,description,status,diagnostic,Additional")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
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
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "name");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "name");
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.PetFile)
                .Include(a => a.owner)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.status = "Cancelada";
                _context.Appointments.Update(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }


        //This a method ti add a med into the appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMed([Bind("appointmentInventoryId,appointmentId,inventoryID,dose,measuredose,status")] AppointmentInventory appointmentInventory)
        {
            appointmentInventory.status = true;

            if (ModelState.IsValid)
            {
                _context.Add(appointmentInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = appointmentInventory.appointmentId });
                
            }

            // ViewData["inventoryID"] = new SelectList(_context.Inventories, "inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["inventoryID"] = new SelectList(_context.Inventories.Where(i => i.category == "Medicamento"), "inventoryId", "name", appointmentInventory.inventoryID);
            ViewData["appointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId", appointmentInventory.appointmentId);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoc([Bind("AppointmentId,documentId,petFileId,name,category,fileType,status,fileType")] Document document, IFormFile File)
        {
            document.status = true;

            if (ModelState.IsValid)
            {
                if (File != null && File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await File.CopyToAsync(memoryStream);
                        document.fileType = memoryStream.ToArray();
                    }
                }
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = document.AppointmentId });
            }

            // Volver a llenar el ViewData en caso de error de validación
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View();
        }



    }

}
