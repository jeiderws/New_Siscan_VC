using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.ClasesService;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> CargarCiudades(int departamentoId)
        {
            var ciudades = await _dbSiscanContext.Ciudads.Where(c => c.IdDepartamento == departamentoId).ToListAsync();
            ViewBag.ciudades = ciudades;
            return Json(ciudades);

        }
        [HttpGet]
        public async Task<IActionResult> Cargarprograma(int programa)
        {
            var ficha = await _dbSiscanContext.Fichas.Where(f => f.CodigoPrograma == programa.ToString()).ToListAsync();
            ViewBag.ficha = ficha;
            return Json(ficha);

        }
        public async Task<IActionResult> Registro()
        {
            var itemsTipoDoc = await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;
          
            var itemsEstAprndz = await _dbSiscanContext.EstadoAprendizs.ToListAsync();
            ViewBag.ItemsEstAprndz = itemsEstAprndz;
            var itemsDepartamento = await _dbSiscanContext.Departamentos.ToListAsync();
            ViewBag.ItemsDepartamento = itemsDepartamento;
            ViewBag.ciudades = new List<Ciudad>();
         
            var itemsEstaTYT = await _dbSiscanContext.EstadoInscripcionTyts.ToListAsync();
            ViewBag.ItemsEstaTYT = itemsEstaTYT;
            var itemsPrograma = await _dbSiscanContext.Programas.ToListAsync();
            ViewBag.ItemsPrograma = itemsPrograma;
             ViewBag.ficha = new List<Ficha>();


            return View();


           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Aprendiz ap, InscripcionTyt intyt)
        {
            if (ModelState.IsValid)
            {
                var aprendiz = new Aprendiz()
                {
                    IdTipodocumento = ap.IdTipodocumento,
                    NumeroDocumentoAprendiz = ap.NumeroDocumentoAprendiz,
                    NombreAprendiz = ap.NombreAprendiz,
                    ApellidoAprendiz = ap.ApellidoAprendiz,
                    CelAprendiz = ap.CelAprendiz,

                    DireccionAprendiz = ap.DireccionAprendiz,
                    CorreoAprendiz = ap.CorreoAprendiz,
                    IdEstadoAprendiz = ap.IdEstadoAprendiz,
                    IdCiudad = ap.IdCiudad,
                    IdEstadoTyt = ap.IdEstadoTyt,
                    Ficha = ap.Ficha,

                    NombreCompletoAcudiente = ap.NombreCompletoAcudiente,

                    CelularAcudiente = ap.CelularAcudiente,
                    CorreoAcuediente = ap.CorreoAcuediente
                };
                _dbSiscanContext.Aprendiz.Add(aprendiz);
                await _dbSiscanContext.SaveChangesAsync();
                //return Json(new { success = true });
                var tyt = new InscripcionTyt()
                {
                    CodigoInscripcion = intyt.CodigoInscripcion,
                    NumeroDocumentoAprendiz = intyt.NumeroDocumentoAprendizNavigation.NumeroDocumentoAprendiz,
                    Idciudad = intyt.Idciudad,
                    IdConvocatoria = intyt.IdConvocatoria,
                    IdEstadotyt = intyt.IdEstadotyt,
               };

                TempData["MensajeAlert"] = "Usuario gurdado correctamente";
                _aprendizService.Insert(aprendiz);
                _inscripcionTYTService.Insert(tyt);
                return RedirectToAction(nameof(Registro));
            }
            Modelviewtytap  vmtytap = new Modelviewtytap
            {
                Aprendiz = ap,
                inscripcionTyt = intyt
            };


            return View(ap);
        }

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
            foreach(var aprendi in queryAprendiz)
            {
                if (aprendi.NumeroDocumentoAprendiz == nmdoc)
                {
                    aprendiz = aprendi;
                }
            }
            //ViewModelAprendiz aprendiz = listaAprendiz.Where(a => a.NombreAprendiz == name).FirstOrDefault();

            ModelviewAp viewModel = new ModelviewAp
            {
                
                Aprendiz = aprendiz,
                ListaAprendices = listaAprendiz
            };

            return View(viewModel);

            //IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            //List<ViewModelAprendiz> listaAprendiz = queryAprendiz
            //                                      .Select(a => new ViewModelAprendiz(a)
            //                                      {
            //                                          NumeroDocumentoAprendiz = a.NumeroDocumentoAprendiz,
            //                                          NombreAprendiz = a.NombreAprendiz,
            //                                          ApellidoAprendiz = a.ApellidoAprendiz,
            //                                          CelAprendiz = a.CelAprendiz,
            //                                          CorreoAprendiz = a.CorreoAprendiz,
            //                                          DireccionAprendiz = a.DireccionAprendiz,
            //                                          NombreCompletoAcudiente = a.NombreCompletoAcudiente,
            //                                          CorreoAcuediente = a.CorreoAcuediente,
            //                                          CelularAcudiente = a.CelularAcudiente,
            //                                          IdEstadoTyt = a.IdEstadoTytNavigation.IdEstadotyt,
            //                                          nomEstadoTyt = a.IdEstadoTytNavigation.DescripcionEstadotyt,
            //                                          IdTipodocumento = a.IdTipodocumentoNavigation.IdTipoDocumento,
            //                                          nombredoc = a.IdTipodocumentoNavigation.TipoDocumento1,
            //                                          Ficha = a.Ficha,
            //                                          IdCiudad = a.IdCiudad,
            //                                          IdEstadoAprendiz = a.IdEstadoAprendiz,
            //                                          nomEstadoAprendiz = a.IdEstadoAprendizNavigation.NombreEstado

            //                                      }
            //                                      ).ToList();

            ////var aprendiz = _dbSiscanContext.Aprendiz;
            //return View(listaAprendiz);
        }



        public IActionResult Editar()
        {
            return View();
        }

    }
}
