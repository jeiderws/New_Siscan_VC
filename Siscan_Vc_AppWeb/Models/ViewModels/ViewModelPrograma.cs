using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelPrograma
    {
        public ViewModelPrograma(Programas pr)
        {
            CodigoPrograma = pr.CodigoPrograma;
            NombrePrograma = pr.NombrePrograma;
            IdEstadoPrograma = pr.IdEstadoProgramaNavigation.IdEstadoPrograma;
            IdNivelPrograma = pr.IdNivelProgramaNavigation.IdNivelPrograma;
            NivePrograma = pr.IdNivelProgramaNavigation.NivelPrograma1;
            EstadoPrograma = pr.IdEstadoProgramaNavigation.DescripcionEstadoPrograma;
        }
        public string CodigoPrograma { get; set; } = null!;
        public string? NombrePrograma { get; set; }
        public int? IdNivelPrograma { get; set; }
        public string? NivePrograma { get; set; }
        public int? IdEstadoPrograma { get; set; }
        public string? EstadoPrograma { get; set; }

    }
}
