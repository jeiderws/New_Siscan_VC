﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Siscan_Vc_AppWeb.Models.ViewModels;
using Siscan_Vc_BLL.Service;
using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Controllers
{
    public class AprendizController : Controller
    {
        private readonly IAprendizService _aprendizService;
        private readonly DbSiscanContext _dbSiscanContext;
        public AprendizController(IAprendizService aprendizService, DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
            _aprendizService = aprendizService;

        }
        //[HttpPost]
        public async Task<IActionResult> Registro(Aprendiz ap)
        {
            var itemsTipoDoc= await _dbSiscanContext.TipoDocumentos.ToListAsync();
            ViewBag.ItemsTipoDoc = itemsTipoDoc;
            //if (ModelState.IsValid)
            //{
            //    var kk = new Aprendiz()
            //    {
            //        NombreAprendiz = ap.NombreAprendiz,
            //        ApellidoAprendiz = ap.ApellidoAprendiz,
            //        NumeroDocumentoAprendiz = ap.NumeroDocumentoAprendiz,
            //        CorreoAprendiz  = ap.CorreoAprendiz,
            //        CelAprendiz = ap.CelAprendiz,
            //        IdTipodocumento = ap.IdTipodocumento,
            //        NombreCompletoAcudiente = ap.NombreCompletoAcudiente,
            //        CorreoAcuediente = ap.CorreoAcuediente,
            //        CelularAcudiente = ap.CelularAcudiente,
            //        Ficha = ap.Ficha,
            //        IdCiudad = ap.IdCiudad,
            //        IdEstadoAprendiz = ap.IdEstadoAprendiz,
            //        IdEstadoTyt = ap.IdEstadoTyt
            //    };
            //    _dbSiscanContext.Aprendiz.Add(ap);
            //    _dbSiscanContext.SaveChanges();
            //    return RedirectToAction("Exito");
            //}
            return View(/*"Registro", ap*/);
        }
        [HttpGet]
        public async Task<IActionResult> Consultar()
        {
            IQueryable<Aprendiz> queryAprendiz = await _aprendizService.GetAll();
            List<ViewModelAprendiz> listaAprendiz = queryAprendiz
                                                  .Select(a => new ViewModelAprendiz(a)
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
                                                    
                                                  }
                                                  ).ToList();

            //var aprendiz = _dbSiscanContext.Aprendiz;
            return View(listaAprendiz);
        }


        public IActionResult Editar()
        {
            return View();
        }
    
    }
}
