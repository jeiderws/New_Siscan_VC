using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System.Net.WebSockets;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly ISeguimientoService _seguimientoService;
        private readonly IEmpresaService _empresaService;
        private readonly IAprendizService _aprendizService;
        private readonly IAsignacionService _asignacionService;
        private readonly IActividadService _actividadService;
        private readonly IObservacionesService _observacionesService;
        public SeguimientoController(DbSiscanContext dbSiscanContext, ISeguimientoService seguimientoService, IEmpresaService empresaService, IAprendizService aprendizService, IAsignacionService asignacionService, IActividadService actividadService, IObservacionesService observacionesService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _empresaService = empresaService;
            _aprendizService = aprendizService;
            _asignacionService = asignacionService;
            _actividadService = actividadService;
            _observacionesService = observacionesService;   
        }
        public async Task LlenarCombos()
        {
            var itemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;

            var itemsmodalidad = await _dbSiscanContext.Modalidads.ToListAsync();
            ViewBag.ItemsModalidad = itemsmodalidad;

            var itemsAreaempresa = await _dbSiscanContext.AreasEmpresas.ToListAsync();
            ViewBag.Itemsareaempresa = itemsAreaempresa;

            var itemsEmpresa = await _dbSiscanContext.Empresas.ToListAsync();
            ViewBag.Itemsempresa = itemsEmpresa;

            var itemsCooformador = await _dbSiscanContext.Coformadors.ToListAsync();
            ViewBag.Itemscooformador = itemsCooformador;

        }

        [HttpGet]
        public async Task<IActionResult> Index(string numdoc)
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();

            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();

            List<ViewModelAprendiz> listaAprendizSinSegui = new List<ViewModelAprendiz>();

            listaAprendices = queryAprendiz.Select(a => new ViewModelAprendiz(a)
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
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado,
                SeguimientoInstructorAprendices = a.SeguimientoInstructorAprendizs,
                NombreApellidoDoc = a.NombreAprendiz + " " + a.ApellidoAprendiz + " " + a.NumeroDocumentoAprendiz
            }).ToList();

            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count() == 0)
                {
                    listaAprendizSinSegui.Add(ap);
                }
            }
            var aprendi = await _aprendizService.GetForDoc(numdoc);

            var vmSeguimiento = new Viewmodelsegui
            {
                listaAprendizSinSegui = listaAprendizSinSegui,
                aprendiz = aprendi
            };
            return View(vmSeguimiento);
        }

        [HttpGet]
        public async Task<IActionResult> Crear(string nmDoc)
        {
            await LlenarCombos();
            var viewmodel = new Viewmodelsegui();
            if (nmDoc != null)
            {
                var segui = await _aprendizService.GetForDoc(nmDoc);
                viewmodel = new Viewmodelsegui
                {
                    aprendiz = segui
                };
                if (viewmodel.aprendiz == null)
                {
                    return NotFound();
                }
            }
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Viewmodelsegui Vmse)
        {
            await LlenarCombos();
            Viewmodelsegui viewmodelsegui = new Viewmodelsegui();
            try
            {
                if (Vmse != null)
                {
                    var seguimiento = new SeguimientoInstructorAprendiz()
                    {
                        NumeroDocumentoAprendiz = Vmse?.aprendiz?.NumeroDocumentoAprendiz,
                        NumeroDocumentoInstructor = Vmse?.seguimientoinstructorAprendiz?.NumeroDocumentoInstructor,
                        IdCoformador = Vmse?.seguimientoinstructorAprendiz?.IdCoformador,
                        FechaInicio = Vmse?.seguimientoinstructorAprendiz?.FechaInicio,
                        FechaFinalizacion = Vmse?.seguimientoinstructorAprendiz?.FechaFinalizacion,
                        IdModalidad = Vmse?.seguimientoinstructorAprendiz?.IdModalidad,
                        IdAreaEmpresa = Vmse?.seguimientoinstructorAprendiz?.IdAreaEmpresa,
                        NitEmpresa = Vmse?.seguimientoinstructorAprendiz.NitEmpresa
                    };

                    var empre = await _empresaService.GetForNit(seguimiento.NitEmpresa);
                    if (empre == null)
                    {
                        TempData["MensajeAlertEmpre"] = "Nit de Empresa no encontrado";
                    }
                    else
                    {
                        await _seguimientoService.Insert(seguimiento);

                        var asignacion = new AsignacionArea()
                        {
                            IdArea = seguimiento.IdAreaEmpresa,
                            NitEmpresa = seguimiento.NitEmpresa
                        };
                        await _asignacionService.Insert(asignacion);
                        seguimiento.IdAsignacionArea = asignacion.IdAsignacionArea;

                        if (Vmse.actividadesList != null)
                        {
                            foreach (var actividadDescripcion in Vmse.actividadesList)
                            {
                                var actividad = new Actividade()
                                {
                                    DescripcionActividad = actividadDescripcion,
                                    IdSeguimiento = seguimiento.IdSeguimiento
                                };
                                await _actividadService.Insert(actividad);
                            }
                        }

                        if (Vmse.observacionesList != null)
                        {
                            foreach (var observacion in Vmse.observacionesList)
                            {
                                var observa = new Observacion()
                                {
                                    Observaciones = observacion,
                                    IdSeguimiento = seguimiento.IdSeguimiento,
                                };
                                await _observacionesService.Insert(observa);
                            }
                        }

                        viewmodelsegui = new Viewmodelsegui()
                        {
                            seguimientoinstructorAprendiz = seguimiento,
                            asignacionArea = asignacion,
                        };

                        TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                    }
                }
                return View(viewmodelsegui);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
                return View(viewmodelsegui);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Consultar(string numDoc)
        {
            //obtener todos los aprendices de la bd
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();
            List<ViewModelAprendiz> listaAprendizSegui = new List<ViewModelAprendiz>();
            List<SeguimientoInstructorAprendiz> listSeguimiento = new List<SeguimientoInstructorAprendiz>();
            Aprendiz aprendiz = null;
            var querySeguimiento = await _seguimientoService.GetAll();

            //obtener lista de aprendices
            listaAprendices = queryAprendiz.Select(a => new ViewModelAprendiz(a)
            {
                nombredoc = a.IdTipodocumentoNavigation.TipoDocumento1,
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
                Ficha = a.Ficha,
                IdCiudad = a.IdCiudad,
                IdEstadoAprendiz = a.IdEstadoAprendiz,
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado,
                SeguimientoInstructorAprendices = a.SeguimientoInstructorAprendizs,
                NombreApellidoDoc = a.NombreAprendiz + " " + a.ApellidoAprendiz + " " + a.NumeroDocumentoAprendiz,
            }).ToList();

            var seguimiento = await _seguimientoService.GetForNumDocAprdz(numDoc);
            foreach(var segui in querySeguimiento)
            {
                listSeguimiento.Add(segui);
            }
            aprendiz = await _aprendizService.GetForDoc(numDoc);
            Empresa empresa = new Empresa();
            if (seguimiento != null)
            {
                empresa = await _dbSiscanContext.Empresas.FindAsync(seguimiento.NitEmpresa);
            }
            var vmSeguimiento = new Viewmodelsegui
            {
                listaSeguimientos=listSeguimiento,
                aprendiz = aprendiz,
                seguimientoinstructorAprendiz = seguimiento,
                Empresa = empresa
            };
            return View(vmSeguimiento);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(long idSeguimiento)
        {
            try
            {
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.FirstOrDefaultAsync(s=>s.IdSeguimiento==idSeguimiento);
                if (seguimiento == null)
                {
                    return Json(new { success = false, message = "El seguimiento no fue encontrado." });
                } 
                var actividades = await  _dbSiscanContext.Actividades.Where(a => a.IdSeguimiento == idSeguimiento).ToListAsync();   
                _dbSiscanContext.Actividades.RemoveRange(actividades);
                var observaciones = await _dbSiscanContext.Observacions.Where(o=> o.IdSeguimiento == idSeguimiento).ToListAsync();
                _dbSiscanContext.Observacions.RemoveRange(observaciones);
                await _seguimientoService.Delete(idSeguimiento);
                TempData["MensajeSeguimientoEliminado"] = "Seguimiento eliminado correctamente!!";

                
                return Json(new { success = true, message = "El seguimiento se elimino correctamente." });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Se produjo un error al intentas eliminar el seguimiento: " + ex.Message });
            }
        }
    }
}
