using Microsoft.AspNetCore.Components.Web;
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
        private readonly ISeguimientoArchivoService _seguimientoArchivoService;
        public SeguimientoController(DbSiscanContext dbSiscanContext, ISeguimientoArchivoService seguimientoArchivoService, IInstructorService instructorService, ISeguimientoService seguimientoService, IEmpresaService empresaService, IAprendizService aprendizService, IAsignacionService asignacionService)
        {
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _empresaService = empresaService;
            _aprendizService = aprendizService;
            _asignacionService = asignacionService;
            _instructorService = instructorService;
            _seguimientoArchivoService = seguimientoArchivoService;
        }
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

        [HttpGet]
        public async Task<IActionResult> Index(string numdoc)
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();

            List<ViewModelAprendiz> listaAprendices = new List<ViewModelAprendiz>();

            List<ViewModelAprendiz> listaAprendizSinSegui = new List<ViewModelAprendiz>();

            listaAprendices = queryAprendiz.Select(a => new ViewModelAprendiz(a)
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
                nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado,
                SeguimientoInstructorAprendices = a.SeguimientoInstructorAprendizs,
                NombreApellidoDoc = a.NombreAprendiz + " " + a.ApellidoAprendiz + " " + a.NumeroDocumentoAprendiz
            }).ToList();

            foreach (var ap in listaAprendices)
            {
                if (ap.SeguimientoInstructorAprendices.Count() == 0)
                {
                    listaAprendizSinSegui.Add(ap);
                }
            }
            var aprendi = await _aprendizService.GetForDoc(numdoc);
            var vmSeguimiento = new Viewmodelsegui
            {
                listaAprendizSinSegui = listaAprendizSinSegui,
                aprendiz = aprendi
            };
            return View(vmSeguimiento);
        }

        [HttpGet]
        public async Task<IActionResult> Crear(string nmDoc)
        {
            await LlenarCombos();
            var viewmodel = new Viewmodelsegui();
            if (nmDoc != null)
            {
                var segui = await _aprendizService.GetForDoc(nmDoc);
                viewmodel = new Viewmodelsegui
                {
                    aprendiz = segui
                };
                if (viewmodel.aprendiz == null)
                {
                    return NotFound();
                }
            }
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Viewmodelsegui Vmse)
        {
            await LlenarCombos();
            Viewmodelsegui viewmodelsegui = new Viewmodelsegui();
            try
            {
                if (Vmse != null)
                {
                    var seguimiento = new SeguimientoInstructorAprendiz()
                    {
                        NumeroDocumentoAprendiz = Vmse?.aprendiz?.NumeroDocumentoAprendiz,
                        NumeroDocumentoInstructor = Vmse?.seguimientoinstructorAprendiz?.NumeroDocumentoInstructor,
                        IdCoformador = Vmse?.seguimientoinstructorAprendiz?.IdCoformador,
                        FechaInicio = Vmse?.seguimientoinstructorAprendiz?.FechaInicio,
                        FechaFinalizacion = Vmse?.seguimientoinstructorAprendiz?.FechaFinalizacion,
                        IdModalidad = Vmse?.seguimientoinstructorAprendiz?.IdModalidad,
                        IdAreaEmpresa = Vmse?.seguimientoinstructorAprendiz?.IdAreaEmpresa,
                        NitEmpresa = Vmse?.seguimientoinstructorAprendiz.NitEmpresa
                    };
                    var asignacion = new AsignacionArea()
                    {
                        IdArea = seguimiento.IdAreaEmpresa,
                        NitEmpresa = seguimiento.NitEmpresa
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
                        await _asignacionService.Insert(asignacion);
                        seguimiento.IdAsignacionArea = asignacion.IdAsignacionArea;
                        await _seguimientoService.Insert(seguimiento);
                        viewmodelsegui = new Viewmodelsegui()
                        {
                            seguimientoinstructorAprendiz = seguimiento,
                            asignacionArea = asignacion
                        };
                        TempData["MensajeAlertSegui"] = "Seguimiento Registrado";
                        return View(viewmodelsegui);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(viewmodelsegui);
        }

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
                FechaInicio = s.FechaInicio,
                FechaFinalizacion = s.FechaFinalizacion,
                NombreModalidad = s.IdModalidadNavigation.NombreModalidad,
                idmodalidad = s.IdModalidad,
                //datos del aprendiz
                idTipoDocumentoAprendiz = s.NumeroDocumentoAprendizNavigation.IdTipodocumento,
                //TipoDocumentoAprendiz = s.NumeroDocumentoAprendizNavigation.IdTipodocumentoNavigation.TipoDocumento1,
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
                NumDocumentoCoformador = s.NumeroDocumentoInstructor,
                NombreCoformador = s.IdCoformadorNavigation.NombreCoformador,
                ApellidoCoformador = s.IdCoformadorNavigation.ApellidoCoformador,
                CorreoCoformador = s.IdCoformadorNavigation.CorreoCoformador,
                TelefonoCoformador = s.IdCoformadorNavigation.CelCoformador,
                //datos de la empresa
                NitEmpresa = s.NitEmpresa,
                NombreEmpresa = s.NitEmpresaNavigation.NombreEmpresa,
                AreaEmpresa = s.IdAreaEmpresaNavigation.NombreArea,
                IdAsignacionArea = s.IdAsignacionArea,
                IdAreaEmpresa = s.IdAreaEmpresa
            }).ToList();

            ViewModelSeguimiento seguimient = null;

            foreach (var segui in listSeguimiento)
            {
                if (segui.IdSeguimiento.ToString() == idSeguimiento)
                {
                    seguimient = segui;
                    break;
                }
            }
            vmSeguimiento = new Viewmodelsegui
            {
                listaSeguimiento = listSeguimiento,
                seguimiento = seguimient
            };

            return View(vmSeguimiento);
        }

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
                    var seguimiento = await _seguimientoService.GetForNumDocAprdz(nmDocAprendiz);

                    List<ViewModelSeguiArchivoAprendiz> listSeguiArchivo = new List<ViewModelSeguiArchivoAprendiz>();
                    List<ViewModelSeguimiento> listSegui = new List<ViewModelSeguimiento>();
                    //obtener el aprendiz con el numero de documento obtenido por parametro
                    var aprendiz = await _aprendizService.GetForDoc(nmDocAprendiz);

                    if (seguimiento != null)
                    {
                        listSegui = seguimiento.Select(s => new ViewModelSeguimiento(s)
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

                    if (seguimientosArchv != null)
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
                        listaSegui=listSegui
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

                await _seguimientoService.Delete(idSeguimiento);
                TempData["MensajeSeguimientoEliminado"] = "Seguimiento eliminado correctamente!!";

                return Json(new { success = true, message = "El seguimiento se elimino correctamente." });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "Se produjo un error al intentas eliminar el seguimiento: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarSeguimiento(long idSeguimiento)
        {
            await LlenarCombos();
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
                    if (aprendiz != null)
                    {
                        vmSeguimiento = new Viewmodelsegui
                        {
                            seguimientoinstructorAprendiz = seguimiento,
                            aprendiz = aprendiz,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSeguimiento(Viewmodelsegui seguimientoVm)
        {
            SeguimientoArchivo seguimientoArchivo = null;
            await LlenarCombos();
            if (seguimientoVm.seguimientoinstructorAprendiz != null)
            {
                try
                {
                    var instructor = await _instructorService.GetForDoc(seguimientoVm.seguimientoinstructorAprendiz.NumeroDocumentoInstructor);
                    var empresa = await _empresaService.GetForNit(seguimientoVm.seguimientoinstructorAprendiz.NitEmpresa);
                    var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(seguimientoVm.seguimientoinstructorAprendiz.IdSeguimiento);
                    if (seguimiento == null)
                    {
                        return NotFound();
                    }
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
                        seguimientoArchivo = new SeguimientoArchivo
                        {
                            NumeroDocumentoAprendiz = seguimientoVm.aprendiz.NumeroDocumentoAprendiz,
                            NumeroDocumentoInstructor = seguimiento.NumeroDocumentoInstructor,
                            NumeroDocumentoCoformador = seguimiento.IdCoformadorNavigation.NumeroDocumentoCoformador,
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
                        seguimiento.IdModalidad = seguimientoVm.seguimientoinstructorAprendiz.IdModalidad;
                        seguimiento.IdAsignacionArea = seguimientoVm.seguimientoinstructorAprendiz.IdAsignacionArea;
                        seguimiento.IdAreaEmpresa = seguimientoVm.seguimientoinstructorAprendiz.IdAreaEmpresa;
                        seguimiento.NitEmpresa = seguimientoVm.seguimientoinstructorAprendiz.NitEmpresa;

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
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguimientoExist(seguimientoVm.seguimientoinstructorAprendiz.IdSeguimiento)) return NotFound(); else throw;
                }
            }
            return View(seguimientoVm);
        }
        private bool SeguimientoExist(long idSeguimiento)
        {
            return _dbSiscanContext.SeguimientoInstructorAprendizs.Any(s => s.IdSeguimiento == idSeguimiento);
        }
    }
}
