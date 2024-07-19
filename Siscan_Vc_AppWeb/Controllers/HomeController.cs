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
        private readonly ISeguimientoService _seguimientoService;

        public HomeController(IAprendizService aprendizService, ISeguimientoService seguimientoService)
        {
            _aprendizService = aprendizService;
            _seguimientoService = seguimientoService;

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
                                                  }).ToList();
            int con = 0;
            int pas = 0;
            int pro = 0;
            var Consulta = _seguimientoService.GetAll();
            foreach (var item in Consulta.Result)
            {
                if (item.IdModalidad == 1)
                {
                    con++;
                }
                if (item.IdModalidad == 2)
                {
                    pas++;
                }
                if (item.IdModalidad == 3)
                {
                    pro++;
                }
            }
            TempData["consulta"]= con;
            TempData["consulta2"]= pas;
            TempData["consulta3"]= pro;

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
