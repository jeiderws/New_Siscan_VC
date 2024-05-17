using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public SeguimientoController(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }

        public async Task<IActionResult> Consultar(int page = 1, int pageSize = 5)
        {
            ViewModelSeguimiento model = null;
            var items = await _dbSiscanContext.SeguimientoInstructorAprendizs
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
            var totalItems = await _dbSiscanContext.SeguimientoInstructorAprendizs.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            foreach (var item in items)
            {
                model = new ViewModelSeguimiento(item)
                {
                    IdSeguimiento = item.IdSeguimiento,

                    NumeroDocumentoAprendiz = item.NumeroDocumentoAprendiz,
                    NombreAprendiz = item.NumeroDocumentoAprendizNavigation.NombreAprendiz,
                    ApellidoAprendiz = item.NumeroDocumentoAprendizNavigation.ApellidoAprendiz,
                    CorreoAprendiz = item.NumeroDocumentoAprendizNavigation.CorreoAprendiz,
                    TelefonoAprendiz = item.NumeroDocumentoAprendizNavigation.CelAprendiz,
                    ProgramAprendiz = item.NumeroDocumentoAprendizNavigation.FichaNavigation.Programa.NombrePrograma,
                    FichaAprendiz = item.NumeroDocumentoAprendizNavigation.Ficha.ToString(),

                    //Instructor
                    NombreInstructor = item.NumeroDocumentoInstructorNavigation.NombreInstructor,
                    ApellidoInstructor = item.NumeroDocumentoInstructorNavigation.ApellidoInstructor,
                    CorreoInstructor = item.NumeroDocumentoInstructorNavigation.CorreoInstructor,
                    TelefonoInstructor = item.NumeroDocumentoInstructorNavigation.CelInstructor,

                    //coformador
                    NombreCoformador = item.IdCoformadorNavigation.NombreCoformador,
                    ApellidoCoformador = item.IdCoformadorNavigation.ApellidoCoformador,
                    CorreoCoformador = item.IdCoformadorNavigation.CorreoCoformador,
                    TelefonoCoformador = item.IdCoformadorNavigation.CelCoformador,

                    //Empresa
                    NitEmpresa = item.NitEmpresa,
                    NombreEmpresa = item.NitEmpresaNavigation.NombreEmpresa,
                    AreaEmpresa = item.IdAreaEmpresaNavigation.NombreArea,

                    //practicas
                    FechaInicio = item.FechaInicio,
                    FechaFinalizacion = item.FechaFinalizacion,
                    NombreModalidad = item.IdModalidadNavigation.NombreModalidad

                };
            }

            return View(model);
        }


        [HttpGet("Seguimiento/Show/{id}")]
        public async Task<IActionResult> Show(int id)
        {

            var item = await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return View("Consultar",item);
            }

           
        }
    }
}
