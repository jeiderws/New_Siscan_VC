using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var modelview = new ModelViewInstructor
            {
                OpcionesTpDoc = _dbSiscanContext.TipoDocumentos.Select(o => new SelectListItem
                {
                    Value = o.IdTipoDocumento.ToString(),
                    Text = o.TipoDocumento1
                }).ToList()
            };
            return View(modelview);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(ModelViewInstructor mvinstructor)
        {
            Instructor instruc= new Instructor();
            if (instruc!=null)
            {
                instruc = new Instructor()
                {
                    NumeroDocumentoInstructor = mvinstructor.Instructor.NumeroDocumentoInstructor,
                    NombreInstructor = mvinstructor.Instructor.NombreInstructor,
                    ApellidoInstructor = mvinstructor.Instructor.ApellidoInstructor,
                    CorreoInstructor = mvinstructor.Instructor.CorreoInstructor,
                    CelInstructor = mvinstructor.Instructor.CelInstructor,
                    IdTipodocumento = mvinstructor.OpcSeleccionada
                };
                _dbSiscanContext.Instructors.Add(instruc);
                await _dbSiscanContext.SaveChangesAsync();
                RedirectToAction(nameof(Registro));
            }
            return View(mvinstructor);
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
