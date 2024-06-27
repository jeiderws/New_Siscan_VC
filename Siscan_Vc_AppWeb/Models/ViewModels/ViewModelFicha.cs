using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelFicha
    {
        public ViewModelFicha(Ficha fi)
        {
            Ficha1 = fi.Ficha1;
            FechaFinalizacion = fi.FechaFinalizacion;
            FechaInicio = fi.FechaInicio;
            CodigoPrograma = fi.CodigoPrograma;
            NumeroDocumentoInstructor = fi.NumeroDocumentoInstructor;
            instructor = fi.NumeroDocumentoInstructorNavigation.NombreInstructor;
            Programas = fi.ProgramaNavigation.NombrePrograma;
            IdSede = fi.IdSede;
            Sede = fi.IdSedeNavigation.NombreSede;

        }
        public int Ficha1 { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFinalizacion { get; set; }

        public string? CodigoPrograma { get; set; }
        public string? Programas { get; set; }

        public string? NumeroDocumentoInstructor { get; set; }
        public string? instructor { get; set; }

        public int? IdSede { get; set; }
        public string? Sede { get; set; }

    }
}
