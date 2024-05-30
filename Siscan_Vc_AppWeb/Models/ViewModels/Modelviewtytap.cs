using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class Modelviewtytap
    {
        public int OpcSeleccionadoTpDoc { get; set; }
        public List<SelectListItem> listaOpcTpDoc { get; set; }
        public int OpcSeleccionadoEstado { get; set; }
        public List<SelectListItem> listaOpcEstado { get; set; }
        public int OpcSeleccionadoCiudad { get; set; }
        public int OpcSeleccionadoCiudadTyt { get; set; }
        public List<SelectListItem> listaOpcCiudad { get; set; }
        public int OpcSeleccionadoEstadoTyt { get; set; }
        public List<SelectListItem> listaOpcEstadoTyt { get; set; }
        public int OpcSeleccionadoFicha { get; set; }
        public List<SelectListItem> listaOpcFicha { get; set; }
        public int OpcSeleccionadoConvocatoria { get; set; }
        public List<SelectListItem> listaOpcConvocatoria { get; set; }
        public List<SelectListItem> listaOpcPrograma { get; set; }
        public List<SelectListItem> listaOpcDepartamento { get; set; }
        public Aprendiz aprendiz { get; set; }
        public InscripcionTyt? inscripcionTyt { get; set; }
    }
}
