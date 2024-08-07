﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System.Data;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class AprendizController : Controller
    {
        private readonly IAprendizService _aprendizService;

        private readonly IInscripcionTYTService _inscripcionTYTService;

        private readonly DbSiscanContext _dbSiscanContext;

        public AprendizController(IAprendizService aprendizService, DbSiscanContext dbSiscanContext, IInscripcionTYTService inscripcionTYTService)
        {
            _dbSiscanContext = dbSiscanContext;
            _aprendizService = aprendizService;
            _inscripcionTYTService = inscripcionTYTService;
        }

        [HttpGet]
        public IActionResult ObtenerNivelPrograma(string programaId)
        {
            var programa = _dbSiscanContext.Programas.FirstOrDefault(p => p.CodigoPrograma == programaId);
            if (programa != null)
            {
                var nivelPrograma = programa.IdNivelPrograma;
                return Json(new { nivelPrograma });
            }
            else
            {
                return Json(new { nivelPrograma = "" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> CargarCiudades(int departamentoId)
        {
            var ciudades = await _dbSiscanContext.Ciudads.Where(c => c.IdDepartamento == departamentoId).ToListAsync();
            ViewBag.ciudades = ciudades;
            return Json(ciudades);
        }
        [HttpGet]
        public async Task<IActionResult> CargarFichas(int programa)
        {
            var ficha = await _dbSiscanContext.Fichas.Where(f => f.CodigoPrograma == programa.ToString()).ToListAsync();
            ViewBag.ficha = ficha;
            return Json(ficha);
        }
        [HttpGet]
        public async Task<IActionResult> Cargarprograma(int programa)
        {
            var ficha = await _dbSiscanContext.Fichas.Where(f => f.CodigoPrograma == programa.ToString()).ToListAsync();
            ViewBag.ficha = ficha;
            return Json(ficha);
        }
        //Llenar combos
        public async Task<IActionResult> Registro()
        {
            var modelview = new Modelviewtytap
            {
                //lista para el combo tipo de documento
                listaOpcTpDoc = _dbSiscanContext.TipoDocumentos.Select(o => new SelectListItem
                {
                    Value = o.IdTipoDocumento.ToString(),
                    Text = o.TipoDocumento1
                }).ToList(),
                //lista para el combo estado aprendiz
                listaOpcEstado = _dbSiscanContext.EstadoAprendizs.Select(e => new SelectListItem
                {
                    Value = e.IdEstado.ToString(),
                    Text = e.NombreEstado
                }).ToList(),
                //lista para el combo departamentos
                listaOpcDepartamento = _dbSiscanContext.Departamentos.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.NombreDepartamento
                }).ToList(),
                //lista para el combo ciudad
                listaOpcCiudad = _dbSiscanContext.Ciudads.Select(c => new SelectListItem
                {
                    Value = c.IdCiudad.ToString(),
                    Text = c.NombreCiudad
                }).ToList(),
                //lista para el combo estado tyt
                listaOpcEstadoTyt = _dbSiscanContext.EstadoInscripcionTyts.Select(e => new SelectListItem
                {
                    Value = e.IdEstadotyt.ToString(),
                    Text = e.DescripcionEstadotyt
                }).ToList(),
                //lista para el combo programas
                listaOpcPrograma = _dbSiscanContext.Programas.Select(p => new SelectListItem
                {
                    Value = p.CodigoPrograma.ToString(),
                    Text = p.NombrePrograma
                }).ToList(),
                //lista para el combo ficha
                listaOpcFicha = _dbSiscanContext.Fichas.Select(f => new SelectListItem
                {
                    Value = f.Ficha1.ToString(),
                    Text = f.Ficha1.ToString()
                }).ToList(),
                //lista para el combo convocatoria tyt
                listaOpcConvocatoria = _dbSiscanContext.ConvocatoriaTyts.Select(c => new SelectListItem
                {
                    Value = c.IdConvocatoria.ToString(),
                    Text = c.SemestreConvocatoria
                }).ToList()

            };
            return View(modelview);
        }
        [HttpGet]
        public async Task<IActionResult> RegistrarLotes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MostrarDatos([FromForm] IFormFile ArchExcel)
        {
            if (ArchExcel == null || ArchExcel.Length == 0)
            {
                return BadRequest("Archivo no válido");
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
                        return BadRequest("Seleccione un archivo excel válido");
                    }

                    ISheet HojaExcel = MiExcel.GetSheetAt(0);
                    int cantFilas = HojaExcel.LastRowNum;

                    List<VMAprendiz> listaExcel = new List<VMAprendiz>();

                    for (int i = 1; i <= cantFilas; i++)
                    {
                        IRow fila = HojaExcel.GetRow(i);
                        if (fila != null)
                        {
                            listaExcel.Add(new VMAprendiz
                            {
                                numeroDocumentoAprendiz = fila.GetCell(0)?.ToString() ?? "",
                                nombreAprendiz = fila.GetCell(1)?.ToString() ?? "",
                                apellidoAprendiz = fila.GetCell(2)?.ToString() ?? "",
                                celAprendiz = fila.GetCell(3)?.ToString() ?? "",
                                correoAprendiz = fila.GetCell(4)?.ToString() ?? "",
                                direccionAprendiz = fila.GetCell(5)?.ToString() ?? "",
                                nombreCompletoAcudiente = fila.GetCell(6)?.ToString() ?? "",
                                correoAcudiente = fila.GetCell(7)?.ToString() ?? "",
                                celularAcudiente = fila.GetCell(8)?.ToString() ?? "",
                                nomEstadoTyt = fila.GetCell(9)?.ToString() ?? "",
                                nombredoc = fila.GetCell(10)?.ToString() ?? "",
                                ficha = fila.GetCell(11)?.ToString() ?? "",
                                nomCiudad = fila.GetCell(12)?.ToString() ?? "",
                                nomEstadoAprendiz = fila.GetCell(13)?.ToString() ?? ""
                            });
                        }
                    }

                    return Ok(listaExcel);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al procesar el archivo: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarLotes(IFormFile fileExcel)
        {
            try
            {
                // Configurar el contexto de la licencia
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var aprendices = new List<Aprendiz>();
                var aprendicesExist = new List<Aprendiz>();
                if (fileExcel == null || fileExcel.Length == 0)
                {
                    ViewBag.MensajeExcelNoSelec = "Por favor seleccione un archivo";
                }
                else
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var hoja = package.Workbook.Worksheets[0];
                            var cantfilas = hoja.Dimension.Rows;
                            List<EstadoInscripcionTyt> estadoTytList = _dbSiscanContext.EstadoInscripcionTyts.ToList();
                            List<TipoDocumento> tipoDocList = _dbSiscanContext.TipoDocumentos.ToList();
                            List<Ciudad> ciudadList = _dbSiscanContext.Ciudads.ToList();
                            List<EstadoAprendiz> estadoAprendizList = _dbSiscanContext.EstadoAprendizs.ToList();

                            for (int fila = 2; fila <= cantfilas; fila++)
                            {
                                var aprendiz = new Aprendiz
                                {
                                    NumeroDocumentoAprendiz = hoja.Cells[fila, 1].Value.ToString().Trim(),
                                    NombreAprendiz = hoja.Cells[fila, 2].Value.ToString(),
                                    ApellidoAprendiz = hoja.Cells[fila, 3].Value.ToString(),
                                    CelAprendiz = hoja.Cells[fila, 4].Value.ToString().Trim(),
                                    CorreoAprendiz = hoja.Cells[fila, 5].Value.ToString().Trim(),
                                    DireccionAprendiz = hoja.Cells[fila, 6].Value.ToString(),
                                    NombreCompletoAcudiente = hoja.Cells[fila, 7].Value.ToString(),
                                    CorreoAcuediente = hoja.Cells[fila, 8].Value.ToString(),
                                    CelularAcudiente = hoja.Cells[fila, 9].Value.ToString(),
                                    Ficha = hoja.Cells[fila, 12].Value.ToString().Trim()
                                };

                                var estadotyt = hoja.Cells[fila, 10].Value.ToString().ToLower().Trim();
                                var tipodoc = hoja.Cells[fila, 11].Value.ToString().ToLower().Trim();
                                var ciudad = hoja.Cells[fila, 13].Value.ToString().ToLower().Trim();
                                var estado = hoja.Cells[fila, 14].Value.ToString().ToLower().Trim();
                                foreach (var estyt in estadoTytList)
                                {
                                    if (estyt.DescripcionEstadotyt.Trim().ToLower() == estadotyt)
                                    {
                                        aprendiz.IdEstadoTyt = Int32.Parse(estyt.IdEstadotyt.ToString());
                                    }
                                }
                                foreach (var tpdoc in tipoDocList)
                                {
                                    if (tpdoc.TipoDocumento1.Trim().ToLower() == tipodoc)
                                    {
                                        aprendiz.IdTipodocumento = tpdoc.IdTipoDocumento;
                                    }
                                }
                                foreach (var ciud in ciudadList)
                                {
                                    if (ciud.NombreCiudad.Trim().ToLower() == ciudad)
                                    {
                                        aprendiz.IdCiudad = ciud.IdCiudad;
                                    }
                                }
                                foreach (var std in estadoAprendizList)
                                {
                                    if (std.NombreEstado.Trim().ToLower() == estado)
                                    {
                                        aprendiz.IdEstadoAprendiz = std.IdEstado;
                                    }
                                }
                                aprendices.Add(aprendiz);
                            }
                        }
                    }
                }

                foreach (var aprendiz in aprendices)
                {
                    var apren = await _aprendizService.GetForDoc(aprendiz.NumeroDocumentoAprendiz);
                    if (apren != null)
                    {
                        aprendicesExist.Add(apren);
                    }
                }
                var numsDocs = "";
                foreach (var aprendiz in aprendicesExist)
                {
                    numsDocs += " " + aprendiz.NumeroDocumentoAprendiz + ",";
                }
                if (aprendicesExist.Count > 0)
                {
                    ViewBag.AprendizExistExcel = "Los aprendices identificados con: " + numsDocs + " ya se encuentran registrados";
                }
                else if (aprendicesExist.Count == 0 && fileExcel != null)
                {
                    _dbSiscanContext.Aprendiz.AddRange(aprendices);
                    await _dbSiscanContext.SaveChangesAsync();
                    ViewBag.mensajeAprendices = "Aprendices registrados exitosamente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.CatchRegistrarExcelAprendz = "Error: " + ex.Message;
            }
            return View();
        }

        //Registrar aprendiz con un view model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Modelviewtytap aptyt)
        {
            Modelviewtytap vmtytap = new Modelviewtytap();
            InscripcionTyt codigoInscrpExist = null;
            try
            {
                if (aptyt != null)
                {
                    Aprendiz apren = await _aprendizService.GetForDoc(aptyt.aprendiz.NumeroDocumentoAprendiz);
                    if (aptyt.inscripcionTyt.CodigoInscripcion != null)
                    {
                        codigoInscrpExist = await _inscripcionTYTService.GetForCogInscripcion(aptyt.inscripcionTyt.CodigoInscripcion);
                    }
                    if (apren != null)
                    {
                        TempData["ValAprendzExiste"] = "Ya existe un aprendiz con este numero de documento";
                        return RedirectToAction(nameof(Registro));
                    }
                    else if (apren == null && codigoInscrpExist==null)
                    {
                        var aprendiz = new Aprendiz()
                        {
                            IdTipodocumento = aptyt.OpcSeleccionadoTpDoc,
                            NumeroDocumentoAprendiz = aptyt.aprendiz.NumeroDocumentoAprendiz,
                            NombreAprendiz = aptyt.aprendiz.NombreAprendiz,
                            ApellidoAprendiz = aptyt.aprendiz.ApellidoAprendiz,
                            CelAprendiz = aptyt.aprendiz.CelAprendiz,
                            DireccionAprendiz = aptyt.aprendiz.DireccionAprendiz,
                            CorreoAprendiz = aptyt.aprendiz.CorreoAprendiz,
                            IdEstadoAprendiz = aptyt.OpcSeleccionadoEstado,
                            IdCiudad = aptyt.OpcSeleccionadoCiudad,
                            Ficha = aptyt.OpcSeleccionadoFicha.ToString(),
                            NombreCompletoAcudiente = aptyt.aprendiz.NombreCompletoAcudiente,
                            CelularAcudiente = aptyt.aprendiz.CelularAcudiente,
                            CorreoAcuediente = aptyt.aprendiz.CorreoAcuediente
                        };
                        await _aprendizService.Insert(aprendiz);
                        if (aprendiz.IdEstadoAprendiz == 4 && aprendiz.IdEstadoTyt == null)
                        {
                            aprendiz.IdEstadoTyt = 6;
                        }
                        else
                        {
                            aprendiz.IdEstadoTyt = aptyt.aprendiz.IdEstadoTyt;
                        }
                        if (aprendiz.NumeroDocumentoAprendiz == aptyt.aprendiz.NumeroDocumentoAprendiz && aprendiz.IdEstadoTyt == 1)
                        {
                            var tyt = new InscripcionTyt()
                            {
                                CodigoInscripcion = aptyt.inscripcionTyt.CodigoInscripcion,
                                NumeroDocumentoAprendiz = aprendiz.NumeroDocumentoAprendiz,
                                Idciudad = aptyt.OpcSeleccionadoCiudadTyt,
                                IdConvocatoria = aptyt.OpcSeleccionadoConvocatoria,
                                IdEstadotyt = aprendiz.IdEstadoTyt
                            };
                            _dbSiscanContext.InscripcionTyts.Add(tyt);
                            _dbSiscanContext.SaveChanges();
                        }
                        vmtytap = new Modelviewtytap
                        {
                            aprendiz = aptyt.aprendiz,
                            inscripcionTyt = aptyt.inscripcionTyt
                        };
                        if (vmtytap.aprendiz.NumeroDocumentoAprendiz != null)
                        {
                            TempData["MensajeAlert"] = "Aprendiz Guardado Correctamente";
                        }
                        return RedirectToAction(nameof(Registro));
                    }
                    else if(codigoInscrpExist.CodigoInscripcion!=null)
                    {
                        TempData["ValInscripExiste"] = "Ya existe un aprendiz registrado con este codigo de inscripcion";
                        return Json(new { success = true, message = "Ya existe un aprendiz registrado con este codigo de inscripcion" });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["registroAprendizExcepcion"] = ex.Message;
            }
            return View(aptyt);
        }

        //consultar aprendiz por numero de documento y obtener la lista de aprendices
        [HttpGet]
        public async Task<IActionResult> Consultar(string nmdoc)
        {
            List<ViewModelAprendiz> listaAprendiz = new List<ViewModelAprendiz>();
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();

            listaAprendiz = queryAprendiz.Select(a => new ViewModelAprendiz(a)
            {
                NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                NombreAprendiz = a.NombreAprendiz,
                ApellidoAprendiz = a.ApellidoAprendiz,
                CelAprendiz = a.CelAprendiz,
                CorreoAprendiz = a.CorreoAprendiz,
                DireccionAprendiz = a.DireccionAprendiz,
                NombreCompletoAcudiente = a.NombreCompletoAcudiente,
                CorreoAcuediente = a.CorreoAcuediente,
                CelularAcudiente = a.CelularAcudiente,
                IdEstadoTyt = a.IdEstadoTytNavigation.IdEstadotyt,
                nomEstadoTyt = a.IdEstadoTytNavigation.DescripcionEstadotyt,
                IdTipodocumento = a.IdTipodocumentoNavigation.IdTipoDocumento,
                nombredoc = a.IdTipodocumentoNavigation.TipoDocumento1,
                Ficha = a.Ficha,
                IdCiudad = a.IdCiudad,
                IdEstadoAprendiz = a.IdEstadoAprendiz,
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado
            }).ToList();

            Aprendiz aprendiz = new Aprendiz();
            foreach (var aprendi in queryAprendiz)
            {
                if (aprendi.NumeroDocumentoAprendiz == nmdoc)
                {
                    aprendiz = aprendi;
                    break;
                }
            }

            ModelviewAp viewModel = new ModelviewAp
            {

                Aprendiz = aprendiz,
                ListaAprendices = listaAprendiz
            };

            TempData["aprendizConsultAlert"] = "No hay Resultados";
            return View(viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string nmdoc)
        {
            try
            {
                var aprendiz = await _dbSiscanContext.Aprendiz.FirstOrDefaultAsync(x => x.NumeroDocumentoAprendiz == nmdoc);

                if (aprendiz == null)
                {
                    return Json(new { success = false, message = "El aprendiz no fue encontrado." });
                }
                TempData["MensajeAlertEliminado"] = "Aprendiz eliminado correctamente!!";
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.Where(i => i.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.SeguimientoInstructorAprendizs.RemoveRange(seguimiento);
                var actividades = await _dbSiscanContext.Actividades.Where(a => a.IdSeguimientoNavigation.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.Actividades.RemoveRange(actividades);
                var observaciones = await _dbSiscanContext.Observacions.Where(o => o.IdSeguimientoNavigation.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.Observacions.RemoveRange(observaciones);
                var inscripciones = await _dbSiscanContext.InscripcionTyts.Where(i => i.NumeroDocumentoAprendiz == nmdoc).ToListAsync();
                _dbSiscanContext.InscripcionTyts.RemoveRange(inscripciones);
                await _aprendizService.Delete(nmdoc);
                return Json(new { success = true, message = "El aprendiz se eliminó correctamente." });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Se produjo un error al intentar eliminar el aprendiz: " + e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string numDoc)
        {
            var viewModel = new Modelviewtytap();
            ViewBag.ItemsTipoDoc = new SelectList(await _dbSiscanContext.TipoDocumentos.ToListAsync(), "IdTipoDocumento", "TipoDocumento1");
            ViewBag.ItemsEstAprndz = new SelectList(await _dbSiscanContext.EstadoAprendizs.ToListAsync(), "IdEstado", "NombreEstado");
            ViewBag.ItemsDepartamento = new SelectList(await _dbSiscanContext.Departamentos.ToListAsync(), "IdDepartamento", "NombreDepartamento");
            ViewBag.ciudades = new SelectList(await _dbSiscanContext.Ciudads.ToListAsync(), "IdCiudad", "NombreCiudad");
            ViewBag.ItemsEstaTYT = new SelectList(await _dbSiscanContext.EstadoInscripcionTyts.ToListAsync(), "IdEstadotyt", "DescripcionEstadotyt");
            ViewBag.ItemsPrograma = new SelectList(await _dbSiscanContext.Programas.ToListAsync(), "CodigoPrograma", "NombrePrograma");
            ViewBag.ficha = new SelectList(await _dbSiscanContext.Fichas.ToListAsync(), "Ficha1", "Ficha1");
            ViewBag.ItemsConvocatoria = new SelectList(await _dbSiscanContext.ConvocatoriaTyts.ToListAsync(), "IdConvocatoria", "SemestreConvocatoria");

            if (numDoc != null)
            {
                var aprendi = await _aprendizService.GetForDoc(numDoc);
                InscripcionTyt insctyt;

                if (aprendi.IdEstadoTyt == 1)
                {
                    insctyt = _dbSiscanContext.InscripcionTyts.First(i => i.NumeroDocumentoAprendiz == aprendi.NumeroDocumentoAprendiz);
                    TempData["CodigoInscripcionExist"] = "Ya existe inscripcion";
                }
                else
                {
                    insctyt = null;
                }

                viewModel = new Modelviewtytap
                {
                    aprendiz = aprendi,
                    inscripcionTyt = insctyt
                };

                if (viewModel.aprendiz == null)
                {
                    return NotFound();
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Modelviewtytap aprendiztyt)
        {
            InscripcionTyt insctyt;
            InscripcionTyt codigInsExist = null;
            if (aprendiztyt != null)
            {
                if (aprendiztyt.inscripcionTyt.CodigoInscripcion != null)
                {
                    codigInsExist = await _inscripcionTYTService.GetForCogInscripcion(aprendiztyt.inscripcionTyt.CodigoInscripcion);
                }
                var aprendiz = await _aprendizService.GetForDoc(aprendiztyt.aprendiz.NumeroDocumentoAprendiz);
                if (aprendiz == null)
                {
                    return NotFound();
                }

                //asignar los datos actualizados a aprendiz
                aprendiz.IdTipodocumento = aprendiztyt.aprendiz.IdTipodocumento;
                aprendiz.NombreAprendiz = aprendiztyt.aprendiz.NombreAprendiz;
                aprendiz.ApellidoAprendiz = aprendiztyt.aprendiz.ApellidoAprendiz;
                aprendiz.CelAprendiz = aprendiztyt.aprendiz.CelAprendiz;
                aprendiz.CorreoAprendiz = aprendiztyt.aprendiz.CorreoAprendiz;
                aprendiz.DireccionAprendiz = aprendiztyt.aprendiz.DireccionAprendiz;
                aprendiz.NombreCompletoAcudiente = aprendiztyt.aprendiz.NombreCompletoAcudiente;
                aprendiz.CorreoAcuediente = aprendiztyt.aprendiz.CorreoAcuediente;
                aprendiz.CorreoAcuediente = aprendiztyt.aprendiz.CorreoAcuediente;
                aprendiz.CelularAcudiente = aprendiztyt.aprendiz.CelularAcudiente;
                aprendiz.IdEstadoAprendiz = aprendiztyt.aprendiz.IdEstadoAprendiz;
                aprendiz.Ficha = aprendiztyt.aprendiz.Ficha;
                aprendiz.IdCiudad = aprendiztyt.aprendiz.IdCiudad;
                aprendiz.IdEstadoTyt = aprendiztyt.aprendiz.IdEstadoTyt;

                try
                {
                    if (aprendiz.IdEstadoTyt == 1 && codigInsExist == null)
                    {
                        insctyt = await _dbSiscanContext.InscripcionTyts.Where(i => i.NumeroDocumentoAprendiz == aprendiztyt.aprendiz.NumeroDocumentoAprendiz).FirstOrDefaultAsync();
                        if (insctyt != null)
                        {
                            insctyt.CodigoInscripcion = aprendiztyt.inscripcionTyt.CodigoInscripcion;
                            insctyt.Idciudad = aprendiztyt.inscripcionTyt.Idciudad;
                            insctyt.NumeroDocumentoAprendiz = aprendiztyt.aprendiz.NumeroDocumentoAprendiz;
                            insctyt.IdConvocatoria = aprendiztyt.inscripcionTyt.IdConvocatoria;
                            insctyt.IdEstadotyt = aprendiztyt.aprendiz.IdEstadoTyt;

                            _dbSiscanContext.InscripcionTyts.Update(insctyt);
                            await _dbSiscanContext.SaveChangesAsync();

                        }
                        else if (insctyt == null)
                        {
                            insctyt = new InscripcionTyt
                            {
                                CodigoInscripcion = aprendiztyt.inscripcionTyt.CodigoInscripcion,
                                Idciudad = aprendiztyt.inscripcionTyt.Idciudad,
                                NumeroDocumentoAprendiz = aprendiztyt.aprendiz.NumeroDocumentoAprendiz,
                                IdConvocatoria = aprendiztyt.inscripcionTyt.IdConvocatoria,
                                IdEstadotyt = aprendiztyt.aprendiz.IdEstadoTyt
                            };
                            _dbSiscanContext.InscripcionTyts.Add(insctyt);
                            await _dbSiscanContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        TempData["CodigoInscripcionSiExist"] = "Ya existe un aprendiz registrado con este codigo de inscripcion";
                    }
                    await _aprendizService.Update(aprendiz);
                    TempData["AprendizEditBien"] = "El aprendiz se ha actualizado correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    //validacion de existencia del aprendiz
                    if (!AprendizExists(aprendiztyt.aprendiz.NumeroDocumentoAprendiz)) return NotFound(); else throw;
                }
            }
            return View(aprendiztyt);
        }
        private bool AprendizExists(string numeroDocumento)
        {
            return _dbSiscanContext.Aprendiz.Any(a => a.NumeroDocumentoAprendiz == numeroDocumento);
        }

    }
}
