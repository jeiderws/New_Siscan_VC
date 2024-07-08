using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.XWPF.UserModel;
using OfficeOpenXml;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.ClasesService;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<IActionResult> Index(ModelViewProgra pro, IFormFile fileExcel)
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
                TempData["ErrorGuardarInstrct"] = "Error: " + ex.Message;
            }

            return View(progr);
        }

        [HttpGet]
        public IActionResult RegistrarLotes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarLotes(IFormFile fileExcel)
        {
            try
            {
                // Configurar el contexto de la licencia
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var fichas = new List<Ficha>();
                var fichasExist = new List<Ficha>();
                if (fileExcel == null || fileExcel.Length == 0)
                {
                    ViewBag.MensajeExcelNoSelecFch = "Por favor seleccione un archivo";
                }
                else
                {
                    var ficha = new Ficha();
                    using (var stream = new MemoryStream())
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var hoja = package.Workbook.Worksheets[0];
                            var cantFilas = hoja.Dimension.Rows;

                            List<Programas> listProgramas = _dbSiscanContext.Programas.ToList();
                            List<Instructor> listInstructores = _dbSiscanContext.Instructors.ToList();
                            List<Sede> listSedes = _dbSiscanContext.Sedes.ToList();

                            for (int fila = 2; fila <= cantFilas; fila++)
                            {
                                var fechaInicioStr = hoja.Cells[fila, 2].Text;
                                var fechaFinalStr = hoja.Cells[fila, 3].Text;
                                if (DateTime.TryParseExact(fechaInicioStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaInicio) && DateTime.TryParseExact(fechaFinalStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaFinal))
                                {
                                    ficha = new Ficha
                                    {
                                        Ficha1 = hoja.Cells[fila, 1].Value.ToString().Trim(),
                                        FechaInicio = DateOnly.FromDateTime(fechaInicio),
                                        FechaFinalizacion = DateOnly.FromDateTime(fechaFinal),
                                        NumeroDocumentoInstructor = hoja.Cells[fila, 5].Value.ToString().Trim()
                                    };
                                }

                                var programa = hoja.Cells[fila, 4].Value.ToString().Trim().ToLower();
                                var sede = hoja.Cells[fila, 6].Value.ToString().Trim().ToLower();

                                foreach (var program in listProgramas)
                                {
                                    if (program.NombrePrograma.Trim().ToLower() == programa)
                                    {
                                        ficha.CodigoPrograma = program.CodigoPrograma;
                                    }
                                }

                                foreach (var sed in listSedes)  
                                {
                                    if (sed.NombreSede.Trim().ToLower() == sede)
                                    {
                                        ficha.IdSede = sed.IdSede;
                                    }
                                }
                                fichas.Add(ficha);
                            }
                        }
                    }
                }
                foreach (var ficha in fichas)
                {
                    var fich = await _fichaService.GetForFicha(ficha.Ficha1);
                    if (fich != null)
                    {
                        fichasExist.Add(fich);
                    }
                }
                var fichs = "";
                foreach (var ficha in fichasExist)
                {
                    fichs += " " + ficha.Ficha1+",";
                }
                if (fichasExist.Count > 0)
                {
                    ViewBag.FichasExcistExcel = "Las fichas: " + fichs + " ya se encuentran registradas";
                }
                else if (fichasExist.Count == 0 && fileExcel != null)
                {
                    _dbSiscanContext.Fichas.AddRange(fichas);
                    await _dbSiscanContext.SaveChangesAsync();
                    ViewBag.mensajeFichas = "Fichas registradas exitosamente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.CatchRegistrarExcelFicha = "Error: " + ex.Message;
            }
            return View();
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
