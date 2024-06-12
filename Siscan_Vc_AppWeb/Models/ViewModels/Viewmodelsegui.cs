using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class Viewmodelsegui
    {
        public string opcSeleccionadaAprendizSeguimiento { get; set; }
        public string opcseleccionadaEmpre {  get; set; }
        public string opcseleccionadaAreaEmpre {  get; set; }
        public int opcseleccionadaCoform {  get; set; }
        public int opcseleccionadaModalidad {  get; set; }
        public List<SelectListItem> listaopcempresa { get; set; }
        public List<SelectListItem> listaopcAreaEmpre { get; set; }
        public List<SelectListItem> listaopcCoform { get; set; }
        public List<SelectListItem> listaopcModalidad { get; set; }
        public List<SelectListItem> listaopctpdoc { get; set; }

        //Listas y objetos
        public SeguimientoInstructorAprendiz seguimientoinstructorAprendiz {  get; set; }
        public List<ViewModelSeguimiento> listaSeguimiento { get; set; }
        public List<Empresa> listaEmpresa { get; set; }
        public Empresa Empresa { get; set; }
        public List<ViewModelAprendiz> listaAprendizSinSegui { get; set; }
        public List<ViewModelAprendiz> listaAprendizSegui { get; set; }
        public AsignacionArea asignacionArea { get; set; }
        public ViewModelAprendiz aprendiz { get; set; }
        
        public List<ViewModelAprendiz> listaAprendizSinSegui { get; set; }
        public AsignacionArea asignacionArea { get; set; }
        public Aprendiz? aprendiz { get; set; }
    }
}
