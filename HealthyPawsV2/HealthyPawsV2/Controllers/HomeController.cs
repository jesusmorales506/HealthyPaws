using HealthyPawsV2.DAL;
using HealthyPawsV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HealthyPawsV2.Controllers
{
    public class SearchViewModel
    {
        public List<PetType> PetTypes { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly HPContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(HPContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Search(string GlobalSearch)
        {
            var petTypeQuery = _context.PetTypes.AsQueryable();
            var usersQuery = _userManager.Users.AsQueryable();

            var petBreedsQuery = _context.PetBreeds
                .Include(r => r.PetType)
                .Where(p => p.status)
                .AsQueryable();

            var appointments = _context.Appointments
                .Include(a => a.PetFile)
                .Include(a => a.owner)
                .Include(a => a.vet)
                .AsQueryable();

            if (!string.IsNullOrEmpty(GlobalSearch))
            {
                petTypeQuery = petTypeQuery.Where(m => m.name.Contains(GlobalSearch));
                usersQuery = usersQuery.Where(p => p.name.Contains(GlobalSearch));
                petBreedsQuery = petBreedsQuery.Where(p => p.name.Contains(GlobalSearch) || p.PetType.name.Contains(GlobalSearch));
                appointments = appointments.Where(a => a.owner.name.Contains(GlobalSearch) || a.vet.name.Contains(GlobalSearch) || a.PetFile.name.Contains(GlobalSearch));


            }

            var petTypeResult = await petTypeQuery.ToListAsync();
            var usersResult = await usersQuery.ToListAsync();
            var petBreedResult = await petBreedsQuery.ToListAsync();
            var appointmentsResult = await appointments.ToListAsync();

            ViewBag.PetTypes = petTypeResult;
            ViewBag.Users = usersResult;
            ViewBag.PetBreeds = petBreedsQuery;
            ViewBag.Appointment = appointments;
            ViewBag.NoResultadosPetTypes = petTypeResult.Count == 0;
            ViewBag.NoResultadosUsers = usersResult.Count == 0;
            ViewBag.NoResultadosPetBreeds = petBreedResult.Count == 0;
            ViewBag.NoResultadosAppointment = appointmentsResult.Count == 0;

            return View();
        }





        public IActionResult Index()
        {
            return View();
        }

    }
}
