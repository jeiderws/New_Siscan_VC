using Microsoft.AspNetCore.Mvc;
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
        public SeguimientoController(DbSiscanContext dbSiscanContext, ISeguimientoService seguimientoService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IQueryable<SeguimientoInstructorAprendiz> querysegui = await _seguimientoService.GetAll();
            List<ViewModelSeguimiento> listaseguimiento = querysegui.Select(a => new ViewModelSeguimiento(a)
            {
                NombreAprendiz = a.NumeroDocumentoAprendizNavigation.NombreAprendiz,
                ApellidoAprendiz = a.NumeroDocumentoAprendizNavigation.ApellidoAprendiz,
                NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                FichaAprendiz = a.NumeroDocumentoAprendizNavigation.Ficha.ToString(),
                NombreEmpresa = a.NitEmpresaNavigation.NombreEmpresa
                
            }).ToList();
            var vmSeguimiento = new Viewmodelsegui
            {
                listaSeguimiento=listaseguimiento,

            };
            return View(vmSeguimiento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task< IActionResult> Index(ViewModelSeguimiento Vmse)
        {
            ViewModelSeguimiento vm = null;
            try
            {
                if (Vmse != null)
                {
                    SeguimientoInstructorAprendiz segui = await _seguimientoService.GetForNumDocAprdz(Vmse.NumeroDocumentoAprendiz);
                    if (segui != null)
                    {
                        TempData["ValSeguiExiste"] = "Ya Existe Un Seguimiento Para Este Aprendiz"; 
                    }
                    else
                    {
                        var seguimiento = new SeguimientoInstructorAprendiz()
                        {
                            NumeroDocumentoAprendiz = Vmse.NumeroDocumentoAprendiz,
                            NumeroDocumentoInstructor = Vmse.NumeroDocumentoInstructor,
                            IdCoformador = Vmse.IdCoformador,
                            FechaInicio = Vmse.FechaInicio,
                            FechaFinalizacion = Vmse.FechaFinalizacion,
                            IdModalidad = Vmse.idmodalidad,
                            IdAsignacionArea = Vmse.IdAsignacionArea,   
                            IdAreaEmpresa = Vmse.IdAreaEmpresa,
                            NitEmpresa = Vmse.NitEmpresa,
                        };
                        await _seguimientoService.Insert(seguimiento);
                        ViewModelSeguimiento vmSeguimiento = new ViewModelSeguimiento(seguimiento);
                        vm = vmSeguimiento;
                        TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                    }
                }
                return View(vm);
            }
            catch (Exception)
            {

                throw;
            }
        }
     

        public async Task<IActionResult> consultar()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {

            var item = await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return View("Consultar", item);
            }


        }
    }
}
