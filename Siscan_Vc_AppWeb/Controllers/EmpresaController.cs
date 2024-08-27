using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly DbSiscanContext _dbSiscanContext;
        private readonly IEmpresaService _empresaService;
        private readonly ISeguimientoService _seguimientoService;
        private readonly ICoformadorService _coformadorService;
        private readonly IAprendizService _aprendizService;
        public EmpresaController(DbSiscanContext dbSiscanContext, IEmpresaService empresaService, ISeguimientoService seguimientoService, ICoformadorService coformadorService, IAprendizService aprendizService)
        {
            _empresaService = empresaService;
            _dbSiscanContext = dbSiscanContext;
            _seguimientoService = seguimientoService;
            _coformadorService = coformadorService;
            _aprendizService = aprendizService;
        }

        [HttpGet]
        public async Task<IActionResult> CargarCiudades(int departamentoId)
        {
            var ciudades = await _dbSiscanContext.Ciudads.Where(c => c.IdDepartamento == departamentoId).ToListAsync();
            ViewBag.ciudades = ciudades;
            return Json(ciudades);
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var modelView = new ModelViewEmpresa
            {
                //lista departamentos
                listaOpcDepartamento = _dbSiscanContext.Departamentos.Select(d => new SelectListItem
                {
                    Value = d.IdDepartamento.ToString(),
                    Text = d.NombreDepartamento
                }).ToList(),
                //lista ciudades
                listaOpcCiudad = _dbSiscanContext.Ciudads.Select(d => new SelectListItem
                {
                    Value = d.IdCiudad.ToString(),
                    Text = d.NombreCiudad
                }).ToList()
            };
            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(ModelViewEmpresa empresaMv)
        {
            ModelViewEmpresa mVEmpresa = new ModelViewEmpresa();
            try
            {
                //registro de empresas
                if (empresaMv.empresa != null)
                {
                    if(empresaMv.empresa.Nitmpresa == null || empresaMv.empresa.TelefonoEmpresa == null || empresaMv.empresa.NombreEmpresa==null || empresaMv.empresa.RepresentanteLegal==null || empresaMv.empresa.DireccionEmpresa == null)
                    {
                        TempData["ValCamposVaciosEmpresa"] = "Por favor llene todos los campos";
                        return RedirectToAction(nameof(Registro));
                    }
                    var empreExist = await _empresaService.GetForNit(empresaMv.empresa.Nitmpresa);
                    if (empreExist != null)
                    {
                        TempData["ValEmpresaExist"] = "Ya existe una empresa registrada con este NIT";
                        return RedirectToAction(nameof(Registro));

                    }
                    else if (empreExist == null)
                    {
                        var empre = new Empresa()
                        {
                            Nitmpresa = empresaMv.empresa.Nitmpresa,
                            NombreEmpresa = empresaMv.empresa.NombreEmpresa,
                            RepresentanteLegal = empresaMv.empresa.RepresentanteLegal,
                            DireccionEmpresa = empresaMv.empresa.DireccionEmpresa,
                            TelefonoEmpresa = empresaMv.empresa.TelefonoEmpresa,
                            IdCiudad = empresaMv.empresa.IdCiudad
                        };
                        mVEmpresa.empresa = empre;
                        if (mVEmpresa.empresa.Nitmpresa != null)
                        {
                            _dbSiscanContext.Empresas.Add(empresaMv.empresa);
                            await _dbSiscanContext.SaveChangesAsync();
                            TempData["RegistroEmpresaExitoso"] = "Empresa registrada exitosamente";
                        }
                        return RedirectToAction(nameof(Registro));
                    }
                }

                //registro de coformador
                if (empresaMv.coformador != null)
                {
                    var coformadorExist = await _coformadorService.GetForDoc(empresaMv.coformador.NumeroDocumentoCoformador);
                    var empre = await _empresaService.GetForNit(empresaMv.coformador.NitEmpresa);
                    if (coformadorExist != null)
                    {
                        TempData["ValCoformadorExist"] = "Ya existe un coformador registrado con este numero de documento";
                        return RedirectToAction(nameof(Registro));
                    }
                    if (empre == null)
                    {
                        TempData["ValEmpresaNoExist"] = "la empresa con este nit no se encuentra registrada";
                    }
                    else if (coformadorExist == null && empre != null)
                    {
                        var coform = new Coformador
                        {
                            NombreCoformador = empresaMv.coformador.NombreCoformador,
                            ApellidoCoformador = empresaMv.coformador.ApellidoCoformador,
                            NumeroDocumentoCoformador = empresaMv.coformador.NumeroDocumentoCoformador,
                            CelCoformador = empresaMv.coformador.CelCoformador,
                            CorreoCoformador = empresaMv.coformador.CorreoCoformador,
                            NitEmpresa = empresaMv.coformador.NitEmpresa
                        };
                        mVEmpresa.coformador = coform;
                        if (mVEmpresa.coformador.NumeroDocumentoCoformador != null)
                        {
                            await _coformadorService.Insert(empresaMv.coformador);
                            TempData["RegistroCoformadorExitoso"] = "Coformador registrado exitosamente";
                        }
                        return RedirectToAction(nameof(Registro));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["registroEmpresaExcepcion"] = ex.Message;
            }
            return View(mVEmpresa);
        }

        [HttpGet]
        public async Task<IActionResult> consultar(string nitEmpresa)
        {
            var queryCoformador = await _coformadorService.GetAll();
            var listSeguimiento = await _seguimientoService.GetAll();
            List<Aprendiz> listAprendiz = new List<Aprendiz>();
            List<Coformador> listCoformador = new List<Coformador>();
            VMEmpresaAprendizCoformador vmEmpresa = new VMEmpresaAprendizCoformador();

            try
            {
                Empresa empresa = new Empresa();
                var empresas = await _empresaService.GetAll();
                if (nitEmpresa != null)
                {
                    foreach (var item in empresas)
                    {
                        if (item.Nitmpresa.Trim().ToLower() == nitEmpresa.Trim().ToLower())
                        {
                            empresa = item;
                        }
                    }
                }

                if (empresa.Nitmpresa == null)
                {
                    TempData["EmpresaNoExiste"] = "No se encontro una empresa empresa con este Nit";
                }
                foreach (var coformador in queryCoformador)
                {
                    if (coformador.NitEmpresa == nitEmpresa)
                    {
                        listCoformador.Add(coformador);
                    }
                }
                foreach (var seguimiento in listSeguimiento)
                {
                    if (seguimiento.NitEmpresa == nitEmpresa)
                    {
                        var aprendiz = await _aprendizService.GetForDoc(seguimiento.NumeroDocumentoAprendiz);
                        listAprendiz.Add(aprendiz);
                    }
                }
                var ciudad = _dbSiscanContext.Ciudads.Where(c => c.IdCiudad == empresa.IdCiudad).FirstOrDefault();
                if (ciudad != null)
                {
                    vmEmpresa = new VMEmpresaAprendizCoformador
                    {
                        empresa = empresa,
                        coformadores = listCoformador,
                        aprendices = listAprendiz,
                        nomCiudad = ciudad.NombreCiudad
                    };
                }

            }
            catch (Exception ex)
            {
                TempData["EmpresaExcepcionConsulta"] = "Error: " + ex.Message;
            }

            return View(vmEmpresa);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCoformador(string nmDocCoformador)
        {
            try
            {
                var coformador = await _dbSiscanContext.Coformadors.FirstOrDefaultAsync(c => c.NumeroDocumentoCoformador == nmDocCoformador);
                if (coformador == null)
                {
                    return Json(new { success = false, message = "El coformador no fue encontrado." });
                }
                TempData["MensajeAlertEliminadoCoformdr"] = "Coformador eliminado correctamente!!";
                var seguimiento = await _dbSiscanContext.SeguimientoInstructorAprendizs.Where(s => s.IdCoformador == coformador.IdCoformador).ToListAsync();
                _dbSiscanContext.SeguimientoInstructorAprendizs.RemoveRange(seguimiento);
                _dbSiscanContext.Coformadors.Remove(coformador);
                _dbSiscanContext.SaveChanges();
                return Json(new { success = true, message = "El coformador se elimino correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Se produjo un error al intentar eliminar el coformador: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarCoformador(string numDocCoformador)
        {
            Coformador coformador = new Coformador();

            if (numDocCoformador != null)
            {
                coformador = await _coformadorService.GetForDoc(numDocCoformador);
            }
            if (coformador == null)
            {
                return NotFound();
            }
            return View(coformador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCoformador(Coformador coformador)
        {
            try
            {
                if (coformador != null)
                {
                    var coform = await _coformadorService.GetForDoc(coformador.NumeroDocumentoCoformador);
                    var empresa = await _empresaService.GetForNit(coformador.NitEmpresa);
                    if (coform == null)
                    {
                        return NotFound();
                    }
                    if (empresa == null)
                    {
                        TempData["EmpresaNoExist"] = "No se encontro una empresa empresa con este Nit";
                    }
                    else
                    {
                        coform.NombreCoformador = coformador.NombreCoformador;
                        coform.ApellidoCoformador = coformador.ApellidoCoformador;
                        coform.CelCoformador = coformador.CelCoformador;
                        coform.CorreoCoformador = coformador.CorreoCoformador;
                        coform.NitEmpresa = coformador.NitEmpresa;

                        _dbSiscanContext.Update(coform);
                        _dbSiscanContext.SaveChanges();
                        TempData["ActualizaCoformdrExit"] = "Se actualizo correctamente";
                        return RedirectToAction(nameof(consultar));
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoformadorExist(coformador.NumeroDocumentoCoformador)) return NotFound(); else throw;
            }
            return View(coformador);
        }

        public bool CoformadorExist(string numDocCoformador)
        {
            return _dbSiscanContext.Coformadors.Any(c => c.NumeroDocumentoCoformador == numDocCoformador);
        }
    }
}
