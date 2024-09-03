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
        private readonly ISeguimientoService _seguimientoService;
        private readonly IAsigancionFichas _asigancionFichas;
        public InstructorController(DbSiscanContext dbSiscanContext, IInstructorService instructorService, ISeguimientoService seguimientoService, IAsigancionFichas asigancionFichas)
        {
            _dbSiscanContext = dbSiscanContext;
            _instructorService = instructorService;
            _seguimientoService = seguimientoService;
            _asigancionFichas = asigancionFichas;
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
            try
            {
                ModelViewInstructor instrucvm = new ModelViewInstructor();
                if (mvinstructor != null)
                {
                    Instructor instructor = await _instructorService.GetForDoc(mvinstructor.Instructor.NumeroDocumentoInstructor);
                    if (instructor != null)
                    {
                        TempData["ValIntrcExiste"] = "Ya existe un intructor con este numero de documento";
                        return RedirectToAction(nameof(Registro));
                    }
                    else
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
                            Instructor = instruc,
                            OpcionesTpDoc = _dbSiscanContext.TipoDocumentos.Select(o => new SelectListItem
                            {
                                Value = o.IdTipoDocumento.ToString(),
                                Text = o.TipoDocumento1
                            }).ToList()
                        };
                        return RedirectToAction(nameof(Registro));
                    }
                }

                instrucvm = new ModelViewInstructor
                {
                    OpcionesTpDoc = _dbSiscanContext.TipoDocumentos.Select(o => new SelectListItem
                    {
                        Value = o.IdTipoDocumento.ToString(),
                        Text = o.TipoDocumento1
                    }).ToList()
                }; 
            }
            catch (Exception ex)
            {
                TempData["ErrorGuardarInstrct"] = ex.Message;
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
        public async Task llenarcombo()
        {
            var itemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string nmdoc)
        {
            await llenarcombo();
            var viewModel = new ModelViewInstructor();
            if (nmdoc != null)
            {
                var instruc = await _instructorService.GetForDoc(nmdoc);
                viewModel = new ModelViewInstructor
                {
                    Instructor = instruc
                };
                if (viewModel.Instructor == null)
                {
                    return NotFound();
                }
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ModelViewInstructor instru)
        {
            if (instru != null)
            {
                try
                {
                    var instructor = await _instructorService.GetForDoc(instru.Instructor.NumeroDocumentoInstructor);
                    if (instructor == null)
                    {
                        return NotFound();
                    }
                    instructor.NumeroDocumentoInstructor = instru.Instructor.NumeroDocumentoInstructor;
                    instructor.NombreInstructor = instru.Instructor.NombreInstructor;
                    instructor.ApellidoInstructor = instru.Instructor.ApellidoInstructor;
                    instructor.CelInstructor = instru.Instructor.CelInstructor;
                    instructor.CorreoInstructor = instru.Instructor.CorreoInstructor;
                    instructor.IdTipodocumento = instru.Instructor.IdTipodocumento;

                    _dbSiscanContext.Instructors.Update(instructor);
                    await _dbSiscanContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrucExists(instru.Instructor.NumeroDocumentoInstructor)) return NotFound(); else throw;
                }
                return RedirectToAction(nameof(Consultar));
            }
            return View(instru);
        }
        private bool InstrucExists(string numeroDocumento)
        {
            return _dbSiscanContext.Instructors.Any(a => a.NumeroDocumentoInstructor == numeroDocumento);
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
                    seguimiento.ForEach(async s =>
                    {
                        SeguimientoInstructorAprendiz segui = await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(s.IdSeguimiento);
                        if (segui != null)
                        {
                            segui.NumeroDocumentoInstructor = null;
                            _dbSiscanContext.SeguimientoInstructorAprendizs.Update(segui);
                        }
                    });
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
                var asigFicha = await _dbSiscanContext.AsignacionFichas.Where(af => af.NumeroDocumentoInstructor == nmDoc).ToListAsync();
                if (asigFicha.Count > 0)
                {
                    _dbSiscanContext.AsignacionFichas.RemoveRange(asigFicha);
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
