using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using HealthyPawsV2.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;


public class UserController : Controller
{
    private readonly HPContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(HPContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _userManager.Users.ToListAsync();

        if (usuarios != null && usuarios.Count == 0)
        {
            ViewData["NoResultados"] = true;
        }
        else
        {
            ViewData["NoResultados"] = false;
        }

        return View(usuarios);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Usuarios/Create
    public IActionResult Create()
    {
        return View();
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create(ApplicationUser usuario)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var result = await _userManager.CreateAsync(usuario);
    //        if (result.Succeeded)
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        else
    //        {
    //            // Manejar errores de creación de usuario
    //        }
    //    }
    //    return View(usuario);
    //}

    // POST: Mascotas/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("name,surnames,lastConnection,idType,idNumber,phone1,phone2,phone3,Email")] ApplicationUser usuario, string provinceName, string cantonName, string districtName)
    {
        //We get the last connection of the user
        usuario.lastConnection = DateTime.Now.Date + DateTime.Now.TimeOfDay;

        //This is a Validation to know if the user is adding the Name and Surnames
        if (string.IsNullOrEmpty(usuario.name) || string.IsNullOrEmpty(usuario.surnames))
        {
            ModelState.AddModelError("", "Los campos de Nombre y Apellidos son obligatorios.");
            return View(usuario);
        }

        //Create password with following format: Ale.c123
        string password = char.ToUpper(usuario.name[0]) + usuario.name.Substring(1, 2).ToLower() + "." + char.ToLower(usuario.surnames[0]) + "123";
        //hash the password
        string hashedPassword = PasswordUtility.HashPassword(usuario, password);
        usuario.PasswordHash = hashedPassword;

        usuario.status = true;
        usuario.UserName = usuario.Email;
        usuario.NormalizedUserName = usuario.Email.ToUpper();
        usuario.NormalizedEmail = usuario.Email.ToUpper();
        usuario.PhoneNumber = usuario.phone1;


        //Obtener usuario logueado y asignarlo como UsuarioCreacionId
        //var identidad = User.Identity as ClaimsIdentity;
        //var usuarioLoggueadoTask = RolesUtils.ObtenerUsuarioLogueado(_userManager, new ClaimsPrincipal(identidad));
        //usuarioLoggueadoTask.Wait();
        //var usuarioLoggueado = usuarioLoggueadoTask.Result;
        //mascota.UsuarioCreacionId = usuarioLoggueado.Id;

        ////If usuario no es admin, asignar el usuario loggeado al duenoId
        //if (!RolesUtils.UsuarioLogueadoEsRol(identidad, "Admin") || !RolesUtils.UsuarioLogueadoEsRol(identidad, "Veterinario"))
        //{
        //    mascota.DuenoId = usuarioLoggueado.Id;
        //}

        if (ModelState.IsValid)
        {
            var address = new Address
            {
                province = provinceName,
                canton = cantonName,
                district = districtName
            };

            //Lines 131/132/133 are creating the addresses and adding them to btoh tables Addresses and Users.
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            usuario.addressId = address.AddressId;

            _context.ApplicationUser.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //ViewData["IdRazaMascota"] = new SelectList(_context.RazaMascotas, "IdRazaMascota", "Nombre", mascota.IdRazaMascota);
        //ViewData["Usuarios"] = new SelectList(_context.ApplicationUser, "Id", "Nombre", mascota.UsuarioCreacionId);
        //ViewData["TiposMascota"] = new SelectList(_context.TipoMascotas, "IdTipoMascota", "Nombre", mascota.IdTipoMascota);

        return View(usuario);
    }

    //public async Task<IActionResult> Edit(string id)
    //{
    //    var usuario = await _context.ApplicationUser
    //        .FirstOrDefaultAsync(u => u.Id == id);

    //    if (usuario == null)
    //    {
    //        return NotFound();
    //    }

    //    // Cargar las provincias
    //    var provincias = await _context.Addresses.Select(a => a.province).Distinct().ToListAsync();
    //    ViewBag.Provincias = new SelectList(provincias);

    //    // Cargar cantones y distritos basados en la provincia del usuario
    //    var cantones = await _context.Addresses
    //        .Where(a => a.province == usuario.province)
    //        .Select(a => a.canton)
    //        .Distinct()
    //        .ToListAsync();
    //    ViewBag.Cantones = new SelectList(cantones);

    //    var distritos = await _context.Addresses
    //        .Where(a => a.canton == usuario.canton)
    //        .Select(a => a.district)
    //        .Distinct()
    //        .ToListAsync();
    //    ViewBag.Distritos = new SelectList(distritos);

    //    return View(usuario);
    //}


    //public async Task<IActionResult> Edit(string id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var usuario = await _userManager.FindByIdAsync(id);
    //    if (usuario == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(usuario);
    //}

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var usuario = await _context.ApplicationUser.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        var address = await _context.Addresses.FindAsync(usuario.addressId);

        // Asegúrate de pasar la dirección
        ViewBag.Provincia = address?.province;
        ViewBag.Canton = address?.canton;
        ViewBag.Distrito = address?.district;

        return View(usuario);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ApplicationUser usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var userToUpdate = await _userManager.FindByIdAsync(id);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                userToUpdate.name = usuario.name;
                userToUpdate.surnames = usuario.surnames;
                userToUpdate.phone1 = usuario.phone1;
                userToUpdate.phone2 = usuario.phone2;
                userToUpdate.phone3 = usuario.phone3;
                userToUpdate.Email = usuario.Email;

                // Obtener la dirección
                var address = await _context.Addresses.FindAsync(userToUpdate.addressId);
                if (address != null)
                {
                    address.province = usuario.province; // O asegúrate de tener estos valores en el formulario
                    address.canton = usuario.canton;
                    address.district = usuario.district;
                }

                // Actualizar usuario
                var result = await _userManager.UpdateAsync(userToUpdate);
                if (!result.Succeeded)
                {

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(usuario);
                }

                // Actualizar dirección
                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(usuario.Id))
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
        return View(usuario);
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            var result = await _userManager.DeleteAsync(usuario);
            if (!result.Succeeded)
            {
                // Manejar errores de eliminación de usuario
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(string id)
    {
        return _userManager.FindByIdAsync(id) != null;
    }
}









