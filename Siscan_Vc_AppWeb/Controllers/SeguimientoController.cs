using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
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
        public SeguimientoController(DbSiscanContext dbSiscanContext, ISeguimientoService seguimientoService, IEmpresaService empresaService, IAprendizService aprendizService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _empresaService = empresaService;
            _aprendizService = aprendizService;
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
                SeguimientoInstructorAprendices=a.SeguimientoInstructorAprendizs
            }).ToList();
            var aprendi =await _aprendizService.GetForDoc(numdoc);

            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count()==0)
                {
                    listaAprendizSinSegui.Add(ap);
                }
            }

            IQueryable<Empresa> queryem = await _empresaService.GetAll();
            List<Empresa> listaempresa = queryem.Select(x => new Empresa()
            {
                NombreEmpresa = x.NombreEmpresa
            }).ToList();

            var vmSeguimiento = new Viewmodelsegui
            {
                listaEmpresa = listaempresa,
                listaAprendizSinSegui = listaAprendizSinSegui
                listaAprendizSinSegui = listaAprendizSinSegui,
                aprendiz = aprendi
            };
            return View(vmSeguimiento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Viewmodelsegui Vmse)
        {
            await LlenarCombos();
            Viewmodelsegui viewmodelsegui = new Viewmodelsegui();
            try
            {
                if (Vmse != null)
                {
                    var seguimiento = new SeguimientoInstructorAprendiz()
                    {
                        NumeroDocumentoAprendiz = Vmse.seguimientoinstructorAprendiz.NumeroDocumentoAprendiz,
                        NumeroDocumentoInstructor = Vmse.seguimientoinstructorAprendiz.NumeroDocumentoInstructor,
                        IdCoformador = Vmse.seguimientoinstructorAprendiz.IdCoformador,
                        FechaInicio = Vmse.seguimientoinstructorAprendiz.FechaInicio,
                        FechaFinalizacion = Vmse.seguimientoinstructorAprendiz.FechaFinalizacion,
                        IdModalidad = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        IdAsignacionArea = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        IdAreaEmpresa = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        NitEmpresa = Vmse.opcseleccionadaEmpre
                    };
                    await _seguimientoService.Insert(seguimiento);
                    var asignacion = new AsignacionArea()
                    {
                        IdArea = Vmse.seguimientoinstructorAprendiz.IdAreaEmpresa,
                        NitEmpresa = Vmse.seguimientoinstructorAprendiz.NitEmpresa
                    };
                    viewmodelsegui = new Viewmodelsegui()
                    {                
                        NumeroDocumentoAprendiz = Vmse.seguimientoinstructorAprendiz.NumeroDocumentoAprendiz,
                        NumeroDocumentoInstructor = Vmse.seguimientoinstructorAprendiz.NumeroDocumentoInstructor,
                        IdCoformador = Vmse.seguimientoinstructorAprendiz.IdCoformador,
                        FechaInicio = Vmse.seguimientoinstructorAprendiz.FechaInicio,
                        FechaFinalizacion = Vmse.seguimientoinstructorAprendiz.FechaFinalizacion,
                        IdModalidad = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        IdAsignacionArea = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        IdAreaEmpresa = Vmse.seguimientoinstructorAprendiz.IdModalidad,
                        NitEmpresa = Vmse.opcseleccionadaEmpre
                    };
                    await _seguimientoService.Insert(seguimiento);
                    var asignacion = new AsignacionArea()
                    {
                        IdArea = Vmse.seguimientoinstructorAprendiz.IdAreaEmpresa,
                        NitEmpresa = Vmse.seguimientoinstructorAprendiz.NitEmpresa
                    };
                    viewmodelsegui = new Viewmodelsegui()
                    {
                        seguimientoinstructorAprendiz = seguimiento,
                        asignacionArea = asignacion
                    };
                    TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                    return RedirectToAction(nameof(Index));
                }
                return View(viewmodelsegui);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> consultar(string nmdoc)
        {
            var num = numDoc;
            //obtener todos los aprendices de la bd
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();
            List<ViewModelAprendiz> listaAprendizSegui = new List<ViewModelAprendiz>();
            ViewModelAprendiz aprendiz = null;

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
            foreach (var apren in listaAprendizSegui)
            {
                if (apren.NumeroDocumentoAprendiz == numDoc)
                {
                    aprendiz = apren;
                }
            }
            Empresa empresa=new Empresa();
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
