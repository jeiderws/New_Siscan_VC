using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelInscripcionTyt
    {
        public ViewModelInscripcionTyt(InscripcionTyt intyt)
        {
                CodigoInscripcion = intyt.CodigoInscripcion;
                NumeroDocumentoAprendiz = intyt.NumeroDocumentoAprendiz;
                NumeroDocumentoAprendi = intyt.NumeroDocumentoAprendizNavigation.NumeroDocumentoAprendiz;
                Idciudad = intyt.Idciudad;
                nomciudad = intyt.IdciudadNavigation.NombreCiudad;
                IdConvocatoria = intyt.IdConvocatoria;
                nomConvocatoria = intyt.IdConvocatoriaNavigation.SemestreConvocatoria;
                IdEstadotyt = intyt.IdEstadotyt;
                nomEstadotyt = intyt.IdEstadotytNavigation.DescripcionEstadotyt;
        }
        public string CodigoInscripcion { get; set; } = null!;

        public int? Idciudad { get; set; }
        public string nomciudad { get; set; }

        public string? NumeroDocumentoAprendiz { get; set; }
        public string? NumeroDocumentoAprendi { get; set; }

        public int? IdConvocatoria { get; set; }
        public string? nomConvocatoria { get; set; }

        public int? IdEstadotyt { get; set; }
        public string nomEstadotyt { get; set; }

    }
}
