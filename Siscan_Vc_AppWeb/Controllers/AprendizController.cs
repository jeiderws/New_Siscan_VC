using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class AprendizController : Controller
    {
        private readonly AprendizService _aprendizService;
        private readonly DbSiscanContext _dbSiscanContext;
        public AprendizController(AprendizService aprendizService, DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
            _aprendizService = aprendizService;

        }
        
        public async Task<IActionResult> Registro()
        {
            var itemsTipoDoc= await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;
            
            var itemsEstAprndz = await _dbSiscanContext.EstadoAprendizs.ToListAsync();
            ViewBag.ItemsEstAprndz = itemsEstAprndz; 
            var itemsDepartamento = await _dbSiscanContext.Departamentos.ToListAsync();
            ViewBag.ItemsDepartamento = itemsDepartamento;
            var itemsCiudad = await _dbSiscanContext.Ciudads.ToListAsync();
            ViewBag.ItemsCiudad = itemsCiudad;
            var itemsEstaTYT = await _dbSiscanContext.EstadoInscripcionTyts.ToListAsync();
            ViewBag.ItemsEstaTYT = itemsEstaTYT;
            var itemsPrograma = await _dbSiscanContext.Programas.ToListAsync();
            ViewBag.ItemsPrograma = itemsPrograma;
            var itemsFichas = await _dbSiscanContext.Fichas.ToListAsync();
            ViewBag.ItemsFichas = itemsFichas;
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Aprendiz ap)
        {
            if (ModelState.IsValid)
            {
                var aprendiz = new Aprendiz()
                {
                    IdTipodocumento = ap.IdTipodocumento,
                    NumeroDocumentoAprendiz = ap.NumeroDocumentoAprendiz,
                    NombreAprendiz = ap.NombreAprendiz,
                    ApellidoAprendiz = ap.ApellidoAprendiz,
                    CelAprendiz = ap.CelAprendiz,

                    DireccionAprendiz = ap.DireccionAprendiz,
                    CorreoAprendiz = ap.CorreoAprendiz,
                    IdEstadoAprendiz = ap.IdEstadoAprendiz,
                    IdCiudad = ap.IdCiudad,
                    IdEstadoTyt = ap.IdEstadoTyt,
                    Ficha = ap.Ficha,

                    NombreCompletoAcudiente = ap.NombreCompletoAcudiente,

                    CelularAcudiente = ap.CelularAcudiente,
                    CorreoAcuediente = ap.CorreoAcuediente
                };
                _dbSiscanContext.Aprendiz.Add(aprendiz);
                _dbSiscanContext.SaveChanges();
                return RedirectToAction(nameof(Registro));
            }
        
            
              return View(ap);
        }

        [HttpGet]
        public async Task<IActionResult> Consultar()
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendiz = queryAprendiz
                                                  .Select(a => new ViewModelAprendiz(a)
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

                                                  }
                                                  ).ToList();

            //var aprendiz = _dbSiscanContext.Aprendiz;
            return View(listaAprendiz);
        }


        
        public IActionResult Editar()
        {
            return View();
        }
    
    }
}
