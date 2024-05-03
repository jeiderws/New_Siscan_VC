using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelEmpresa
    {
        public ViewModelEmpresa(Empresa empresa)
        {
            Nitmpresa=empresa.Nitmpresa;

            NombreEmpresa=empresa.NombreEmpresa;

            RepresentanteLegal=empresa.RepresentanteLegal;

            DireccionEmpresa = empresa.DireccionEmpresa;

            TelefonoEmpresa = empresa.TelefonoEmpresa;

            IdCiudad=empresa.IdCiudad;

            NombreEmpresa = empresa.IdCiudadNavigation.NombreCiudad;
            
        }
        public string Nitmpresa { get; set; } = null!;

        public string? NombreEmpresa { get; set; }

        public string? RepresentanteLegal { get; set; }

        public string? DireccionEmpresa { get; set; }

        public string? TelefonoEmpresa { get; set; }

        public int? IdCiudad { get; set; }

        public string? NombreCiudad { get; set; }
    }
}
