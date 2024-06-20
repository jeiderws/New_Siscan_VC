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
            IdSede = fi.IdSede;

        }
        public int Ficha1 { get; set; }

        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFinalizacion { get; set; }

        public string? CodigoPrograma { get; set; }

        public string? NumeroDocumentoInstructor { get; set; }

        public int? IdSede { get; set; }

    }
}
