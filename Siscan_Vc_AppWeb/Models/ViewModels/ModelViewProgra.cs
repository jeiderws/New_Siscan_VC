using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewProgra
    {
        [Required(ErrorMessage = "Por Favor Seleccione un Nivel Para el  Programa")]
        public int? opcseleccionadaNivel { get; set; }
        public List<SelectListItem>? listaopcNivel { get; set; }

        [Required(ErrorMessage = "Por Favor Seleccione un Estado Para el  Programa")]
        public int? opcseleccionadaEstado { get; set; }
        public List<SelectListItem>? listaopcEstado { get; set; }

        public int? opcseleccionadaSede { get; set; }
        public List<SelectListItem>? listaopcSede { get; set; }

        public Programas programas { get; set; }
        public List<ViewModelPrograma>? listaprogramas { get; set; }

        public Ficha? ficha { get; set; }
        public List<ViewModelFicha>? listaFicha { get;set; }

    }
}
