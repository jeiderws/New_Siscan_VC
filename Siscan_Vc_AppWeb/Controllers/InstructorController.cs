using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class InstructorController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public InstructorController(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public async Task<IActionResult> Registro()
        {
            ViewBag.ItemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(Instructor instructor)
        {
            if (instructor!=null)
            {
                var instruc = new Instructor()
                {
                    NumeroDocumentoInstructor = instructor.NumeroDocumentoInstructor,
                    NombreInstructor = instructor.NombreInstructor,
                    ApellidoInstructor = instructor.ApellidoInstructor,
                    CorreoInstructor = instructor.CorreoInstructor,
                    CelInstructor = instructor.CelInstructor,
                    IdTipodocumento = instructor.IdTipodocumento,
                };
                _dbSiscanContext.Instructors.Add(instruc);
                await _dbSiscanContext.SaveChangesAsync();
            }
            return View(instructor);
        }

        public IActionResult Consultar()
        {
            return View();
        }

        public IActionResult Editar()
        {
            return View();
        }
    }
}
