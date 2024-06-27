using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewProgra
    {
        public int opcseleccionadaNivel { get; set; }
        public List<SelectListItem> listaopcNivel { get; set; }
        public int opcseleccionadaEstado { get; set; }
        public List<SelectListItem> listaopcEstado { get; set; }

        public Programas programas { get; set; }
        public List<ViewModelPrograma> listaprogramas { get; set; }

        public Ficha ficha { get; set; }
        public List<ViewModelFicha> listaFicha { get;set; }

    }
}
