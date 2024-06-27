using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.ClasesService;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbSiscanContext>(opc=>{
    opc.UseSqlServer(builder.Configuration.GetConnectionString("cadenaDB"));
});
builder.Services.AddScoped<IGenericRepository<Aprendiz>,AprendizRepository>();
builder.Services.AddScoped<IGenericRepository<Instructor>, InstructorRepository>();
builder.Services.AddScoped<IGenericRepository<InscripcionTyt>, InscripcionTYTRepository>();
builder.Services.AddScoped<IGenericRepository<SeguimientoInstructorAprendiz>, SeguimientoInsApreRepository>();
builder.Services.AddScoped<IGenericRepository<Empresa>, EmpresaRepository>();
builder.Services.AddScoped<IGenericRepository<AsignacionArea>, AsignacionAreaRepository>();
builder.Services.AddScoped<IGenericRepository<Programas>, ProgramasRepository>();
builder.Services.AddScoped<IGenericRepository<Ficha>, FichaRepository>();

builder.Services.AddScoped<IAprendizService,AprendizService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IInscripcionTYTService, InscripcionTYTService>();
builder.Services.AddScoped<ISeguimientoService, SeguimientoService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IAsignacionService, AsignacionAreaService>();
builder.Services.AddScoped<IProgramasService, ProgramasService>();
builder.Services.AddScoped<IFichaService, FichaService>();

var app = builder.Build();

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

