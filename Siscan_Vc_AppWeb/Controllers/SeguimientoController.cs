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
            try
        {

            }
            catch (Exception)
            {

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

            }

        public async Task<IActionResult> consultar()
        {
            return View();
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
