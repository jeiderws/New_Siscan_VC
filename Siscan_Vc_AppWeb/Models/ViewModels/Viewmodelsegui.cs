using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;
using System.Security.Policy;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class Viewmodelsegui
    {
        //opciones seleccionadas en los combos
        public string? opcSeleccionadaAprendizSeguimiento { get; set; }
        public string? opcseleccionadaEmpre { get; set; }
        public string? opcseleccionadaAreaEmpre { get; set; }
        public int? opcseleccionadaCoform { get; set; }
        public int? opcseleccionadaModalidad { get; set; }

        //listas selecionables de los combos
        public List<SelectListItem>? listaopcempresa { get; set; }
        public List<SelectListItem>? listaopcAreaEmpre { get; set; }
        public List<SelectListItem>? listaopcCoform { get; set; }
        public List<SelectListItem>? listaopcModalidad { get; set; }
        public List<SelectListItem>? listaopctpdoc { get; set; }

        //Listas y objetos
        public List<ViewModelSeguimiento>? listaSeguimiento { get; set; }
        public List<Empresa>? listaEmpresa { get; set; }
        public List<ViewModelAprendiz>? listaAprendizSinSegui { get; set; }
        public List<ViewModelAprendiz>? listaAprendizSegui { get; set; }
        public Empresa? Empresa { get; set; }
        public Instructor? Instructor { get; set; }
        public SeguimientoInstructorAprendiz seguimientoinstructorAprendiz { get; set; }
        public AsignacionArea? asignacionArea { get; set; }
        public Aprendiz? aprendiz { get; set; }
        public ViewModelAprendiz? aprendizSegui { get; set; }
        public ViewModelSeguimiento? seguimiento { get; set; }
        public Actividade? actividades { get; set; }
        public List<string>? actividadesList { get; set; }
        public List<string>? observacionesList { get; set; }
    }
}
