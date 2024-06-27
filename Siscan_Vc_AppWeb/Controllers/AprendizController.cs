using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System.Data;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class AprendizController : Controller
    {
        private readonly IAprendizService _aprendizService;

        private readonly IInscripcionTYTService _inscripcionTYTService;

        private readonly DbSiscanContext _dbSiscanContext;
              
        public AprendizController(IAprendizService aprendizService, DbSiscanContext dbSiscanContext, IInscripcionTYTService inscripcionTYTService)
        {
            _dbSiscanContext = dbSiscanContext;
            _aprendizService = aprendizService;
            _inscripcionTYTService = inscripcionTYTService;
        }

        [HttpGet]
        public IActionResult ObtenerNivelPrograma(string programaId)
        {
            var programa = _dbSiscanContext.Programas.FirstOrDefault(p => p.CodigoPrograma == programaId);
            if (programa != null)
            {
                var nivelPrograma = programa.IdNivelPrograma;
                return Json(new { nivelPrograma });
            }
            else
            {
                return Json(new { nivelPrograma = "" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> CargarCiudades(int departamentoId)
        {
            var ciudades = await _dbSiscanContext.Ciudads.Where(c => c.IdDepartamento == departamentoId).ToListAsync();
            ViewBag.ciudades = ciudades;
            return Json(ciudades);
        }
        [HttpGet]
        public async Task<IActionResult> Cargarprograma(int programa)
        {
            var ficha = await _dbSiscanContext.Fichas.Where(f => f.CodigoPrograma == programa.ToString()).ToListAsync();
            ViewBag.ficha = ficha;
            return Json(ficha);
        }

        public async Task LlenarCombos()
        {
            var itemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;

            var itemsEstAprndz = await _dbSiscanContext.EstadoAprendizs.ToListAsync();
            ViewBag.ItemsEstAprndz = itemsEstAprndz;

            var itemsDepartamento = await _dbSiscanContext.Departamentos.ToListAsync();
            ViewBag.ItemsDepartamento = itemsDepartamento;

            ViewBag.ciudades = new List<Ciudad>();

            var itemsEstaTYT = await _dbSiscanContext.EstadoInscripcionTyts.ToListAsync();
            ViewBag.ItemsEstaTYT = itemsEstaTYT;

            var itemsPrograma = await _dbSiscanContext.Programas.ToListAsync();
            ViewBag.ItemsPrograma = itemsPrograma;

            ViewBag.ficha = new List<Ficha>();

            var itemConvocatoria = await _dbSiscanContext.ConvocatoriaTyts.ToListAsync();
            ViewBag.ItemsConvocatoria = itemConvocatoria;

        }
        //Llenar combos
        public async Task<IActionResult> Registro()
        {
            var modelview = new Modelviewtytap
            {
                //lista para el combo tipo de documento
                listaOpcTpDoc = _dbSiscanContext.TipoDocumentos.Select(o => new SelectListItem
                {
                    Value = o.IdTipoDocumento.ToString(),
                    Text = o.TipoDocumento1
                }).ToList(),
                //lista para el combo estado aprendiz
                listaOpcEstado = _dbSiscanContext.EstadoAprendizs.Select(e => new SelectListItem
                {
                    Value = e.IdEstado.ToString(),
                    Text = e.NombreEstado
                }).ToList(),                
                //lista para el combo departamentos
                listaOpcDepartamento = _dbSiscanContext.Departamentos.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.NombreDepartamento
                }).ToList(),               
                //lista para el combo ciudad
                listaOpcCiudad = _dbSiscanContext.Ciudads.Select(c => new SelectListItem
                {
                    Value= c.IdCiudad.ToString(),
                    Text= c.NombreCiudad
                }).ToList(),
                //lista para el combo estado tyt
                listaOpcEstadoTyt = _dbSiscanContext.EstadoInscripcionTyts.Select(e =>new SelectListItem
                {
                    Value=e.IdEstadotyt.ToString(),
                    Text=e.DescripcionEstadotyt
                }).ToList(),
                //lista para el combo programas
                listaOpcPrograma = _dbSiscanContext.Programas.Select(p=>new SelectListItem
                {
                    Value=p.CodigoPrograma.ToString(),
                    Text=p.NombrePrograma
                }).ToList(),
                //lista para el combo ficha
                listaOpcFicha = _dbSiscanContext.Fichas.Select(f=> new SelectListItem
                {
                    Value=f.Ficha1.ToString(),
                    Text=f.Ficha1.ToString()
                }).ToList(),
                //lista para el combo convocatoria tyt
                listaOpcConvocatoria = _dbSiscanContext.ConvocatoriaTyts.Select(c=>new SelectListItem
                {
                    Value=c.IdConvocatoria.ToString(),
                    Text=c.SemestreConvocatoria
                }).ToList()

            };
            await LlenarCombos();
            return View(modelview);
        }
        public async Task<IActionResult> RegistrarLotes()
        {
            return View();
        }

        //Registrar aprendiz con un view model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Modelviewtytap aptyt)
        {
            Modelviewtytap vmtytap = new Modelviewtytap();
            try
            {
                if (aptyt != null)
                {
                    Aprendiz apren = await _aprendizService.GetForDoc(aptyt.aprendiz.NumeroDocumentoAprendiz);
                    if (apren != null)
                    {
                        TempData["ValAprendzExiste"] = "Ya existe un aprendiz con este numero de documento";
                       return RedirectToAction(nameof(Registro));
                    }
                    else if (apren == null)
                    {
                        var aprendiz = new Aprendiz()
                        {
                            IdTipodocumento = aptyt.OpcSeleccionadoTpDoc,
                            NumeroDocumentoAprendiz = aptyt.aprendiz.NumeroDocumentoAprendiz,
                            NombreAprendiz = aptyt.aprendiz.NombreAprendiz,
                            ApellidoAprendiz = aptyt.aprendiz.ApellidoAprendiz,
                            CelAprendiz = aptyt.aprendiz.CelAprendiz,
                            DireccionAprendiz = aptyt.aprendiz.DireccionAprendiz,
                            CorreoAprendiz = aptyt.aprendiz.CorreoAprendiz,
                            IdEstadoAprendiz = aptyt.OpcSeleccionadoEstado,
                            IdCiudad = aptyt.OpcSeleccionadoCiudad,
                            Ficha = aptyt.OpcSeleccionadoFicha,
                            NombreCompletoAcudiente = aptyt.aprendiz.NombreCompletoAcudiente,
                            CelularAcudiente = aptyt.aprendiz.CelularAcudiente,
                            CorreoAcuediente = aptyt.aprendiz.CorreoAcuediente
                        };
                        if (aprendiz.IdEstadoAprendiz == 4 && aprendiz.IdEstadoTyt == null)
                        {
                            aprendiz.IdEstadoTyt = 6;
                        }
                        else
                        {

                            aprendiz.IdEstadoTyt = aptyt.aprendiz.IdEstadoTyt;
                        }
                        await _aprendizService.Insert(aprendiz);

                        if (aprendiz.NumeroDocumentoAprendiz == aptyt.aprendiz.NumeroDocumentoAprendiz && aprendiz.IdEstadoTyt == 1)
                        {
                            var tyt = new InscripcionTyt()
                            {
                                CodigoInscripcion = aptyt.inscripcionTyt.CodigoInscripcion,
                                NumeroDocumentoAprendiz = aprendiz.NumeroDocumentoAprendiz,
                                Idciudad = aptyt.OpcSeleccionadoCiudadTyt,
                                IdConvocatoria = aptyt.OpcSeleccionadoConvocatoria,
                                IdEstadotyt = aprendiz.IdEstadoTyt
                            };
                            _dbSiscanContext.InscripcionTyts.Add(tyt);
                            _dbSiscanContext.SaveChanges();
                        }

                        vmtytap = new Modelviewtytap
                        {
                            aprendiz = aptyt.aprendiz,
                            inscripcionTyt = aptyt.inscripcionTyt
                        };
                        if (vmtytap.aprendiz.NumeroDocumentoAprendiz != null)
                        {
                            TempData["MensajeAlert"] = "Aprendiz Guardado Correctamente";
                        }
                        return RedirectToAction(nameof(Registro));

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["registroAprendizExcepcion"] = ex.Message;
            }
            return View(aptyt);
        }

        //consultar aprendiz por numero de documento y obtener la lista de aprendices
        [HttpGet]
        public async Task<IActionResult> Consultar(string nmdoc)
        {
            List<ViewModelAprendiz> listaAprendiz = new List<ViewModelAprendiz>();
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();

            listaAprendiz = queryAprendiz.Select(a => new ViewModelAprendiz(a)
            {
                NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                NombreAprendiz = a.NombreAprendiz,
                ApellidoAprendiz = a.ApellidoAprendiz,
                CelAprendiz = a.CelAprendiz,
                CorreoAprendiz = a.CorreoAprendiz,
                DireccionAprendiz = a.DireccionAprendiz,
                NombreCompletoAcudiente = a.NombreCompletoAcudiente,
                CorreoAcuediente = a.CorreoAcuediente,
                CelularAcudiente = a.CelularAcudiente,
                IdEstadoTyt = a.IdEstadoTytNavigation.IdEstadotyt,
                nomEstadoTyt = a.IdEstadoTytNavigation.DescripcionEstadotyt,
                IdTipodocumento = a.IdTipodocumentoNavigation.IdTipoDocumento,
                nombredoc = a.IdTipodocumentoNavigation.TipoDocumento1,
                Ficha = a.Ficha,
                IdCiudad = a.IdCiudad,
                IdEstadoAprendiz = a.IdEstadoAprendiz,
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado
            }).ToList();

            Aprendiz aprendiz = new Aprendiz();
            foreach (var aprendi in queryAprendiz)
            {
                if (aprendi.NumeroDocumentoAprendiz == nmdoc)
                {
                    aprendiz = aprendi;   
                    break;
                }
            }

            ModelviewAp viewModel = new ModelviewAp
            {

                Aprendiz = aprendiz,
                ListaAprendices = listaAprendiz
            };

            TempData["aprendizConsultAlert"] = "No hay Resultados";
            return View(viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string nmdoc)
        {
            try
            {
                var aprendiz = await _dbSiscanContext.Aprendiz.FirstOrDefaultAsync(x => x.NumeroDocumentoAprendiz == nmdoc);

                if (aprendiz == null)
                {
                    return Json(new { success = false, message = "El aprendiz no fue encontrado." });
                }
                TempData["MensajeAlertEliminado"] = "Aprendiz eliminado correctamente!!";
                var inscripciones = await _dbSiscanContext.InscripcionTyts.Where(i => i.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.InscripcionTyts.RemoveRange(inscripciones);
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.Where(i => i.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.SeguimientoInstructorAprendizs.RemoveRange(seguimiento);
                await _aprendizService.Delete(nmdoc);
                return Json(new { success = true, message = "El aprendiz se eliminó correctamente." });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Se produjo un error al intentar eliminar el aprendiz: " + e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string numDoc)
        {
            await LlenarCombos();
            var viewModel = new Modelviewtytap();
            if (numDoc != null)
            {
                var aprendi = await _aprendizService.GetForDoc(numDoc);
                InscripcionTyt insctyt;

                if (aprendi.IdEstadoTyt == 1)
                {
                    insctyt = _dbSiscanContext.InscripcionTyts.First(i => i.NumeroDocumentoAprendiz == aprendi.NumeroDocumentoAprendiz);
                }
                else
                {
                    insctyt = null;
                }

                viewModel = new Modelviewtytap
                {
                    aprendiz = aprendi,
                    inscripcionTyt = insctyt
                }; 

                if (viewModel.aprendiz == null)
                {
                    return NotFound();
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Modelviewtytap aprendiztyt)
        {
            InscripcionTyt insctyt;
            if (aprendiztyt != null)
            {
                var aprendiz = await _aprendizService.GetForDoc(aprendiztyt.aprendiz.NumeroDocumentoAprendiz);
                if (aprendiz == null)    
                {
                    return NotFound();
                }

                //asignar los datos actualizados a aprendiz
                aprendiz.IdTipodocumento = aprendiztyt.aprendiz.IdTipodocumento;
                aprendiz.NombreAprendiz = aprendiztyt.aprendiz.NombreAprendiz;
                aprendiz.ApellidoAprendiz = aprendiztyt.aprendiz.ApellidoAprendiz;
                aprendiz.CelAprendiz = aprendiztyt.aprendiz.CelAprendiz;
                aprendiz.CorreoAprendiz = aprendiztyt.aprendiz.CorreoAprendiz;
                aprendiz.DireccionAprendiz = aprendiztyt.aprendiz.DireccionAprendiz;
                aprendiz.NombreCompletoAcudiente = aprendiztyt.aprendiz.NombreCompletoAcudiente;
                aprendiz.CorreoAcuediente = aprendiztyt.aprendiz.CorreoAcuediente;
                aprendiz.CorreoAcuediente = aprendiztyt.aprendiz.CorreoAcuediente;
                aprendiz.CelularAcudiente = aprendiztyt.aprendiz.CelularAcudiente;
                aprendiz.IdEstadoAprendiz = aprendiztyt.aprendiz.IdEstadoAprendiz;
                aprendiz.Ficha = aprendiztyt.aprendiz.Ficha;
                aprendiz.IdCiudad = aprendiztyt.aprendiz.IdCiudad;
                aprendiz.IdEstadoAprendiz = aprendiztyt.aprendiz.IdEstadoAprendiz;

                try
                {

                    _dbSiscanContext.Aprendiz.Update(aprendiz);
                    if (aprendiz.IdEstadoTyt == 1)
                    {
                        insctyt = await _dbSiscanContext.InscripcionTyts.FirstOrDefaultAsync(i => i.NumeroDocumentoAprendiz == aprendiz.NumeroDocumentoAprendiz);

                        insctyt.CodigoInscripcion = aprendiztyt.inscripcionTyt.CodigoInscripcion;
                        insctyt.Idciudad = aprendiztyt.inscripcionTyt.Idciudad;
                        insctyt.NumeroDocumentoAprendiz = aprendiztyt.aprendiz.NumeroDocumentoAprendiz;
                        insctyt.IdConvocatoria = aprendiztyt.inscripcionTyt.IdConvocatoria;
                        insctyt.IdEstadotyt = aprendiztyt.aprendiz.IdEstadoTyt;

                        _dbSiscanContext.InscripcionTyts.Update(insctyt);
                    }
                    await _dbSiscanContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //validacion de existencia del aprendiz
                    if (!AprendizExists(aprendiztyt.aprendiz.NumeroDocumentoAprendiz))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Consultar));
            }
            return View(aprendiztyt);
        }
        private bool AprendizExists(string numeroDocumento)
        {
            return _dbSiscanContext.Aprendiz.Any(a => a.NumeroDocumentoAprendiz == numeroDocumento);
        }

    }
}
