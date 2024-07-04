using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.XWPF.UserModel;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.ClasesService;
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
        private readonly IInstructorService _instructorService;
        

        public ProgramasController(DbSiscanContext dbSiscanContext, IProgramasService programasService, IFichaService ficha, IInstructorService instructorService)
        {
            _dbSiscanContext = dbSiscanContext;
            _programasService = programasService;
            _fichaService = ficha;
            _instructorService = instructorService;
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
            if (listaprograma.Count == 0)
            {
                TempData["NoProgramsFound"] = "No se encontraron programas válidos.";
                return RedirectToAction(nameof(Index));
            }
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
                    if (string.IsNullOrEmpty(pro.programas.CodigoPrograma))
                    {
                        TempData["ErrorGuardarInstrct"] = "El código de programa no puede ser nulo.";
                        return RedirectToAction(nameof(Index));
                    }

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

        [HttpGet]
        public async Task<IActionResult> CrearFicha(string codigo)
        {
            var modelview = new ModelViewProgra
            {
                listaopcSede = _dbSiscanContext.Sedes.Select(o => new SelectListItem
                {
                    Value = o.IdSede.ToString(),
                    Text = o.NombreSede
                }).ToList(),
                programas = new Programas { CodigoPrograma = codigo } 
            };

            if (codigo != null)
            {
                var cod = await _programasService.GetForCog(codigo);
                modelview.programas = cod;

                if (modelview.programas == null)
                {
                    return NotFound();
                }
            }
            return View(modelview);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearFicha(ModelViewProgra vmpf)
        {
            try
            {
                ModelViewProgra viewmodel = new ModelViewProgra
                {
                    listaopcSede = _dbSiscanContext.Sedes.Select(o => new SelectListItem
                    {
                        Value = o.IdSede.ToString(),
                        Text = o.NombreSede
                    }).ToList(),
                };

                if (vmpf != null)
                {
                    var ficha = new Ficha()
                    {
                        Ficha1 = vmpf.ficha.Ficha1,
                        FechaInicio = vmpf.ficha.FechaInicio,
                        FechaFinalizacion = vmpf.ficha.FechaFinalizacion,
                        CodigoPrograma = vmpf.programas.CodigoPrograma,
                        NumeroDocumentoInstructor = vmpf.ficha.NumeroDocumentoInstructor,
                        IdSede = vmpf.opcseleccionadaSede,
                    };

                    var asignacion = new AsignacionFicha()
                    {
                        Ficha = vmpf.ficha.Ficha1,
                        NumeroDocumentoInstructor = vmpf.ficha.NumeroDocumentoInstructor
                    };
                    var instr = await _instructorService.GetForDoc(ficha.NumeroDocumentoInstructor);
                    if (instr == null)
                    {
                        TempData["MensajeAlertIns"] = " Instructor no encontrado";
                    }
                    else
                    {
                        
                        _dbSiscanContext.Fichas.Add(ficha);
                        _dbSiscanContext.AsignacionFichas.Add(asignacion);
                        await _dbSiscanContext.SaveChangesAsync();
                        viewmodel.ficha = ficha;
                        TempData["MensajeAlertFi"] = "Ficha Registrado";
                        return View(viewmodel);
                    }
                }
                return RedirectToAction(nameof(CrearFicha));
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
