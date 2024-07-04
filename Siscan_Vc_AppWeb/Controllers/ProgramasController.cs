using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class ProgramasController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly IProgramasService _programasService;
        private readonly IFichaService _fichaService;

        public ProgramasController(DbSiscanContext dbSiscanContext, IProgramasService programasService,IFichaService ficha)
        {
            _dbSiscanContext = dbSiscanContext;
            _programasService = programasService;
            _fichaService = ficha;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Cargar listas de selección
            var modelview = new ModelViewProgra
            {
                listaopcNivel = _dbSiscanContext.NivelProgramas.Select(o => new SelectListItem
                {                                                     
                    Value = o.IdNivelPrograma.ToString(),
                    Text = o.NivelPrograma1
                }).ToList(),
                listaopcEstado = _dbSiscanContext.EstadoProgramas.Select(o => new SelectListItem
                {
                    Value = o.IdEstadoPrograma.ToString(),
                    Text = o.DescripcionEstadoPrograma
                }).ToList(),
                listaprogramas = new List<ViewModelPrograma>()
            };

             List<ViewModelPrograma> listaprograma = new List<ViewModelPrograma>();
                IQueryable<Programas> queryprograma = await _programasService.GetAll();
                listaprograma = queryprograma.Select(a => new ViewModelPrograma(a)
                {
                    CodigoPrograma = a.CodigoPrograma,
                    NombrePrograma = a.NombrePrograma,
                    IdEstadoPrograma = a.IdEstadoPrograma,
                    IdNivelPrograma = a.IdNivelPrograma,
                }).ToList();

                Programas programa = new Programas();
                foreach (var item in queryprograma)
                {
                    if (item.CodigoPrograma == null)
                    {
                        programa = item;
                        break;
                    }
                }

                modelview.listaprogramas = listaprograma;
                modelview.programas = programa;

            return View(modelview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ModelViewProgra pro)
        {
            ModelViewProgra progr = new ModelViewProgra();
            try
            {
                if (pro != null)
                {
                    Programas pr = await _programasService.GetForCog(pro.programas.CodigoPrograma);
                    if (pr != null)
                    {
                        TempData["ValProgramExiste"] = "Ya existe un Programa con ese Codigo";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var programa = new Programas
                        {
                            CodigoPrograma = pro.programas.CodigoPrograma,
                            NombrePrograma = pro.programas.NombrePrograma,
                            IdEstadoPrograma = pro.opcseleccionadaEstado,
                            IdNivelPrograma = pro.opcseleccionadaNivel
                        };
                        _dbSiscanContext.Programas.Add(programa);
                        await _dbSiscanContext.SaveChangesAsync();
                        progr = new ModelViewProgra
                        {
                            programas = programa
                        };
                        TempData["AlertProAdd"] = "Programa guardado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorGuardarInstrct"] = ex.Message;
            }

            return View(pro);
        }

        public IActionResult CrearFicha()
        {
            return View();
        }
    }
}
