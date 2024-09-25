//using HealthyPawsV2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//using HealthyPawsV2.Data;
using HealthyPawsV2.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using HealthyPawsV2.DAL;

//Conexion a Base de datos para uso de Identity / Usuarios y Roles
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");
builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<AuthContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();
//Conexion a Base de datos para uso de Identity / Usuarios y Roles


//Inyeccion de Dependencias y Conexion a BD General con Entidades 
builder.Services.AddDbContext<HPContext>(options => options.UseSqlServer("name=ConnHealthyDB"));
//Inyeccion de Dependencias y Conexion a BD General con Entidades

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
