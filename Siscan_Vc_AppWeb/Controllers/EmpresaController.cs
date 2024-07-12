using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICoformadorService _coformadorService;
        private readonly IAprendizService _aprendizService;
        public EmpresaController(DbSiscanContext dbSiscanContext, IEmpresaService empresaService, ISeguimientoService seguimientoService, ICoformadorService coformadorService, IAprendizService aprendizService)
        {
            _empresaService = empresaService;
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _coformadorService = coformadorService;
            _aprendizService = aprendizService;
        }

        [HttpGet]
        public async Task<IActionResult> CargarCiudades(int departamentoId)
        {
            var ciudades = await _dbSiscanContext.Ciudads.Where(c => c.IdDepartamento == departamentoId).ToListAsync();
            ViewBag.ciudades=ciudades;
            return Json(ciudades);
        }

        [HttpGet]
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
        public async Task<IActionResult> Registro(ModelViewEmpresa empresaMv)
        {
            ModelViewEmpresa mVEmpresa = new ModelViewEmpresa();
            try 
            {
                if (empresaMv.empresa != null)
                {
                    var empreExist = await _empresaService.GetForNit(empresaMv.empresa.Nitmpresa);
                    if (empreExist != null)
                    {
                        TempData["ValEmpresaExist"] = "Ya existe una empresa registrada con este NIT";
                        return RedirectToAction(nameof(Registro));

                    }
                    else if (empreExist == null)
                    {
                        var empre = new Empresa()
                        {
                            Nitmpresa = empresaMv.empresa.Nitmpresa,
                            NombreEmpresa = empresaMv.empresa.NombreEmpresa,
                            RepresentanteLegal = empresaMv.empresa.RepresentanteLegal,
                            DireccionEmpresa = empresaMv.empresa.DireccionEmpresa,
                            TelefonoEmpresa = empresaMv.empresa.TelefonoEmpresa,
                            IdCiudad = empresaMv.empresa.IdCiudad
                        };
                        mVEmpresa.empresa = empre;
                        if (mVEmpresa.empresa.Nitmpresa != null)
                        {
                            _dbSiscanContext.Empresas.Add(empresaMv.empresa);
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
            var queryCoformador = await _coformadorService.GetAll();
            var listSeguimiento = await _seguimientoService.GetAll();
            List<Aprendiz> listAprendiz=new List<Aprendiz>();
            List<Coformador> listCoformador=new List<Coformador>();
            VMEmpresaAprendizCoformador vmEmpresa = new VMEmpresaAprendizCoformador();

            try
            {
                var empresa = await _empresaService.GetForNit(nitEmpresa);
                if (empresa == null)
                {
                    TempData["EmpresaNoExiste"] = "No se encontro una empresa con este Nit";
                }
                foreach(var coformador in queryCoformador)
                {
                    if(coformador.NitEmpresa == nitEmpresa)
                    {
                        listCoformador.Add(coformador);
                    }
                }
                foreach(var seguimiento in listSeguimiento)
                {
                    if (seguimiento.NitEmpresa == nitEmpresa)
                    {
                        var aprendiz= await _aprendizService.GetForDoc(seguimiento.NumeroDocumentoAprendiz);
                        listAprendiz.Add(aprendiz);
                    }
                }
                vmEmpresa = new VMEmpresaAprendizCoformador
                {
                    empresa=empresa,
                    coformadores=listCoformador,
                    aprendices=listAprendiz
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
