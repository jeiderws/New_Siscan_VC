using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.ClasesService;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbSiscanContext>(opc=>{
    opc.UseSqlServer(builder.Configuration.GetConnectionString("cadenaDB"));
});
builder.Services.AddScoped<IGenericRepository<Aprendiz>, AprendizRepository>();
builder.Services.AddScoped<IGenericRepository<Instructor>, InstructorRepository>();
builder.Services.AddScoped<IGenericRepository<InscripcionTyt>, InscripcionTYTRepository>();
builder.Services.AddScoped<IGenericRepository<SeguimientoInstructorAprendiz>, SeguimientoInsApreRepository>();
builder.Services.AddScoped<IGenericRepository<Empresa>, EmpresaRepository>();
builder.Services.AddScoped<IGenericRepository<AsignacionArea>, AsignacionAreaRepository>();
builder.Services.AddScoped<IGenericRepository<Programas>, ProgramasRepository>();
builder.Services.AddScoped<IGenericRepository<Ficha>, FichaRepository>();
builder.Services.AddScoped<IGenericRepository<AsignacionFicha>, AsignacionFichaRepository>();
builder.Services.AddScoped<IGenericRepository<Coformador>, CoformadorRepository>();
builder.Services.AddScoped<IGenericRepository<SeguimientoArchivo>, SeguimientoArchivoRepository>();
builder.Services.AddScoped<IGenericRepository<Actividade>, ActividadRepository>();
builder.Services.AddScoped<IGenericRepository<Observacion>, ObservacionesRepository>();

builder.Services.AddScoped<IAprendizService, AprendizService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IInscripcionTYTService, InscripcionTYTService>();
builder.Services.AddScoped<ISeguimientoService, SeguimientoService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IAsignacionService, AsignacionAreaService>();
builder.Services.AddScoped<IProgramasService, ProgramasService>();
builder.Services.AddScoped<IFichaService, FichaService>();
builder.Services.AddScoped<IAsigancionFichas, AsignacionFichasService>();
builder.Services.AddScoped<ICoformadorService, CoformadorService>();
builder.Services.AddScoped<ISeguimientoArchivoService, SeguimientoArchivoService>();
builder.Services.AddScoped<IActividadService, ActividadService>();
builder.Services.AddScoped<IObservacionesService, ObservacionesService>();
// Establecer el contexto de licencia de EPPlus
var app = builder.Build();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

