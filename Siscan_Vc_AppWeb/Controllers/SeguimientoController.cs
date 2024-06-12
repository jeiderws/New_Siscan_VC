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
using System.Security.Policy;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly ISeguimientoService _seguimientoService;
        private readonly IEmpresaService _empresaService;
        private readonly IAprendizService _aprendizService;
        private readonly IAsignacionService _asignacionService;
        public SeguimientoController(DbSiscanContext dbSiscanContext, ISeguimientoService seguimientoService, IEmpresaService empresaService, IAprendizService aprendizService, IAsignacionService asignacionService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _empresaService = empresaService;
            _aprendizService = aprendizService;
            _asignacionService = asignacionService;
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

            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count() == 0)
                {
                    listaAprendizSinSegui.Add(ap);
                }
            }

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
                    var asignacion = new AsignacionArea()
                    {
                        IdArea = seguimiento.IdAreaEmpresa,
                        NitEmpresa = seguimiento.NitEmpresa
                    };
                    var empre = await _empresaService.GetForNit(seguimiento.NitEmpresa);
                    if (empre == null)
                    {
                        TempData["MensajeAlertEmpre"] = " Nit de Empresa no encontrado";
                    }
                    else
                    {
                        await _asignacionService.Insert(asignacion);
                        seguimiento.IdAsignacionArea = asignacion.IdAsignacionArea;
                        await _seguimientoService.Insert(seguimiento);
                        viewmodelsegui = new Viewmodelsegui()
                        {
                            seguimientoinstructorAprendiz = seguimiento,
                            asignacionArea = asignacion
                        };
                        TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                        return View(viewmodelsegui);
                    }
                }
              
                return View(viewmodelsegui);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Consultar(string numDoc)
        {
            //obtener todos los aprendices de la bd
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();
            List<ViewModelAprendiz> listaAprendizSegui = new List<ViewModelAprendiz>();
            Aprendiz aprendiz = null;

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
                //Programa=a.FichaNavigation.ProgramaNavigation.NombrePrograma
            }).ToList();
            var seguimiento = await _seguimientoService.GetForNumDocAprdz(numDoc);
            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count() != 0)
                {
                    listaAprendizSegui.Add(ap);
                }
            }
            aprendiz = await _aprendizService.GetForDoc(numDoc);
            Empresa empresa = new Empresa();
            if (seguimiento != null)
            {
                empresa = await _dbSiscanContext.Empresas.FindAsync(seguimiento.NitEmpresa);
            }
            var vmSeguimiento = new Viewmodelsegui
            {
                listaAprendizSegui = listaAprendizSegui,
                aprendiz = aprendiz,
                seguimientoinstructorAprendiz = seguimiento,
                Empresa = empresa
            };
            return View(vmSeguimiento);
        }
    }
}
