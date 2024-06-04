using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

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
                SeguimientoInstructorAprendices=a.SeguimientoInstructorAprendizs
            }).ToList();

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


        //public async Task<IActionResult> consultar(string nmdoc)
        //{
        //    List<Viewmodelsegui> listasegui = new List<Viewmodelsegui>();
        //    IQueryable<SeguimientoInstructorAprendiz> querysegui = await _seguimientoService.GetAll();
        //    listasegui = querysegui.Select(a => new Viewmodelsegui()
        //    {
        //        seguimientoinstructorAprendiz = a.NumeroDocumentoAprendiz,
        //        nume
        //    }).ToList();
        //}
    }
}
