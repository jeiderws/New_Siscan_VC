using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewSeguimientoArchivo
    {
        public List<ViewModelSeguimiento> listaSegui { get; set; }
        public List<ViewModelSeguiArchivoAprendiz> listaSeguiArchivos { get; set; }
        public Aprendiz Aprendiz { get; set; }
    }
}
