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
        private readonly IAprendizService _aprendizService;
        //private readonly DbSiscanContext _dbSiscanContext;
        public AprendizController(IAprendizService aprendizService)
        {
            //_dbSiscanContext = dbSiscanContext;

            _aprendizService = aprendizService;

        }
        public IActionResult Registro()
        {
            return View();
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
                                                      IdEstadoTyt = a.IdEstadoTyt,
                                                      IdTipodocumento = a.IdTipodocumentoNavigation.IdTipoDocumento,
                                                      nombredoc = a.IdTipodocumentoNavigation.TipoDocumento1,
                                                      Ficha = a.Ficha,
                                                      IdCiudad = a.IdCiudad,
                                                      IdEstadoAprendiz = a.IdEstadoAprendiz
                                                    
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
