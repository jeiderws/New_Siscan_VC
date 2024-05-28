using Microsoft.AspNetCore.Mvc;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class ProgramasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
