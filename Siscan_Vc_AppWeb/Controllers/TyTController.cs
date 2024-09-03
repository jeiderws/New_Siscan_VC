using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class TyTController : Controller
    {
        private readonly DbSiscanContext _dbcontext;
        private readonly IAprendizService _aprendizService;
        public TyTController(DbSiscanContext dbSiscanContext, IAprendizService aprendizService)
        {
            _dbcontext = dbSiscanContext;
        }
        [HttpGet]
        public IActionResult Registro()
        {
            var viewModel = new Modelviewtytap();
            return View(viewModel);
        }
        //regitra las fechas de la convocatoria
        [HttpPost]
        public IActionResult Registro(Modelviewtytap viewModel)
        {
            if (viewModel.FechaPresentacionSem1 != null || viewModel.FechaPresentacionSem2 != null)
            {
                var semestre1 = _dbcontext.ConvocatoriaTyts.FirstOrDefault(c => c.SemestreConvocatoria == "Semestre 1");
                var semestre2 = _dbcontext.ConvocatoriaTyts.FirstOrDefault(c => c.SemestreConvocatoria == "Semestre 2");

                if (semestre1 != null && viewModel.FechaPresentacionSem1!=null)
                {
                    semestre1.FechaPresentacion = viewModel.FechaPresentacionSem1;
                }

                if (semestre2 != null && viewModel.FechaPresentacionSem2!=null)
                {
                    semestre2.FechaPresentacion = viewModel.FechaPresentacionSem2;
                }
                

                _dbcontext.SaveChanges();
                TempData["MensajeAlertFecha"] = "Fecha de Presentacion De Prueba Registrada";
            }

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Consultar()
        {
            return View(new TyTConsultationViewModel());
        }
        [HttpPost]
        public IActionResult ConsultarResult(TyTConsultationViewModel viewModel)
        {
            if (viewModel != null)
            {
                var aprendices = (from a in _dbcontext.Aprendiz
                                  join i in _dbcontext.InscripcionTyts on a.NumeroDocumentoAprendiz equals i.NumeroDocumentoAprendiz into ai
                                  from i in ai.DefaultIfEmpty()
                                  join c in _dbcontext.ConvocatoriaTyts on i.IdConvocatoria equals c.IdConvocatoria
                                  join f in _dbcontext.Fichas on a.Ficha equals f.Ficha1
                                  where f.Ficha1 == viewModel.FichaId && c.SemestreConvocatoria == viewModel.SemestreConvocatoria && a.IdEstadoTyt == 1
                                  select new AprendizViewModel
                                  {
                                      NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                                      NombreAprendiz = a.NombreAprendiz,
                                      ApellidoAprendiz = a.ApellidoAprendiz,
                                      CelAprendiz = a.CelAprendiz,
                                      CorreoAprendiz = a.CorreoAprendiz,
                                      DireccionAprendiz = a.DireccionAprendiz,
                                      NombreCompletoAcudiente = a.NombreCompletoAcudiente,
                                      CorreoAcudiente = a.CorreoAcuediente,
                                      CelularAcudiente = a.CelularAcudiente,
                                      EstadoTytNombre = a.IdEstadoTytNavigation.DescripcionEstadotyt,
                                      TipoDocumentoNombre = a.IdTipodocumentoNavigation.TipoDocumento1,
                                      Ficha = a.Ficha,
                                      CiudadNombre = a.IdCiudadNavigation.NombreCiudad,
                                      EstadoAprendizNombre = a.IdEstadoAprendizNavigation.NombreEstado,
                                      CodigoInscripcion = i.CodigoInscripcion,
                                      HasCompletedTyt = i.CodigoInscripcion != null,
                                      ChangeStatusToRealizadas = viewModel.ChangeStatusToRealizadas
                                  }).ToList();

                viewModel.Aprendices = aprendices;
            }

            return View("Consultar", viewModel);
        }
        //metodo para lo del checkbox
        [HttpPost]
        public IActionResult UpdateStatus(string[] selectedAprendices)
        {
            if (selectedAprendices != null && selectedAprendices.Length > 0)
            {
                foreach (var numeroDocumento in selectedAprendices)
                {
                    var apprenticeEntity = _dbcontext.Aprendiz.FirstOrDefault(a => a.NumeroDocumentoAprendiz == numeroDocumento);
                    if (apprenticeEntity != null)
                    {
                        apprenticeEntity.IdEstadoTyt = 6; // Cambiar estado a realizadas
                    }
                }
                _dbcontext.SaveChanges();
                TempData["MensajeAlertIns"] = "Se Actualizo el estado TYT de los Aprendices";
            }
            return RedirectToAction("Consultar");
        }
        public async Task<IActionResult> TyTPresentadas(TyTConsultationViewModel viewModel)
        {
            if (viewModel != null)
            {
                var aprendices = (from a in _dbcontext.Aprendiz
                                  join i in _dbcontext.InscripcionTyts on a.NumeroDocumentoAprendiz equals i.NumeroDocumentoAprendiz into ai
                                  from i in ai.DefaultIfEmpty()
                                  join c in _dbcontext.ConvocatoriaTyts on i.IdConvocatoria equals c.IdConvocatoria
                                  join f in _dbcontext.Fichas on a.Ficha equals f.Ficha1
                                  where a.IdEstadoTyt == 1
                                  select new AprendizViewModel
                                  {
                                      NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
                                      NombreAprendiz = a.NombreAprendiz,
                                      ApellidoAprendiz = a.ApellidoAprendiz,
                                      CelAprendiz = a.CelAprendiz,
                                      CorreoAprendiz = a.CorreoAprendiz,
                                      DireccionAprendiz = a.DireccionAprendiz,
                                      NombreCompletoAcudiente = a.NombreCompletoAcudiente,
                                      CorreoAcudiente = a.CorreoAcuediente,
                                      CelularAcudiente = a.CelularAcudiente,
                                      EstadoTytNombre = a.IdEstadoTytNavigation.DescripcionEstadotyt,
                                      TipoDocumentoNombre = a.IdTipodocumentoNavigation.TipoDocumento1,
                                      Ficha = a.Ficha,
                                      CiudadNombre = a.IdCiudadNavigation.NombreCiudad,
                                      EstadoAprendizNombre = a.IdEstadoAprendizNavigation.NombreEstado,
                                      CodigoInscripcion = i.CodigoInscripcion,
                                      HasCompletedTyt = i.CodigoInscripcion != null,
                                      ChangeStatusToRealizadas = viewModel.ChangeStatusToRealizadas
                                  }).ToList();

                viewModel.Aprendices = aprendices;
            }

            return View( viewModel);
        }


    }
}
