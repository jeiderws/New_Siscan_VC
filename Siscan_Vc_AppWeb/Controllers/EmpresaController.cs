using Microsoft.AspNetCore.Mvc;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public EmpresaController(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult consultar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerNombresEmpresa(string term)
        {
            var nombreEmpresa = _dbSiscanContext.Empresas
                .Where(e => e.NombreEmpresa.Contains(term))
                .Select(e => e.NombreEmpresa)
                .ToList();

            return Json(nombreEmpresa);
        }
    }
}
