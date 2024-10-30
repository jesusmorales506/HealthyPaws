using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyPawsV2.DAL;
using HealthyPawsV2.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


public class UserController : Controller
{
	private readonly HPContext _context;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserController(HPContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
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
	public async Task<IActionResult> CreateAsync()
	{
		ViewBag.Roles = await RolesUtils.GetAllRoles(_roleManager);

		return View();
	}

	// POST: Mascotas/Create
	// To protect from overposting attacks, enable the specific properties you want to bind to.
	// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([Bind("name,surnames,lastConnection,idType,idNumber,phone1,phone2,phone3,Email")] ApplicationUser usuario, string provinceName, string cantonName, string districtName, string role)
	{
		//We get the last connection of the user
		usuario.lastConnection = DateTime.Now.Date + DateTime.Now.TimeOfDay;
		usuario.status = true;
		usuario.UserName = usuario.Email;
		usuario.NormalizedUserName = usuario.Email.ToUpper();
		usuario.NormalizedEmail = usuario.Email.ToUpper();
		usuario.PhoneNumber = usuario.phone1;

		//This is a Validation to know if the user is adding the Name and Surnames
		if (string.IsNullOrEmpty(usuario.name) || string.IsNullOrEmpty(usuario.surnames))
		{
			ModelState.AddModelError("", "Los campos de Nombre y Apellidos son obligatorios.");
			return View(usuario);
		}

		//Create password with some format
		string password = char.ToUpper(usuario.name[0]) + usuario.name.Substring(1, 2).ToLower() + "." + char.ToLower(usuario.surnames[0]) + "123";
		//hash the password
		string hashedPassword = PasswordUtility.HashPassword(usuario, password);
		usuario.PasswordHash = hashedPassword;

		//Get logged user and assign it as loggedUser
		var userIdentify = new ClaimsPrincipal(User.Identity);
		var loggedUserTask = RolesUtils.GetLoggedUser(_userManager, userIdentify);
		loggedUserTask.Wait();
		var loggedUser = loggedUserTask.Result;

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

			//Create user and add the password
			var result = await _userManager.CreateAsync(usuario, password);

			if (result.Succeeded)
			{
				//Assign role "User" to the new user if there is no indicated role previously.
				if (role == null)
				{
					var resultRole = await _userManager.AddToRoleAsync(usuario, "User");

					if (resultRole.Succeeded)
					{
						return RedirectToAction(nameof(Index));
					}
					else
					{
						foreach (var error in resultRole.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
		}

		//ViewData["IdRazaMascota"] = new SelectList(_context.RazaMascotas, "IdRazaMascota", "Nombre", mascota.IdRazaMascota);
		//ViewData["Usuarios"] = new SelectList(_context.ApplicationUser, "Id", "Nombre", mascota.UsuarioCreacionId);
		//ViewData["TiposMascota"] = new SelectList(_context.TipoMascotas, "IdTipoMascota", "Nombre", mascota.IdTipoMascota);

		return View(usuario);
	}


    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var usuario = await _context.ApplicationUser.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
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

     
                var result = await _userManager.UpdateAsync(userToUpdate);
                if (!result.Succeeded)
                {

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(usuario);
                }


            }
            catch (DbUpdateConcurrencyException)
            {
               
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

		var usuario = await _context.ApplicationUser.FindAsync(id);
		if (usuario == null)
		{
			return NotFound();
		}

		usuario.status = false;
		await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        //return View(usuario);
	}

	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(string id)
	{
		var usuario = await _context.ApplicationUser.FindAsync(id);
        if (usuario != null)
		{
			usuario.status = false;
		}
		return RedirectToAction(nameof(Index));
	}

	
}









