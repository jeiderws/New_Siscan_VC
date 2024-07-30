using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguiArchivoAprendiz
    {
        public string Area {  get; set; }
        public string Modalidad {  get; set; }
        public Empresa Empresa { get; set; }
        public Coformador Coformador { get; set; }
        public Instructor Instructor { get; set; }

        public Aprendiz Aprendiz { get; set; }
    }
}
