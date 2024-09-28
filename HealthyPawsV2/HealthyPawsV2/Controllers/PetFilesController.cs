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
    public class PetFilesController : Controller
    {
        private readonly HPContext _context;

        public PetFilesController(HPContext context)
        {
            _context = context;
        }

        // GET: PetFiles
        public async Task<IActionResult> Index()
        {
            var hPContext = _context.PetFiles.Include(p => p.PetBreed);
            return View(await hPContext.ToListAsync());
        }

        // GET: PetFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petFile = await _context.PetFiles
                .Include(p => p.PetBreed)
                .FirstOrDefaultAsync(m => m.petFileId == id);
            if (petFile == null)
            {
                return NotFound();
            }

            return View(petFile);
        }

        // GET: PetFiles/Create
        public IActionResult Create()
        {
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name");
            return View();
        }

        // POST: PetFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("petFileId,petBreedId,idNumber,name,petTypeId,gender,age,weight,creationDate,vaccineHistory,castration,description,status")] PetFile petFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name", petFile.petBreedId);
            return View(petFile);
        }

        // GET: PetFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petFile = await _context.PetFiles.FindAsync(id);
            if (petFile == null)
            {
                return NotFound();
            }
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name", petFile.petBreedId);
            return View(petFile);
        }

        // POST: PetFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("petFileId,petBreedId,idNumber,name,petTypeId,gender,age,weight,creationDate,vaccineHistory,castration,description,status")] PetFile petFile)
        {
            if (id != petFile.petFileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetFileExists(petFile.petFileId))
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
            ViewData["petBreedId"] = new SelectList(_context.PetBreeds, "petBreedId", "name", petFile.petBreedId);
            return View(petFile);
        }

        // GET: PetFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petFile = await _context.PetFiles
                .Include(p => p.PetBreed)
                .FirstOrDefaultAsync(m => m.petFileId == id);
            if (petFile == null)
            {
                return NotFound();
            }

            return View(petFile);
        }

        // POST: PetFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petFile = await _context.PetFiles.FindAsync(id);
            if (petFile != null)
            {
                _context.PetFiles.Remove(petFile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetFileExists(int id)
        {
            return _context.PetFiles.Any(e => e.petFileId == id);
        }
    }
}
