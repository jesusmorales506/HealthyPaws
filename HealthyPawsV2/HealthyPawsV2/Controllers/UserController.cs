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

	[HttpGet]
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
		ViewBag.Roles = await RolesUtils.GetAllRoles(_roleManager);

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
	public async Task<IActionResult> Create(
		[Bind("name,surnames,lastConnection,idType,idNumber,phone1,phone2,phone3,Email")] ApplicationUser user,
		string provinceName, string cantonName, string districtName, string role)
	{
		// Set the roles for the view
		ViewBag.Roles = await RolesUtils.GetAllRoles(_roleManager);

		// Validate the model state
		if (!ModelState.IsValid)
		{
			return View(user); // Return with current model state errors
		}

		// Validate required fields
		if (string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.surnames))
		{
			ModelState.AddModelError("", "Los campos de Nombre y Apellidos son obligatorios.");
			return View(user);
		}

		// Assign last connection date
		user.lastConnection = DateTime.Now;

		// Set username and normalized values
		user.UserName = user.Email;
		user.NormalizedUserName = user.Email.ToUpper();
		user.NormalizedEmail = user.Email.ToUpper();
		user.PhoneNumber = user.phone1;
		user.status = true;

		// Create password
		string password = $"{char.ToUpper(user.name[0])}{user.name.Substring(1, 2).ToLower()}.{char.ToLower(user.surnames[0])}123";
		user.PasswordHash = PasswordUtility.HashPassword(user, password);

		// Create address
		var address = new Address
		{
			province = provinceName,
			canton = cantonName,
			district = districtName
		};

		// Add address to context
		_context.Addresses.Add(address);
		await _context.SaveChangesAsync();

		// Link address to user
		user.addressId = address.AddressId;

		// Create user
		var result = await _userManager.CreateAsync(user, password);
		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(user); // Return on error with model state
		}

		// Assign role
		if (string.IsNullOrEmpty(role))
		{
			//When role is null, the vet is adding an user, so the role should be 'User'
			var resultRole = await _userManager.AddToRoleAsync(user, "User");

			if (!resultRole.Succeeded)
			{
				foreach (var error in resultRole.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(user); // Return on error with model state
			}
		}
		else
		{
			//Asign role in dropdown to the user
			var resultRole = await _userManager.AddToRoleAsync(user, role);
		}

		// Redirect to index if everything succeeded
		return RedirectToAction(nameof(Index));
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









