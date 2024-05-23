using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class InstructorController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly IInstructorService _instructorService;
        public InstructorController(DbSiscanContext dbSiscanContext, IInstructorService instructorService)
        {
            _dbSiscanContext = dbSiscanContext;
            _instructorService = instructorService;
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
            ModelViewInstructor instrucvm = new ModelViewInstructor();
            if (instrucvm != null)
            {
                var instruc = new Instructor()
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
                TempData["AlertInstrcAdd"] = "Instructor guardado correctamente";

                instrucvm = new ModelViewInstructor
                {
                    Instructor = instruc
                };
                return RedirectToAction(nameof(Registro));
            }
            return View(mvinstructor);
        }
        [HttpGet]
        public async Task<IActionResult> Consultar(string nmdoc)
        {
            List<ViewModelInstructor> listaInstructores = new List<ViewModelInstructor>();
            IQueryable<Instructor> queryInstructor = await _instructorService.GetAll();
            listaInstructores = queryInstructor.Select(i => new ViewModelInstructor(i)
            {
                NumeroDocumentoInstructor = i.NumeroDocumentoInstructor,
                NombreInstructor = i.NombreInstructor,
                ApellidoInstructor = i.ApellidoInstructor,
                CorreoInstructor = i.CorreoInstructor,
                CelInstructor = i.CelInstructor,
                IdTipodocumento = i.IdTipodocumento,
                Tipodocumento = i.IdTipodocumentoNavigation
            }).ToList();

            Instructor instructor = new Instructor();
            foreach (var instr in queryInstructor)
            {
                if (instr.NumeroDocumentoInstructor == nmdoc)
                {
                    instructor = instr;
                    break;
                }
            }
            ModelViewInstrc viewModel = new ModelViewInstrc
            {
                Instructor = instructor,
                ListaInstructores = listaInstructores
            };
            TempData["instructorConsultAlert"] = "No hay resultados";
            return View(viewModel);
        }

        public IActionResult Editar()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string nmDoc)
        {
            try
            {
                var instructor = await _dbSiscanContext.Instructors.FirstOrDefaultAsync(i => i.NumeroDocumentoInstructor == nmDoc);
                if (instructor == null)
                {
                    TempData["AlertInstrucNoEncontrado"] = "El Instructor no fue encontrado";
                }
                TempData["AlertEliminadoInstruc"] = "Instructor eliminado correctamente!!";
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.Where(s => s.NumeroDocumentoInstructor == nmDoc).ToListAsync();
                if (seguimiento.Count > 0)
                {
                    _dbSiscanContext.SeguimientoInstructorAprendizs.RemoveRange(seguimiento);
                }
                var ficha = await _dbSiscanContext.Fichas.Where(f => f.NumeroDocumentoInstructor == nmDoc).ToListAsync();
                if (ficha.Count > 0)
                {
                    foreach (var f in ficha)
                    {
                        f.NumeroDocumentoInstructor = null;
                        _dbSiscanContext.Fichas.UpdateRange(f);
                    }
                }
               await _instructorService.Delete(nmDoc);
                await _dbSiscanContext.SaveChangesAsync();
                return Json(new { success = true, message = "El instructor se eliminó correctamente." });

            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Se produjo un error al intentar eliminar el instructor: " + ex.Message });

            }
        }
    }
}
