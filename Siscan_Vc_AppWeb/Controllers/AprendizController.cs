using Microsoft.AspNetCore.Mvc;
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
                                nomEstadoAprendiz = fila.GetCell(13)?.ToString() ?? "",
                                codigoInscripcionTyt = fila.GetCell(14)?.ToString() ?? "",
                                ciudadPresentacion = fila.GetCell(15)?.ToString() ?? "",
                                convocatoria = fila.GetCell(16)?.ToString() ?? ""
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
                var inscripciones = new List<InscripcionTyt>();
                var aprendicesExist = new List<Aprendiz>();
                var inscripcionesExist = new List<InscripcionTyt>();
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
                            List<ConvocatoriaTyt> convocatoriaList = _dbSiscanContext.ConvocatoriaTyts.ToList();

                            for (int fila = 2; fila <= cantfilas; fila++)
                            {
                                //obtener datos de los aprendices
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
                                //obtener los datos de las foraneas del aprendiz en el excel
                                var estadotyt = hoja.Cells[fila, 10].Value.ToString().ToLower().Trim();
                                var tipodoc = hoja.Cells[fila, 11].Value.ToString().ToLower().Trim();
                                var ciudad = hoja.Cells[fila, 13].Value.ToString().ToLower().Trim();
                                var estado = hoja.Cells[fila, 14].Value.ToString().ToLower().Trim();

                                //obtener los id de las foraneas del aprendiz
                                aprendiz.IdEstadoTyt = Int32.Parse(estadoTytList.Where(e => e.DescripcionEstadotyt.Trim().ToLower() == estadotyt).Select(e => e.IdEstadotyt).FirstOrDefault().ToString());
                                aprendiz.IdTipodocumento = Int32.Parse(tipoDocList.Where(e => e.TipoDocumento1.Trim().ToLower() == tipodoc).Select(t => t.IdTipoDocumento).FirstOrDefault().ToString());
                                aprendiz.IdCiudad = Int32.Parse(ciudadList.Where(e => e.NombreCiudad.Trim().ToLower() == ciudad).Select(c => c.IdCiudad).FirstOrDefault().ToString());
                                aprendiz.IdEstadoAprendiz = Int32.Parse(estadoAprendizList.Where(e => e.NombreEstado.Trim().ToLower() == estado).Select(e => e.IdEstado).FirstOrDefault().ToString());

                                //obtener datos de la inscripcion de las tyt del aprendiz
                                if (aprendiz.IdEstadoTyt == 1)
                                {
                                    var ciudadPresentacion = hoja.Cells[fila, 16].Value.ToString().Trim().ToLower();
                                    var convocatoria = hoja.Cells[fila, 17].Value.ToString().Trim().ToLower();

                                    var inscripcion = new InscripcionTyt
                                    {
                                        CodigoInscripcion = hoja.Cells[fila, 15].Value.ToString().Trim(),
                                        NumeroDocumentoAprendiz = aprendiz.NumeroDocumentoAprendiz,
                                        IdEstadotyt = aprendiz.IdEstadoTyt
                                    };
                                    inscripcion.Idciudad = Int32.Parse(ciudadList.Where(c => c.NombreCiudad.Trim().ToLower() == ciudadPresentacion).Select(c => c.IdCiudad).FirstOrDefault().ToString());
                                    inscripcion.IdConvocatoria = Int32.Parse(convocatoriaList.Where(c => c.SemestreConvocatoria.Trim().ToLower() == convocatoria).Select(c => c.IdConvocatoria).FirstOrDefault().ToString());
                                    inscripciones.Add(inscripcion);
                                }
                                aprendices.Add(aprendiz);
                            }
                        }
                    }
                }

                //obtener aprendices existentes en la base de datos con el numero de documento
                foreach (var aprendiz in aprendices)
                {
                    var apren = await _aprendizService.GetForDoc(aprendiz.NumeroDocumentoAprendiz);
                    if (apren != null)
                    {
                        aprendicesExist.Add(apren);
                    }
                }
                //obtener los numeros de documentos de los aprendices existentes
                var numsDocs = "";
                foreach (var aprendiz in aprendicesExist)
                {
                    numsDocs += " " + aprendiz.NumeroDocumentoAprendiz + " ";
                }
                //mensaje de aprendices existentes
                if (aprendicesExist.Count > 0)
                {
                    ViewBag.AprendizExistExcel = "Los aprendices identificados con: " + numsDocs + " ya se encuentran registrados";
                }

                if (inscripciones.Count > 0)
                {
                    //obtener las inscripciones TYT existentes en la base de datos con el codigo de inscripcion
                    foreach (var inscrp in inscripciones)
                    {
                        var ins = await _inscripcionTYTService.GetForCogInscripcion(inscrp.CodigoInscripcion);
                        if (ins != null)
                        {
                            inscripcionesExist.Add(ins);
                        }
                    }

                    //obtener los codigos de inscripcion de las inscripciones TYT existentes
                    var cogInscripcion = "";
                    foreach (var inscrp in inscripcionesExist)
                    {
                        cogInscripcion += " " + inscrp.CodigoInscripcion + " ";
                    }
                    if (inscripcionesExist.Count > 0)
                    {
                        ViewBag.InscripcionesExist = "Ya existen registros de aprendices registrados con los siguientes codigos de inscripcion a las pruebas tyt: " + cogInscripcion;
                    }
                }
                //guardado de aprendices y inscripciones tyt
                if (aprendicesExist.Count == 0 && inscripcionesExist.Count == 0 && fileExcel != null)
                {
                    _dbSiscanContext.Aprendiz.AddRange(aprendices);
                    if (inscripciones.Count > 0)
                    {
                        _dbSiscanContext.InscripcionTyts.AddRange(inscripciones);
                    }
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
                    else if (apren == null && codigoInscrpExist == null)
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
                        if (aptyt.aprendiz.IdEstadoTyt == null)
                        {
                            aprendiz.IdEstadoTyt = 5;
                        }
                        else if (aprendiz.IdEstadoAprendiz == 4 && aprendiz.IdEstadoTyt == null)
                        {
                            aprendiz.IdEstadoTyt = 6;
                        }
                        else
                        {
                            aprendiz.IdEstadoTyt = aptyt.aprendiz.IdEstadoTyt;
                        }
                        await _aprendizService.Insert(aprendiz);
                        TempData["MensajeAlert"] = "Aprendiz Guardado Correctamente";
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
                        return RedirectToAction(nameof(Registro));
                    }
                    else if (codigoInscrpExist.CodigoInscripcion != null)
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
                InscripcionTyt insctyt = null;
                if (aprendi != null && aprendi.IdEstadoTyt == 1)
                {
                    insctyt = await _dbSiscanContext.InscripcionTyts.Where(i => i.NumeroDocumentoAprendiz == aprendi.NumeroDocumentoAprendiz).FirstOrDefaultAsync();
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
                                Idciudad = aprendiztyt.inscripcionTyt.Idciudad,
                                NumeroDocumentoAprendiz = aprendiztyt.aprendiz.NumeroDocumentoAprendiz,
                                IdConvocatoria = aprendiztyt.inscripcionTyt.IdConvocatoria,
                                IdEstadotyt = aprendiztyt.aprendiz.IdEstadoTyt
                            };
                            _dbSiscanContext.InscripcionTyts.Add(insctyt);
                            await _dbSiscanContext.SaveChangesAsync();
                        }
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
