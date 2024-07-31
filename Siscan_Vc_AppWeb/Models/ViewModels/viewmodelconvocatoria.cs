using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class viewmodelconvocatoria
    {
        public viewmodelconvocatoria( ConvocatoriaTyt con)
        {
            IdConvocatoria = con.IdConvocatoria;
            FechaPresentacion = con.FechaPresentacion;
            SemestreConvocatoria = con.SemestreConvocatoria;
        }
        public int IdConvocatoria { get; set; }

        public DateOnly? FechaPresentacion { get; set; }

        public string? SemestreConvocatoria { get; set; }
    }
}
