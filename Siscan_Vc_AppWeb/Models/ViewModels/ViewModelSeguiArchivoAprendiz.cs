using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguiArchivoAprendiz
    {
        public DateOnly? FechaInicio {  get; set; }
        public DateOnly? FechaFinalizacion {  get; set; }
        public string Area {  get; set; }
        public string Modalidad {  get; set; }
        public Empresa Empresa { get; set; }
        public Coformador Coformador { get; set; }
        public Instructor Instructor { get; set; }
    }
}
