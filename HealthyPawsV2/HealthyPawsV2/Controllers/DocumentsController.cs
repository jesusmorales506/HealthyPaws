using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Identity;

namespace HealthyPawsV2.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly HPContext _context;
        

        public DocumentsController(HPContext context)
        {
            _context = context;
            

        }

		// GET: Documents
		[HttpGet]
		public async Task<IActionResult> Index(string documentSearch)
        {

            var documents = _context.Documents.AsQueryable();

			if (!string.IsNullOrEmpty(documentSearch))
			{
				documents = documents.Where(m => m.name.Contains(documentSearch));
			}

            var documentResult = await documents.ToListAsync();

			ViewData["AppointmentId"] = new SelectList(
            from appointment in _context.Appointments
            join petFile in _context.PetFiles on appointment.petFileId equals petFile.petFileId
            join user in _context.ApplicationUser on petFile.ownerId equals user.Id
            select new
            {
                AppointmentId = appointment.AppointmentId,
                DisplayText = $"{appointment.AppointmentId} - {petFile.name} - {user.name} - {user.idNumber}  "
            },
            "AppointmentId",
            "DisplayText"
            );

            ViewData["petFileName"] = new SelectList(
            from pf in _context.PetFiles
            select new SelectListItem
            {
                Value = pf.petFileId.ToString(),
                Text = $"{pf.petFileId} - {pf.name}"
            },
                "Value",
                "Text"
                );


            //ViewBag.petFileId = new SelectList(petFiles, "Value", "Text");
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(documentResult);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId");
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,documentId,petFileId,name,category,fileType,status,fileType")] Document document, IFormFile File)
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
                return RedirectToAction(nameof(Index));
            }

            // Volver a llenar el ViewData en caso de error de validación
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("documentId, AppointmentId, petFileId, name, category, fileType, status")] Document document, IFormFile File)
        {
            if (id != document.documentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    if (File != null && File.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await File.CopyToAsync(memoryStream);
                            document.fileType = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        
                        var existingDocument = await _context.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.documentId == id);
                        if (existingDocument != null)
                        {
                            document.fileType = existingDocument.fileType;  
                        }
                    }

                    
                    _context.Entry(document).State = EntityState.Detached;

                    
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.documentId))
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

            // Volver a llenar ViewData en caso de error de validación
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "AppointmentId", "AppointmentId");
            ViewData["petFileId"] = new SelectList(_context.PetFiles, "petFileId", "petFileId", document.petFileId);
            ViewData["Users"] = new SelectList(_context.ApplicationUser, "Id", "UserName");

            return View(document);
        }



        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.documentId == id);
        }



        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

			document.status = false;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

			document.status = false;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

        


        //This is to download the files
        public IActionResult Download(int id)
        {
            var document = _context.Documents.FirstOrDefault(d => d.documentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return File(document.fileType, "application/octet-stream", document.name);
        }
    }
}
