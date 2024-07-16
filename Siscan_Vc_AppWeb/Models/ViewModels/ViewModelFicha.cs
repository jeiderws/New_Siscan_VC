using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelFicha
    {
        public ViewModelFicha(Ficha fi)
        {
            Ficha1 = fi.Ficha1.ToString();
            FechaFinalizacion = fi.FechaFinalizacion;
            FechaInicio = fi.FechaInicio;
            CodigoPrograma = fi.ProgramaNavigation.CodigoPrograma;
            NumeroDocumentoInstructor = fi.NumeroDocumentoInstructor;
            instructor = fi.NumeroDocumentoInstructorNavigation.NombreInstructor;
            Programas = fi.ProgramaNavigation.NombrePrograma;
            IdSede = fi.IdSedeNavigation.IdSede;
            Sede = fi.IdSedeNavigation.NombreSede;

        }
        public string Ficha1 { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFinalizacion { get; set; }

        public string? CodigoPrograma { get; set; }
        public string? Programas { get; set; }

        public string? NumeroDocumentoInstructor { get; set; }
        public string? instructor { get; set; }

        public int? IdSede { get; set; }
        public string? Sede { get; set; }

        public ICollection<AsignacionFicha> asignacionFicha { get; set; }
    }
}
