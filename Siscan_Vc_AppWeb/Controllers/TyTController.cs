using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class TyTController : Controller
    {
        private readonly DbSiscanContext _dbcontext;
        public TyTController(DbSiscanContext dbSiscanContext)
        {
            _dbcontext = dbSiscanContext;
        }
        [HttpGet]
        public IActionResult Registro()
        {
            var viewModel = new Modelviewtytap();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Registro(Modelviewtytap viewModel)
        {
            if (viewModel != null)
            {
                var semestre1 = _dbcontext.ConvocatoriaTyts.FirstOrDefault(c => c.SemestreConvocatoria == "Semestre 1");
                var semestre2 = _dbcontext.ConvocatoriaTyts.FirstOrDefault(c => c.SemestreConvocatoria == "Semestre 2");

                if (semestre1 != null)
                {
                    semestre1.FechaPresentacion = viewModel.FechaPresentacionSem1;
                }

                if (semestre2 != null)
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
                                  where f.Ficha1 == viewModel.FichaId && c.SemestreConvocatoria == viewModel.SemestreConvocatoria
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
                                      IdEstadoTyt = a.IdEstadoTyt,
                                      IdTipodocumento = a.IdTipodocumento,
                                      Ficha = a.Ficha,
                                      IdCiudad = a.IdCiudad,
                                      IdEstadoAprendiz = a.IdEstadoAprendiz,
                                      CodigoInscripcion = i.CodigoInscripcion,
                                      HasCompletedTyt = i.CodigoInscripcion != null,
                                      ChangeStatusToRealizadas = viewModel.ChangeStatusToRealizadas
                                  }).ToList();
                if (viewModel.ChangeStatusToRealizadas)
                {
                    foreach (var aprendiz in aprendices)
                    {
                        if (aprendiz.IdEstadoTyt == 1) 
                        {
                            var apprenticeEntity = _dbcontext.Aprendiz.FirstOrDefault(a => a.NumeroDocumentoAprendiz == aprendiz.NumeroDocumentoAprendiz);
                            if (apprenticeEntity != null)
                            {
                                apprenticeEntity.IdEstadoTyt = 6; 
                            }
                        }
                    }
                    _dbcontext.SaveChanges();
                }

                viewModel.Aprendices = aprendices;
            }

            return View("Consultar", viewModel);
        }
    }
}
