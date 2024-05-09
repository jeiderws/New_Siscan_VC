using Microsoft.AspNetCore.Mvc;
using Siscan_Vc_AppWeb.Models;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System.Diagnostics;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAprendizService _aprendizService;

        public HomeController(IAprendizService aprendizService)
        {
            _aprendizService = aprendizService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendiz = queryAprendiz
                                                  .Select(a => new ViewModelAprendiz(a)
                                                  {
                                                     
                                                      NombreAprendiz = a.NombreAprendiz,
                                                      ApellidoAprendiz = a.ApellidoAprendiz,
                                                      NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                                                      Ficha = a.Ficha,
                                                      nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado
                                                  }
                                                  ).ToList();

            //var aprendiz = _dbSiscanContext.Aprendiz;
            return View(listaAprendiz);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
