﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System.Net.WebSockets;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly ISeguimientoService _seguimientoService;
        private readonly IEmpresaService _empresaService;
        private readonly IAprendizService _aprendizService;
        private readonly IAsignacionService _asignacionService;
        private readonly IInstructorService _instructorService;
        private readonly IActividadService _actividadService;
        private readonly IObservacionesService _observacionesService;
        private readonly ISeguimientoArchivoService _seguimientoArchivoService;
        public SeguimientoController(DbSiscanContext dbSiscanContext, IInstructorService instructorService, ISeguimientoArchivoService seguimientoArchivoService, ISeguimientoService seguimientoService, IEmpresaService empresaService, IAprendizService aprendizService, IAsignacionService asignacionService, IActividadService actividadService, IObservacionesService observacionesService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _empresaService = empresaService;
            _aprendizService = aprendizService;
            _asignacionService = asignacionService;
            _instructorService = instructorService;
            _seguimientoArchivoService = seguimientoArchivoService;
            _actividadService = actividadService;
            _observacionesService = observacionesService;
        }
        //metodo para llenar los select
        public async Task LlenarCombos()
        {
            var itemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;


            var itemsmodalidad = await _dbSiscanContext.Modalidads.ToListAsync();
            ViewBag.ItemsModalidad = itemsmodalidad;

            var itemsAreaempresa = await _dbSiscanContext.AreasEmpresas.ToListAsync();
            ViewBag.Itemsareaempresa = itemsAreaempresa;

            var itemsEmpresa = await _dbSiscanContext.Empresas.ToListAsync();
            ViewBag.Itemsempresa = itemsEmpresa;

            var itemsCooformador = await _dbSiscanContext.Coformadors.ToListAsync();
            ViewBag.Itemscooformador = itemsCooformador;

        }
        //metodo para obtener que los aprendices que se le va asignar el seguimiento
        [HttpGet]
        public async Task<IActionResult> Index(string numdoc)
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();

            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();

            List<ViewModelAprendiz> listaAprendizSegui = new List<ViewModelAprendiz>();

            listaAprendices = queryAprendiz.Select(a => new ViewModelAprendiz(a)
            {
                Tipodocumento = a.IdTipodocumentoNavigation.TipoDocumento1,
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
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado,
                SeguimientoInstructorAprendices = a.SeguimientoInstructorAprendizs,
                NombreApellidoDoc = a.NombreAprendiz + " " + a.ApellidoAprendiz + " " + a.NumeroDocumentoAprendiz
            }).ToList();
            ViewModelAprendiz aprendi = null;
            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count() < 3 && ap.IdEstadoAprendiz != 4 && ap.IdEstadoAprendiz != 3)
                {
                    listaAprendizSegui.Add(ap);
                }
            }
            Aprendiz apren = null;
            if (numdoc != null)
            {
                aprendi = listaAprendizSegui.Where(s => s.NumeroDocumentoAprendiz == numdoc.Trim()).FirstOrDefault();
                apren = await _aprendizService.GetForDoc(numdoc.Trim());
            }


            if (apren != null && apren.SeguimientoInstructorAprendizs.Count() >= 5)
            {
                TempData["MensajeAlertMAxSegui"] = "Este Aprendiz Tiene Limites de Seguimiento";
            }
            var vmSeguimiento = new Viewmodelsegui
            {
                listaAprendizSegui = listaAprendizSegui,
                aprendizSegui = aprendi
            };
            return View(vmSeguimiento);
        }
        //metodo para obtener los datos del aprendiz que se le va asignar el seguimiento
        [HttpGet]
        public async Task<IActionResult> Crear(string nmDoc)
        {
            var viewmodel = new Viewmodelsegui();
            if (nmDoc != null)
            {
                var aprendiz = await _aprendizService.GetForDoc(nmDoc);
                string tpDocAprendiz = _dbSiscanContext.TipoDocumentos.Where(tp => tp.IdTipoDocumento == aprendiz.IdTipodocumento).Select(tp => tp.TipoDocumento1).FirstOrDefault();
                viewmodel = new Viewmodelsegui
                {
                    listaopcModalidad = _dbSiscanContext.Modalidads.Select(m => new SelectListItem
                    {
                        Value = m.IdModalidad.ToString(),
                        Text = m.NombreModalidad
                    }).ToList(),
                    listaopcAreaEmpre = _dbSiscanContext.AreasEmpresas.Select(a => new SelectListItem
                    {
                        Value = a.IdArea.ToString(),
                        Text = a.NombreArea
                    }).ToList(),
                    listaopcCoform = _dbSiscanContext.Coformadors.Select(c => new SelectListItem
                    {
                        Value = c.IdCoformador.ToString(),
                        Text = c.NombreCoformador + " " + c.ApellidoCoformador
                    }).ToList(),
                    aprendiz = aprendiz,
                    tipoDocAprendiz = tpDocAprendiz
                };
                if (viewmodel.aprendiz == null)
                {
                    return NotFound();
                }
            }
            return View(viewmodel);
        }
        //metodo para crear el seguimiento al aprendiz obtenido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Viewmodelsegui Vmse)
        {
            Viewmodelsegui viewmodelsegui = new Viewmodelsegui();
            SeguimientoInstructorAprendiz seguimiento = null;
            try
            {
                if (Vmse.aprendiz.NumeroDocumentoAprendiz != null)
                {
                    if (!ModelState.IsValid)
                    {
                        var aprendiz = await _aprendizService.GetForDoc(Vmse.aprendiz.NumeroDocumentoAprendiz);
                        string tpDocAprendiz = _dbSiscanContext.TipoDocumentos.Where(tp => tp.IdTipoDocumento == aprendiz.IdTipodocumento).Select(tp => tp.TipoDocumento1).FirstOrDefault();
                        viewmodelsegui = new Viewmodelsegui
                        {
                            listaopcModalidad = _dbSiscanContext.Modalidads.Select(m => new SelectListItem
                            {
                                Value = m.IdModalidad.ToString(),
                                Text = m.NombreModalidad
                            }).ToList(),
                            listaopcAreaEmpre = _dbSiscanContext.AreasEmpresas.Select(a => new SelectListItem
                            {
                                Value = a.IdArea.ToString(),
                                Text = a.NombreArea
                            }).ToList(),
                            listaopcCoform = _dbSiscanContext.Coformadors.Select(c => new SelectListItem
                            {
                                Value = c.IdCoformador.ToString(),
                                Text = c.NombreCoformador + " " + c.ApellidoCoformador
                            }).ToList(),
                            aprendiz = aprendiz,
                            tipoDocAprendiz = tpDocAprendiz
                        };
                        return View(nameof(Crear), viewmodelsegui);
                    }
                }
                if (Vmse != null)
                {
                    if (Vmse.seguimientoinstructorAprendiz.IdModalidad != 3)
                    {
                        seguimiento = new SeguimientoInstructorAprendiz()
                        {
                            NumeroDocumentoAprendiz = Vmse?.aprendiz?.NumeroDocumentoAprendiz,
                            NumeroDocumentoInstructor = Vmse?.seguimientoinstructorAprendiz?.NumeroDocumentoInstructor,
                            IdCoformador = Vmse?.seguimientoinstructorAprendiz?.IdCoformador,
                            FechaInicio = Vmse?.seguimientoinstructorAprendiz?.FechaInicio,
                            FechaFinalizacion = Vmse?.seguimientoinstructorAprendiz?.FechaFinalizacion,
                            FechaRealizacionSeguimiento = Vmse?.seguimientoinstructorAprendiz.FechaRealizacionSeguimiento,
                            IdModalidad = Vmse?.seguimientoinstructorAprendiz?.IdModalidad,
                            IdAreaEmpresa = Vmse?.seguimientoinstructorAprendiz?.IdAreaEmpresa,
                            NitEmpresa = Vmse?.seguimientoinstructorAprendiz.NitEmpresa
                        };

                        var empre = await _empresaService.GetForNit(seguimiento.NitEmpresa);
                        var instructor = await _instructorService.GetForDoc(seguimiento.NumeroDocumentoInstructor);
                        if (empre == null)
                        {
                            TempData["MensajeAlertEmpre"] = "Nit de Empresa no encontrado";
                        }
                        if (instructor == null)
                        {
                            TempData["MensajeAlertInstruc"] = "No se encontro un instructor con este numero de documento";
                        }
                        else if (empre != null && instructor != null)
                        {
                            await _seguimientoService.Insert(seguimiento);

                            var asignacion = new AsignacionArea()
                            {
                                IdArea = seguimiento.IdAreaEmpresa,
                                NitEmpresa = seguimiento.NitEmpresa
                            };
                            await _asignacionService.Insert(asignacion);
                            seguimiento.IdAsignacionArea = asignacion.IdAsignacionArea;
                            await _seguimientoService.Update(seguimiento);

                            if (Vmse.actividadesList != null)
                            {
                                foreach (var actividadDescripcion in Vmse.actividadesList)
                                {
                                    var actividad = new Actividade()
                                    {
                                        DescripcionActividad = actividadDescripcion,
                                        IdSeguimiento = seguimiento.IdSeguimiento
                                    };
                                    await _actividadService.Insert(actividad);
                                }
                            }

                            if (Vmse.observacionesList != null)
                            {
                                foreach (var observacion in Vmse.observacionesList)
                                {
                                    var observa = new Observacion()
                                    {
                                        Observaciones = observacion,
                                        IdSeguimiento = seguimiento.IdSeguimiento,
                                    };
                                    await _observacionesService.Insert(observa);
                                }
                            }

                            viewmodelsegui = new Viewmodelsegui()
                            {
                                seguimientoinstructorAprendiz = seguimiento,
                                asignacionArea = asignacion,
                            };

                            TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                        }
                    }
                    else if (Vmse.seguimientoinstructorAprendiz.IdModalidad == 3)
                    {
                        seguimiento = new SeguimientoInstructorAprendiz()
                        {
                            NumeroDocumentoAprendiz = Vmse?.aprendiz?.NumeroDocumentoAprendiz,
                            NumeroDocumentoInstructor = Vmse?.seguimientoinstructorAprendiz?.NumeroDocumentoInstructor,
                            FechaInicio = Vmse?.seguimientoinstructorAprendiz?.FechaInicio,
                            FechaFinalizacion = Vmse?.seguimientoinstructorAprendiz?.FechaFinalizacion,
                            FechaRealizacionSeguimiento = Vmse?.seguimientoinstructorAprendiz.FechaRealizacionSeguimiento,
                            IdModalidad = Vmse?.seguimientoinstructorAprendiz?.IdModalidad,
                            IdAreaEmpresa = Vmse?.seguimientoinstructorAprendiz?.IdAreaEmpresa,
                            NitProyecto = Vmse?.seguimientoinstructorAprendiz.NitProyecto,
                            NombreProyecto = Vmse?.seguimientoinstructorAprendiz?.NombreProyecto,
                            ObjetivoProyecto = Vmse?.seguimientoinstructorAprendiz?.ObjetivoProyecto
                        };
                        await _seguimientoService.Insert(seguimiento);
                        if (Vmse.actividadesList != null)
                        {
                            foreach (var actDesc in Vmse.actividadesList)
                            {
                                var actividades = new Actividade()
                                {
                                    DescripcionActividad = actDesc,
                                    IdSeguimiento = seguimiento.IdSeguimiento
                                };
                                await _actividadService.Insert(actividades);
                            }
                        }
                        if (Vmse.observacionesList != null)
                        {
                            foreach (var obsrv in Vmse.observacionesList)
                            {
                                var observa = new Observacion()
                                {
                                    Observaciones = obsrv,
                                    IdSeguimiento = seguimiento.IdSeguimiento,
                                };
                                await _observacionesService.Insert(observa);
                            }
                        }
                        viewmodelsegui = new Viewmodelsegui
                        {
                            seguimientoinstructorAprendiz = seguimiento,
                        };
                        TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
            }
            return View(viewmodelsegui);
        }

        //metodo para obtener los datos del seguimiento del aprendiz
        [HttpGet]
        public async Task<IActionResult> Consultar(string idSeguimiento)
        {
            Viewmodelsegui vmSeguimiento = new Viewmodelsegui();

            //obtener todos los aprendices de la bd
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelSeguimiento> listSeguimiento = new List<ViewModelSeguimiento>();
            Aprendiz aprendiz = null;
            var querySeguimiento = await _seguimientoService.GetAll();

            listSeguimiento = querySeguimiento.Select(s => new ViewModelSeguimiento(s)
            {
                IdSeguimiento = s.IdSeguimiento,
                FechaRealizacionSeguimiento = s.FechaRealizacionSeguimiento,
                FechaInicio = s.FechaInicio,
                FechaFinalizacion = s.FechaFinalizacion,
                NombreModalidad = s.IdModalidadNavigation.NombreModalidad,
                idmodalidad = s.IdModalidad,
                //datos del aprendiz
                idTipoDocumentoAprendiz = s.NumeroDocumentoAprendizNavigation.IdTipodocumento,
                NumeroDocumentoAprendiz = s.NumeroDocumentoAprendiz,
                NombreAprendiz = s.NumeroDocumentoAprendizNavigation.NombreAprendiz,
                ApellidoAprendiz = s.NumeroDocumentoAprendizNavigation.ApellidoAprendiz,
                CorreoAprendiz = s.NumeroDocumentoAprendizNavigation.CorreoAprendiz,
                TelefonoAprendiz = s.NumeroDocumentoAprendizNavigation.CelAprendiz,
                ProgramAprendiz = s.NumeroDocumentoAprendizNavigation.FichaNavigation.CodigoProgramaNavigation.NombrePrograma,
                FichaAprendiz = s.NumeroDocumentoAprendizNavigation.Ficha,
                //datos del instructor
                NumeroDocumentoInstructor = s.NumeroDocumentoInstructor,
                NombreInstructor = s.NumeroDocumentoInstructorNavigation.NombreInstructor,
                ApellidoInstructor = s.NumeroDocumentoInstructorNavigation.ApellidoInstructor,
                CorreoInstructor = s.NumeroDocumentoInstructorNavigation.CorreoInstructor,
                TelefonoInstructor = s.NumeroDocumentoInstructorNavigation.CelInstructor,
                //datos del coformador
                NumDocumentoCoformador = s.IdCoformadorNavigation.NumeroDocumentoCoformador,
                NombreCoformador = s.IdCoformadorNavigation.NombreCoformador,
                ApellidoCoformador = s.IdCoformadorNavigation.ApellidoCoformador,
                CorreoCoformador = s.IdCoformadorNavigation.CorreoCoformador,
                TelefonoCoformador = s.IdCoformadorNavigation.CelCoformador,
                //datos de la empresa
                NitEmpresa = s.NitEmpresa,
                NombreEmpresa = s.NitEmpresaNavigation.NombreEmpresa,
                AreaEmpresa = s.IdAreaEmpresaNavigation.NombreArea,
                IdAsignacionArea = s.IdAsignacionArea,
                IdAreaEmpresa = s.IdAreaEmpresa,
                //proyecto
                NitProyecto = s.NitProyecto,
                NombreProyecto = s.NombreProyecto,
                ObjetivoProyecto = s.ObjetivoProyecto
            }).ToList();

            ViewModelSeguimiento seguimient = listSeguimiento.Where(s => s.IdSeguimiento.ToString() == idSeguimiento).FirstOrDefault();
            //lista de observaciones de el seguimiento consultado
            var observaciones = _dbSiscanContext.Observacions.Where(o => o.IdSeguimiento.ToString() == idSeguimiento).Select(o => o.Observaciones).ToList();
            //lista de actividades de el seguimiento consultado
            var actividades = _dbSiscanContext.Actividades.Where(a => a.IdSeguimiento.ToString() == idSeguimiento).Select(a => a.DescripcionActividad).ToList();
            vmSeguimiento = new Viewmodelsegui
            {
                listaSeguimiento = listSeguimiento,
                seguimiento = seguimient,
                actividadesList = actividades,
                observacionesList = observaciones,
            };
            return View(vmSeguimiento);
        }
        //metodos para mostra los seguimientos guardados en el historial de el aprendiz obtenido
        [HttpGet]
        public async Task<IActionResult> MostrarHistorial(string nmDocAprendiz)
        {
            ModelViewSeguimientoArchivo vmSeguimiento = new ModelViewSeguimientoArchivo();
            try
            {
                if (nmDocAprendiz != null)
                {
                    //obtener la lista de los seguimientos que tiene el aprendiz con el numero de documento obtenido por parametro
                    var seguimientosArchv = await _seguimientoArchivoService.GetForDocAprendiz(nmDocAprendiz);
                    var seguimientos = await _seguimientoService.GetForNumDocAprdz(nmDocAprendiz);

                    List<ViewModelSeguiArchivoAprendiz> listSeguiArchivo = new List<ViewModelSeguiArchivoAprendiz>();
                    List<ViewModelSeguimiento> listSegui = new List<ViewModelSeguimiento>();
                    //obtener el aprendiz con el numero de documento obtenido por parametro
                    var aprendiz = await _aprendizService.GetForDoc(nmDocAprendiz);

                    if (seguimientos != null)
                    {
                        listSegui = seguimientos.Select(s => new ViewModelSeguimiento(s)
                        {
                            FechaInicio = s.FechaInicio,
                            FechaFinalizacion = s.FechaFinalizacion,
                            NombreModalidad = s.IdModalidadNavigation.NombreModalidad,
                            //datos del aprendiz                            
                            NumeroDocumentoAprendiz = s.NumeroDocumentoAprendiz,
                            NombreAprendiz = s.NumeroDocumentoAprendizNavigation.NombreAprendiz,
                            ApellidoAprendiz = s.NumeroDocumentoAprendizNavigation.ApellidoAprendiz,
                            CorreoAprendiz = s.NumeroDocumentoAprendizNavigation.CorreoAprendiz,
                            TelefonoAprendiz = s.NumeroDocumentoAprendizNavigation.CelAprendiz,
                            FichaAprendiz = s.NumeroDocumentoAprendizNavigation.Ficha,
                            //datos del instructor
                            NumeroDocumentoInstructor = s.NumeroDocumentoInstructor,
                            NombreInstructor = s.NumeroDocumentoInstructorNavigation.NombreInstructor,
                            ApellidoInstructor = s.NumeroDocumentoInstructorNavigation.ApellidoInstructor,
                            //datos del coformador
                            NumDocumentoCoformador = s.NumeroDocumentoInstructor,
                            NombreCoformador = s.IdCoformadorNavigation.NombreCoformador,
                            ApellidoCoformador = s.IdCoformadorNavigation.ApellidoCoformador,
                            //datos de la empresa
                            NitEmpresa = s.NitEmpresa,
                            NombreEmpresa = s.NitEmpresaNavigation.NombreEmpresa,
                            AreaEmpresa = s.IdAreaEmpresaNavigation.NombreArea,
                        }).ToList();
                    }

                    if (seguimientosArchv != null && seguimientosArchv.Count() > 0)
                    {
                        Coformador coformador = null;
                        foreach (var segui in seguimientosArchv)
                        {
                            //obtener instructor asignado al seguimiento
                            var instructor = _dbSiscanContext.Instructors.Find(segui.NumeroDocumentoInstructor);
                            //obtener coformador asignado al seguimiento
                            coformador = _dbSiscanContext.Coformadors.Where(c => c.NumeroDocumentoCoformador == segui.NumeroDocumentoCoformador).FirstOrDefault();
                            //obtener la empresa asignada al seguimiento
                            var empresa = await _empresaService.GetForNit(segui.NitEmpresa);
                            //obtener el area asignada al seguimiento
                            var areasEmpresa = _dbSiscanContext.AreasEmpresas.Find(segui.IdAreaEmpresa);
                            //obtener la modalidad del seguimiento
                            var modalidad = _dbSiscanContext.Modalidads.Find(segui.IdModalidad);

                            if (instructor != null && coformador != null && empresa != null && areasEmpresa != null && modalidad != null)
                            {
                                var seguimientoArchivo = new ViewModelSeguiArchivoAprendiz
                                {
                                    Area = areasEmpresa.NombreArea,
                                    Coformador = coformador,
                                    Empresa = empresa,
                                    Instructor = instructor,
                                    Modalidad = modalidad.NombreModalidad,
                                    FechaInicio = segui.FechaInicio,
                                    FechaFinalizacion = segui.FechaFinalizacion
                                };
                                listSeguiArchivo.Add(seguimientoArchivo);
                            }
                        }
                    }
                    vmSeguimiento = new ModelViewSeguimientoArchivo
                    {
                        Aprendiz = aprendiz,
                        listaSeguiArchivos = listSeguiArchivo,
                        listaSegui = listSegui
                    };
                }

            }
            catch
            {
                return NotFound();
            }
            return View(vmSeguimiento);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(long idSeguimiento)
        {
            try
            {
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.FirstOrDefaultAsync(s => s.IdSeguimiento == idSeguimiento);
                if (seguimiento == null)
                {
                    return Json(new { success = false, message = "El seguimiento no fue encontrado." });
                }
                var actividades = await _dbSiscanContext.Actividades.Where(a => a.IdSeguimiento == idSeguimiento).ToListAsync();
                _dbSiscanContext.Actividades.RemoveRange(actividades);
                var observaciones = await _dbSiscanContext.Observacions.Where(o => o.IdSeguimiento == idSeguimiento).ToListAsync();
                _dbSiscanContext.Observacions.RemoveRange(observaciones);
                await _seguimientoService.Delete(idSeguimiento);
                TempData["MensajeSeguimientoEliminado"] = "Seguimiento eliminado correctamente!!";
                return Json(new { success = true, message = "El seguimiento se elimino correctamente." });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Se produjo un error al intentas eliminar el seguimiento: " + ex.Message });
            }
        }
        //metodo para obtener los datos del seguimiento que se va editar
        [HttpGet]
        public async Task<IActionResult> EditarSeguimiento(long idSeguimiento)
        {
            var vmSeguimiento = new Viewmodelsegui();
            try
            {
                var seguimientos = await _seguimientoService.GetAll();
                SeguimientoInstructorAprendiz seguimiento = null;
                foreach (var seg in seguimientos)
                {
                    if (seg.IdSeguimiento == idSeguimiento)
                    {
                        seguimiento = seg;
                        break;
                    }
                }

                if (seguimiento != null)
                {
                    var aprendiz = await _aprendizService.GetForDoc(seguimiento.NumeroDocumentoAprendiz);
                    var tpcdoc = _dbSiscanContext.TipoDocumentos.Where(t => t.IdTipoDocumento == aprendiz.IdTipodocumento).Select(t => t.TipoDocumento1).FirstOrDefault();
                    if (aprendiz != null)
                    {
                        vmSeguimiento = new Viewmodelsegui
                        {
                            listaopcModalidad = _dbSiscanContext.Modalidads.Select(m => new SelectListItem
                            {
                                Value = m.IdModalidad.ToString(),
                                Text = m.NombreModalidad
                            }).ToList(),
                            listaopcAreaEmpre = _dbSiscanContext.AreasEmpresas.Select(a => new SelectListItem
                            {
                                Value = a.IdArea.ToString(),
                                Text = a.NombreArea
                            }).ToList(),
                            listaopcCoform = _dbSiscanContext.Coformadors.Select(c => new SelectListItem
                            {
                                Value = c.IdCoformador.ToString(),
                                Text = c.NombreCoformador + " " + c.ApellidoCoformador
                            }).ToList(),
                            seguimientoinstructorAprendiz = seguimiento,
                            aprendiz = aprendiz,
                            tipoDocAprendiz=tpcdoc
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return View(vmSeguimiento);
        }
        //metodo para editar el seguimiento obtenido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSeguimiento(Viewmodelsegui seguimientoVm)
        {
            SeguimientoArchivo seguimientoArchivo = null;
            Viewmodelsegui viewmodelsegui = new Viewmodelsegui();

            try
            {
                if (seguimientoVm.aprendiz.NumeroDocumentoAprendiz != null)
                {
                    if (!ModelState.IsValid)
                    {
                        var aprendiz = await _aprendizService.GetForDoc(seguimientoVm.aprendiz.NumeroDocumentoAprendiz);
                        string tpDocAprendiz = _dbSiscanContext.TipoDocumentos.Where(tp => tp.IdTipoDocumento == aprendiz.IdTipodocumento).Select(tp => tp.TipoDocumento1).FirstOrDefault();
                        viewmodelsegui = new Viewmodelsegui
                        {
                            listaopcModalidad = _dbSiscanContext.Modalidads.Select(m => new SelectListItem
                            {
                                Value = m.IdModalidad.ToString(),
                                Text = m.NombreModalidad
                            }).ToList(),
                            listaopcAreaEmpre = _dbSiscanContext.AreasEmpresas.Select(a => new SelectListItem
                            {
                                Value = a.IdArea.ToString(),
                                Text = a.NombreArea
                            }).ToList(),
                            listaopcCoform = _dbSiscanContext.Coformadors.Select(c => new SelectListItem
                            {
                                Value = c.IdCoformador.ToString(),
                                Text = c.NombreCoformador + " " + c.ApellidoCoformador
                            }).ToList(),
                            aprendiz = aprendiz,
                            tipoDocAprendiz = tpDocAprendiz
                        };
                        return View(nameof(EditarSeguimiento), viewmodelsegui);
                    }
                }

                if (seguimientoVm.seguimientoinstructorAprendiz != null)
                {


                    var instructor = await _instructorService.GetForDoc(seguimientoVm.seguimientoinstructorAprendiz.NumeroDocumentoInstructor);
                    var empresa = await _empresaService.GetForNit(seguimientoVm.seguimientoinstructorAprendiz.NitEmpresa);
                    var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(seguimientoVm.seguimientoinstructorAprendiz.IdSeguimiento);
                    if (seguimiento == null)
                    {
                        return NotFound();
                    }
                    if (seguimientoVm.seguimientoinstructorAprendiz.IdModalidad != 3)
                    {
                        if (empresa == null)
                        {
                            TempData["MensajeEmpresaNoExistSegui"] = "Nit de Empresa no encontrado";
                        }
                        if (instructor == null)
                        {
                            TempData["MensajeInstructorNoExistSegui"] = "No se encontro un instructor con este numero de documento";
                        }
                        else if (empresa != null && instructor != null && seguimiento != null)
                        {
                            //Obteniendo los datos del seguimiento para asigarlo a seguimiento archivo 
                            var coformador = _dbSiscanContext.Coformadors.Where(c => c.IdCoformador == seguimiento.IdCoformador).FirstOrDefault();
                            seguimientoArchivo = new SeguimientoArchivo
                            {
                                NumeroDocumentoAprendiz = seguimientoVm.aprendiz.NumeroDocumentoAprendiz,
                                NumeroDocumentoInstructor = seguimiento.NumeroDocumentoInstructor,
                                NumeroDocumentoCoformador = coformador.NumeroDocumentoCoformador,
                                FechaInicio = seguimiento.FechaInicio,
                                FechaFinalizacion = seguimiento.FechaFinalizacion,
                                IdModalidad = seguimiento.IdModalidad,
                                IdAsignacionArea = seguimiento.IdAsignacionArea,
                                IdAreaEmpresa = seguimiento.IdAreaEmpresa,
                                NitEmpresa = seguimiento.NitEmpresa
                            };

                            //Asignando los datos del segumiento actualizado
                            seguimiento.NumeroDocumentoAprendiz = seguimientoVm.aprendiz.NumeroDocumentoAprendiz;
                            seguimiento.NumeroDocumentoInstructor = seguimientoVm.seguimientoinstructorAprendiz.NumeroDocumentoInstructor;
                            seguimiento.IdCoformador = seguimientoVm.seguimientoinstructorAprendiz.IdCoformador;
                            seguimiento.FechaInicio = seguimientoVm.seguimientoinstructorAprendiz.FechaInicio;
                            seguimiento.FechaFinalizacion = seguimientoVm.seguimientoinstructorAprendiz.FechaFinalizacion;
                            seguimiento.FechaRealizacionSeguimiento = seguimientoVm.seguimientoinstructorAprendiz.FechaRealizacionSeguimiento;
                            seguimiento.IdModalidad = seguimientoVm.seguimientoinstructorAprendiz.IdModalidad;
                            seguimiento.IdAsignacionArea = seguimientoVm.seguimientoinstructorAprendiz.IdAsignacionArea;
                            seguimiento.IdAreaEmpresa = seguimientoVm.seguimientoinstructorAprendiz.IdAreaEmpresa;
                            seguimiento.NitEmpresa = seguimientoVm.seguimientoinstructorAprendiz.NitEmpresa;
                            seguimiento.NitProyecto = null;
                            seguimiento.NombreProyecto = null;
                            seguimiento.ObjetivoProyecto = null;

                            _dbSiscanContext.Update(seguimiento);
                            await _dbSiscanContext.SaveChangesAsync();
                            TempData["MensajeSeguimientoActualizado"] = "Seguimiento Actualizado correctamente!!";
                            if (seguimiento.IdModalidad != seguimientoArchivo.IdModalidad)
                            {
                                _dbSiscanContext.SeguimientoArchivos.Add(seguimientoArchivo);
                                await _dbSiscanContext.SaveChangesAsync();
                            }
                        }
                    }
                    else if (seguimientoVm.seguimientoinstructorAprendiz.IdModalidad == 3)
                    {
                        seguimientoArchivo = new SeguimientoArchivo
                        {
                            NumeroDocumentoAprendiz = seguimientoVm.aprendiz.NumeroDocumentoAprendiz,
                            NumeroDocumentoInstructor = seguimiento.NumeroDocumentoInstructor,
                            FechaInicio = seguimiento.FechaInicio,
                            FechaFinalizacion = seguimiento.FechaFinalizacion,
                            FechaRealizacionSeguimiento = seguimiento.FechaRealizacionSeguimiento,
                            IdModalidad = seguimiento.IdModalidad,
                            IdAreaEmpresa = seguimiento.IdAreaEmpresa,
                            NitProyecto = seguimiento.NitProyecto,
                            NombreProyecto = seguimiento.NombreProyecto,
                            ObjetivoProyecto = seguimiento.ObjetivoProyecto
                        };

                        //Asignando los datos del segumiento actualizado
                        seguimiento.NumeroDocumentoAprendiz = seguimientoVm.aprendiz.NumeroDocumentoAprendiz;
                        seguimiento.NumeroDocumentoInstructor = seguimientoVm.seguimientoinstructorAprendiz.NumeroDocumentoInstructor;
                        seguimiento.IdCoformador = null;
                        seguimiento.FechaInicio = seguimientoVm.seguimientoinstructorAprendiz.FechaInicio;
                        seguimiento.FechaFinalizacion = seguimientoVm.seguimientoinstructorAprendiz.FechaFinalizacion;
                        seguimiento.FechaRealizacionSeguimiento = seguimientoVm.seguimientoinstructorAprendiz.FechaRealizacionSeguimiento;
                        seguimiento.IdModalidad = seguimientoVm.seguimientoinstructorAprendiz.IdModalidad;
                        seguimiento.IdAsignacionArea = null;
                        seguimiento.IdAreaEmpresa = seguimientoVm.seguimientoinstructorAprendiz.IdAreaEmpresa;
                        seguimiento.NitEmpresa = null;
                        seguimiento.NitProyecto = seguimientoVm.seguimientoinstructorAprendiz.NitProyecto;
                        seguimiento.NombreProyecto = seguimientoVm.seguimientoinstructorAprendiz.NombreProyecto;
                        seguimiento.ObjetivoProyecto = seguimientoVm.seguimientoinstructorAprendiz.ObjetivoProyecto;

                        _dbSiscanContext.Update(seguimiento);
                        await _dbSiscanContext.SaveChangesAsync();
                        TempData["MensajeSeguimientoActualizado"] = "Seguimiento Actualizado correctamente!!";
                        if (seguimiento.IdModalidad != seguimientoArchivo.IdModalidad)
                        {
                            _dbSiscanContext.SeguimientoArchivos.Add(seguimientoArchivo);
                            await _dbSiscanContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguimientoExist(seguimientoVm.seguimientoinstructorAprendiz.IdSeguimiento)) return NotFound(); else throw;
            }

            return View(seguimientoVm);
        }
        //metodo para verificar si el seguimiento existe 
        private bool SeguimientoExist(long idSeguimiento)
        {
            return _dbSiscanContext.SeguimientoInstructorAprendizs.Any(s => s.IdSeguimiento == idSeguimiento);
        }
    }
}
