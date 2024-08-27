using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
using static NPOI.HSSF.Util.HSSFColor;

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
                    fichs += " " + ficha.Ficha1 + ",";
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

        [HttpPost]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchExcel)
        {
            if (ArchExcel == null || ArchExcel.Length == 0)
            {
                return BadRequest("Archivo no valido");
            }
            try
            {
                using (var stream = ArchExcel.OpenReadStream())
                {
                    IWorkbook MiExcel = null;
                    if (Path.GetExtension(ArchExcel.FileName) == ".xlsx")
                    {
                        MiExcel = new XSSFWorkbook(stream);
                    }
                    else
                    {
                        return BadRequest("Seleccione un archivo excel valido");
                    }

                    ISheet HojaExcel = MiExcel.GetSheetAt(0);
                    int cantFilas = HojaExcel.LastRowNum;

                    List<VMFicha> listExcelFchs = new List<VMFicha>();

                    for (int i = 1; i <= cantFilas; i++)
                    {
                        IRow fila = HojaExcel.GetRow(i);
                        if (fila != null)
                        {
                            listExcelFchs.Add(new VMFicha
                            {
                                ficha1 = fila.GetCell(0)?.ToString() ?? "",
                                fechaInicio = fila.GetCell(1).ToString(),
                                fechaFinalizacion = fila.GetCell(2).ToString(),
                                programa = fila.GetCell(3)?.ToString() ?? "",
                                numeroDocumentoInstructor = fila.GetCell(4)?.ToString() ?? "",
                                sede = fila.GetCell(5)?.ToString() ?? ""
                            });
                        }
                    }
                    return Ok(listExcelFchs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar el archivo: {ex.Message}");
            }
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
        [HttpGet]
        public async Task<IActionResult> Consultar(string codigo)
        {
            Programas programa=new Programas();
            List<ViewModelPrograma> listaprogramas = new List<ViewModelPrograma>();
            IQueryable<Programas> queryprogramas = await _programasService.GetAll();
            listaprogramas = queryprogramas.Select(p => new ViewModelPrograma(p)
            {
                CodigoPrograma = p.CodigoPrograma,
                NombrePrograma = p.NombrePrograma,
                IdEstadoPrograma = p.IdEstadoPrograma,
                IdNivelPrograma = p.IdNivelPrograma,


            }).ToList();
            if (listaprogramas.Count == 0)
            {
                
                return RedirectToAction(nameof(Index));
            }
            if (codigo != null)
            {
                programa = await _programasService.GetForCog(codigo);

            }
            if (codigo == null)
            {
                TempData["NoProgramsFound"] = "No Hay Resultados.";
            }
            ModelViewProgra modelViewProgra = new ModelViewProgra
            {
                programas = programa,
                listaprogramas = listaprogramas,
            };
            return View(modelViewProgra);
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(string Codigo)
        {
            try
            {
                var programa = await _dbSiscanContext.Programas.FirstOrDefaultAsync(p => p.CodigoPrograma == Codigo);
                if (programa == null)
                {
                    TempData["AlertProgramaNoEncontrado"] = "El Programa no fue encontrado";
                    return Json(new { success = false, message = "El Programa no fue encontrado" });
                }
                var fichas = await _dbSiscanContext.Fichas.Where(f => f.CodigoPrograma == Codigo).ToListAsync();
                foreach (var ficha in fichas)
                {
                    var asignaciones = await _dbSiscanContext.AsignacionFichas.Where(af => af.Ficha == ficha.Ficha1).ToListAsync();
                    if (asignaciones.Count > 0)
                    {
                        _dbSiscanContext.AsignacionFichas.RemoveRange(asignaciones);
                    }
                }

                if (fichas.Count > 0)
                {
                    _dbSiscanContext.Fichas.RemoveRange(fichas);
                }

                _dbSiscanContext.Programas.Remove(programa);
                await _dbSiscanContext.SaveChangesAsync();

                TempData["AlertEliminadoPrograma"] = "Programa eliminado correctamente!!";
                return Json(new { success = true, message = "El programa se eliminó correctamente." });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Json(new { success = false, message = "Se produjo un error al intentar eliminar el programa: " + innerException });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Editar(string cdg)
        {
            ViewBag.ItemsNivelModel = new SelectList(await _dbSiscanContext.NivelProgramas.ToListAsync(), "IdNivelPrograma", "NivelPrograma1");
            ViewBag.ItemsEstadoModel = new SelectList(await _dbSiscanContext.EstadoProgramas.ToListAsync(), "IdEstadoPrograma", "DescripcionEstadoPrograma");
            var viewmodel = new  ModelViewProgra();
            if (cdg != null)
            {
                var program = await _programasService.GetForCog(cdg);
                viewmodel = new ModelViewProgra
                {
                    programas = program
                };
                if (viewmodel.programas == null)
                {
                    return NotFound();

                }
            }
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ModelViewProgra program)
        {
            if (program != null)
            {
                try
                {
                    var programa = await _programasService.GetForCog(program.programas.CodigoPrograma);
                    if (programa == null)
                    {
                        return NotFound();
                    }
                    programa.CodigoPrograma = program.programas.CodigoPrograma;
                    programa.NombrePrograma = program.programas.NombrePrograma;
                    programa.IdNivelPrograma = program.programas.IdNivelPrograma;
                    programa.IdEstadoPrograma = program.programas.IdEstadoPrograma;
                    _dbSiscanContext.Programas.Update(programa);
                    await _dbSiscanContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!programExist(program.programas.CodigoPrograma))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Consultar));
            }
            return View();
        }
        private bool programExist( string Codigo)
        {
            return _dbSiscanContext.Programas.Any(p => p.CodigoPrograma == Codigo);
        }     

        [HttpGet]
        public IActionResult ConsultarFicha(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                return NotFound();
            }

            var programa = _dbSiscanContext.Programas
                .Include(p => p.Fichas)
                .ThenInclude(f => f.NumeroDocumentoInstructorNavigation)
                .Include(p => p.Fichas)
                .ThenInclude(f => f.IdSedeNavigation)
                .FirstOrDefault(p => p.CodigoPrograma == codigo);

            if (programa == null)
            {
                TempData["AlertFichanotfound"] = "No se encontraron fichas para este programa.";
                return NotFound();
            }

            if (!programa.Fichas.Any())
            {
                TempData["AlertFichanotfound"] = "No se encontraron fichas para este programa.";
            }

            var viewModel = new ModelViewProgra
            {
                programas = programa,
                listaFicha = programa.Fichas.Select(f => new ViewModelFicha(f)).ToList()
            };

            return View(viewModel);
        }
        [HttpDelete]
        public async Task<IActionResult> EliminarFicha(string codigo)
        {
            try
            {
                var ficha = await _dbSiscanContext.Fichas.FirstOrDefaultAsync(f => f.Ficha1 == codigo);
                if (ficha == null)
                {
                    TempData["AlertFichaNoEncontrado"] = "La Ficha no fue encontrada";
                    return Json(new { success = false, message = "La ficha no fue encontrada." });
                }
                var fichas = await _dbSiscanContext.Fichas.Where(f => f.Ficha1 == codigo).ToListAsync();
                foreach (var i in fichas)
                {
                    var asignaciones = await _dbSiscanContext.AsignacionFichas.Where(af => af.Ficha == i.Ficha1).ToListAsync();
                    if (asignaciones.Count > 0)
                    {
                        _dbSiscanContext.AsignacionFichas.RemoveRange(asignaciones);
                    }
                }

                if (fichas.Count > 0)
                {
                    _dbSiscanContext.Fichas.RemoveRange(fichas);
                }


                _dbSiscanContext.Fichas.Remove(ficha);
                await _dbSiscanContext.SaveChangesAsync();

                TempData["AlertFichaEliminada"] = "Ficha eliminada correctamente!!";
                return Json(new { success = true, message = "La ficha se eliminó correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Se produjo un error al intentar eliminar la ficha: " + ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditarFicha(string fi)
        {
            ViewBag.ItemsSede = new SelectList(await _dbSiscanContext.Sedes.ToListAsync(), "IdSede", "NombreSede");
            var viewmodel = new ModelViewProgra();
            if (fi != null)
            {
                var fich = await _fichaService.GetForFicha(fi);
                viewmodel = new ModelViewProgra
                {
                    ficha = fich,
                };
                if (viewmodel.ficha == null)
                {
                    return NotFound();
                }
            }
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFicha(ModelViewProgra fichas)
        {
            if (fichas != null)
            {
                try
                {
                    var fich = await _fichaService.GetForFicha(fichas.ficha.Ficha1);
                    if (fich == null)
                    {
                        return NotFound();
                    }
                    fich.Ficha1 = fichas.ficha.Ficha1;
                    fich.FechaInicio = fichas.ficha.FechaInicio;
                    fich.FechaFinalizacion = fichas.ficha.FechaFinalizacion;
                    fich.CodigoPrograma = fichas.ficha.CodigoPrograma;
                    fich.NumeroDocumentoInstructor = fichas.ficha.NumeroDocumentoInstructor;
                    fich.IdSede = fichas.ficha.IdSede;
                    _dbSiscanContext.Fichas.Update(fich);
                    await _dbSiscanContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!fichaExist(fichas.ficha.Ficha1))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                   
                }
                return RedirectToAction(nameof(Consultar));
            }
            return View (fichas);
        }
        private bool fichaExist(string fich)
        {
            return _dbSiscanContext.Fichas.Any(f => f.Ficha1 == fich);
        }


    }

}
