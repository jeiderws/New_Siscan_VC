using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly IEmpresaService _empresaService;
        private readonly ISeguimientoService _seguimientoService;
        public EmpresaController(DbSiscanContext dbSiscanContext, IEmpresaService empresaService, ISeguimientoService seguimientoService)
        {
            _empresaService = empresaService;
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
        }


        public async Task<IActionResult> Registro()
        {
            var modelView = new ModelViewEmpresa
            {
                //lista departamentos
                listaOpcDepartamento = _dbSiscanContext.Departamentos.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.NombreDepartamento
                }).ToList(),
                //lista ciudades
                listaOpcCiudad = _dbSiscanContext.Ciudads.Select(d => new SelectListItem
                {
                    Value = d.IdCiudad.ToString(),
                    Text = d.NombreCiudad
                }).ToList()
            };
            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(ModelViewEmpresa empresa)
        {
            ModelViewEmpresa mVEmpresa = new ModelViewEmpresa();
            try
            {
                if (empresa != null)
                {
                    var empreExist = await _empresaService.GetForNit(empresa.empresa.Nitmpresa);
                    if (empreExist != null)
                    {
                        TempData["ValEmpresaExist"] = "Ya existe una empresa registrada con este NIT";
                        return RedirectToAction(nameof(Registro));

                    }
                    else if (empreExist == null)
                    {
                        var empre = new Empresa()
                        {
                            Nitmpresa = empresa.empresa.Nitmpresa,
                            NombreEmpresa = empresa.empresa.NombreEmpresa,
                            RepresentanteLegal = empresa.empresa.RepresentanteLegal,
                            DireccionEmpresa = empresa.empresa.DireccionEmpresa,
                            TelefonoEmpresa = empresa.empresa.TelefonoEmpresa,
                            IdCiudad = empresa.opcSeleccionadaCiudad
                        };
                        mVEmpresa.empresa = empre;
                        if (mVEmpresa.empresa.Nitmpresa != null)
                        {
                            _dbSiscanContext.Empresas.Add(empresa.empresa);
                            await _dbSiscanContext.SaveChangesAsync();
                            TempData["RegistroEmpresaExitoso"] = "Empresa registrada exitosamente";
                        }
                        return RedirectToAction(nameof(Registro));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["registroEmpresaExcepcion"] = ex.Message;
            }
            return View(mVEmpresa);
        }

        [HttpGet]
        public async Task<IActionResult> consultar(string nitEmpresa)
        {
            List<Aprendiz> listAprendiz=new List<Aprendiz>();
            List<Coformador> listCoformador=new List<Coformador>();
            VMEmpresaAprendizCoformador vmEmpresa = new VMEmpresaAprendizCoformador();
            var listSeguimiento = await _seguimientoService.GetAll();
            try
            {
                var empresa = await _empresaService.GetForNit(nitEmpresa);
                if (empresa == null)
                {
                    TempData["EmpresaNoExiste"] = "No se encontro una empresa con este Nit";
                }
                vmEmpresa = new VMEmpresaAprendizCoformador
                {
                    empresa=empresa,
                };
            }
            catch (Exception ex)
            {
                TempData["EmpresaExcepcionConsulta"] = "Error: " + ex.Message;
            }

            return View(vmEmpresa);
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
