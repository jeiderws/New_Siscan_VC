using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.ClasesService;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using System.Data;
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
        public async Task LlenarCombos()
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

            var itemConvocatoria = await _dbSiscanContext.ConvocatoriaTyts.ToListAsync();
            ViewBag.ItemsConvocatoria = itemConvocatoria;

        }
        //Llenar combos
        public async Task<IActionResult> Registro()
        {

            await LlenarCombos();
            return View();



        }

        //Registrar aprendiz con un view model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Modelviewtytap aptyt)
        {
            Modelviewtytap vmtytap = new Modelviewtytap();
            if (vmtytap != null)
            {
                var aprendiz = new Aprendiz()
                {
                    IdTipodocumento = aptyt.aprendiz.IdTipodocumento,
                    NumeroDocumentoAprendiz = aptyt.aprendiz.NumeroDocumentoAprendiz,
                    NombreAprendiz = aptyt.aprendiz.NombreAprendiz,
                    ApellidoAprendiz = aptyt.aprendiz.ApellidoAprendiz,
                    CelAprendiz = aptyt.aprendiz.CelAprendiz,

                    DireccionAprendiz = aptyt.aprendiz.DireccionAprendiz,
                    CorreoAprendiz = aptyt.aprendiz.CorreoAprendiz,
                    IdEstadoAprendiz = aptyt.aprendiz.IdEstadoAprendiz,
                    IdCiudad = aptyt.aprendiz.IdCiudad,
                    IdEstadoTyt = aptyt.aprendiz.IdEstadoTyt,
                    Ficha = aptyt.aprendiz.Ficha,

                    NombreCompletoAcudiente = aptyt.aprendiz.NombreCompletoAcudiente,

                    CelularAcudiente = aptyt.aprendiz.CelularAcudiente,
                    CorreoAcuediente = aptyt.aprendiz.CorreoAcuediente
                };
                await _aprendizService.Insert(aprendiz);
                if (aprendiz.NumeroDocumentoAprendiz == aptyt.aprendiz.NumeroDocumentoAprendiz && aprendiz.IdEstadoTyt == 1)
                {
                    var tyt = new InscripcionTyt()
                    {
                        CodigoInscripcion = aptyt.inscripcionTyt.CodigoInscripcion,
                        NumeroDocumentoAprendiz = aprendiz.NumeroDocumentoAprendiz,
                        Idciudad = aptyt.inscripcionTyt.Idciudad,
                        IdConvocatoria = aptyt.inscripcionTyt.IdConvocatoria,
                        IdEstadotyt = aprendiz.IdEstadoTyt
                    };
                    _dbSiscanContext.InscripcionTyts.Add(tyt);
                    _dbSiscanContext.SaveChanges();
                }


                TempData["MensajeAlert"] = "Usuario guardado correctamente";

                vmtytap = new Modelviewtytap
                {
                    aprendiz = aptyt.aprendiz,
                    inscripcionTyt = aptyt.inscripcionTyt
                };

                return RedirectToAction(nameof(Registro));


            }
            return View(vmtytap);
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
                }
            }


            ModelviewAp viewModel = new ModelviewAp
            {

                Aprendiz = aprendiz,
                ListaAprendices = listaAprendiz
            };

            return View(viewModel);

        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(string nmdoc)
        {
            try
            {
                var a = await _dbSiscanContext.Aprendiz.FirstOrDefaultAsync(x => x.NumeroDocumentoAprendiz == nmdoc);

                if (a == null)
                {
                    return Json(new { success = false, message = "El aprendiz no fue encontrado." });
                }
                TempData["MensajeAlertEliminado"] = "Usuario eliminado correctamente";
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
            await LlenarCombos();
            var num = numDoc;
            var viewModel = new Modelviewtytap();
            if (numDoc != null)
            {
                //TempData["numDoc"] = numDoc;
              var  aprendi=await _aprendizService.GetForDoc(numDoc);

                InscripcionTyt insctyt;
                //if (aprendi.NumeroDocumentoAprendiz == null)
                //{
                //    return NotFound();
                //}
                //if (aprendi == null)
                //{
                //    return NotFound();
                //}
                if (aprendi.IdEstadoTyt == 1)
                {
                    insctyt = _dbSiscanContext.InscripcionTyts.First(i => i.NumeroDocumentoAprendiz == aprendi.NumeroDocumentoAprendiz);
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
        public async Task<IActionResult> Editar(Modelviewtytap aprendiztyt)
        {
            InscripcionTyt insctyt;
            if (!ModelState.IsValid)
            {
                return View(aprendiztyt);
            }
            var aprendiz = await _aprendizService.GetForDoc(aprendiztyt.aprendiz.NumeroDocumentoAprendiz);

            //Validar si el aprendiz esta inscrito
            if (aprendiz.IdEstadoTyt == 1)
            {
                insctyt = _dbSiscanContext.InscripcionTyts.First(i => i.NumeroDocumentoAprendiz == aprendiz.NumeroDocumentoAprendiz);
            }
            if (aprendiz == null)
            {
                return NotFound();
            }

            //asignar los datos obtenidos por parametros a los objetos que se guardaran
            aprendiz = aprendiztyt.aprendiz;
            insctyt = aprendiztyt.inscripcionTyt;

            try
            {
                //actualizacion de registros
                await _aprendizService.Update(aprendiz);
                await _inscripcionTYTService.Update(insctyt);
            }
            catch (DbUpdateConcurrencyException)
            {
                //validacion de existencia del aprendiz
                if (!AprendizExists(aprendiztyt.aprendiz.NumeroDocumentoAprendiz))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Consultar));

            //if (numDocumento != aprendiztyt.aprendiz.NumeroDocumentoAprendiz)
            //{
            //    return NotFound();
            //}
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        await _aprendizService.Update(aprendiztyt.aprendiz);
            //        if (aprendiztyt.inscripcionTyt.CodigoInscripcion != null)
            //        {
            //            await _inscripcionTYTService.Update(aprendiztyt.inscripcionTyt);
            //        }
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!AprendizExists(aprendiztyt.aprendiz.NumeroDocumentoAprendiz))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Consultar));
            //}
            //return View(aprendiztyt);
        }
        private bool AprendizExists(string numeroDocumento)
        {
            return _dbSiscanContext.Aprendiz.Any(a => a.NumeroDocumentoAprendiz == numeroDocumento);
        }

    }
}
