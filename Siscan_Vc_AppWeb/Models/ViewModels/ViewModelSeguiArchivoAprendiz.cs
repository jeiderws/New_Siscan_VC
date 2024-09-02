using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguiArchivoAprendiz
    {
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFinalizacion { get; set; }
        public string Area { get; set; }
        public string Modalidad { get; set; }
        public Empresa Empresa { get; set; }
        public Coformador Coformador { get; set; }
        public Instructor Instructor { get; set; }
        public string? NitProyecto { get; set; }
        public string? NombreProyecto { get; set; }
        public string? ObjetivoProyecto { get; set; }
        public DateOnly? FechaRealizacionSeguimiento { get; set; }
    }
}
