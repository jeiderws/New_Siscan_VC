using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelTipoDocumento
    {
        public ViewModelTipoDocumento(TipoDocumento tpdoc)
        {
            IdTipoDocumento = tpdoc.IdTipoDocumento;
            TipoDocumento1 = tpdoc.TipoDocumento1;
        }
        public int IdTipoDocumento { get; set; }

        public string TipoDocumento1 { get; set; } = null!;
    }
}
