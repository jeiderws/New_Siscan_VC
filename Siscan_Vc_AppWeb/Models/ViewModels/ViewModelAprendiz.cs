using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelAprendiz
    {
        public ViewModelAprendiz(Aprendiz aprendiz)
        {        
            NumeroDocumentoAprendiz = aprendiz.NumeroDocumentoAprendiz;
            NombreAprendiz = aprendiz.NombreAprendiz;
            ApellidoAprendiz = aprendiz.ApellidoAprendiz;
            CelAprendiz = aprendiz.CelAprendiz;
            CorreoAprendiz = aprendiz.CorreoAprendiz;
            DireccionAprendiz = aprendiz.DireccionAprendiz;
            NombreCompletoAcudiente = aprendiz.NombreCompletoAcudiente;
            CorreoAcuediente = aprendiz.CorreoAcuediente;
            CelularAcudiente = aprendiz.CelularAcudiente;
            IdTipodocumento = aprendiz.IdTipodocumentoNavigation.IdTipoDocumento;
            nombredoc = aprendiz.IdTipodocumentoNavigation.TipoDocumento1;
            IdEstadoTyt = aprendiz.IdEstadoTytNavigation.IdEstadotyt;
            nomEstadoTyt = aprendiz.IdEstadoTytNavigation.DescripcionEstadotyt;
            IdEstadoAprendiz = aprendiz.IdEstadoAprendiz;
            nomEstadoAprendiz = aprendiz.IdEstadoAprendizNavigation.NombreEstado;
            SeguimientoInstructorAprendices = aprendiz.SeguimientoInstructorAprendizs;
            NombreApellidoDoc = aprendiz.NombreAprendiz +" "+ aprendiz.ApellidoAprendiz+" "+aprendiz.NumeroDocumentoAprendiz;
            Ficha=aprendiz.Ficha;
            SeguimientoInstructorAprendices=aprendiz.SeguimientoInstructorAprendizs;
        }
        public string NumeroDocumentoAprendiz { get; set; } = null!;

        public string? NombreAprendiz { get; set; }

        public string? NombreApellidoDoc {  get; set; }

        public string nombredoc { get; set; }

        public string? ApellidoAprendiz { get; set; }

        public string? CelAprendiz { get; set; }

        public string? CorreoAprendiz { get; set; }

        public string? DireccionAprendiz { get; set; }

        public string? NombreCompletoAcudiente { get; set; }

        public string? CorreoAcuediente { get; set; }

        public string? CelularAcudiente { get; set; }

        public int? IdEstadoTyt { get; set; }

        public string? nomEstadoTyt { get; set; }

        public int? IdTipodocumento { get; set; }

        public int? Ficha { get; set; }

        public string? Programa {  get; set; }

        public int? IdCiudad { get; set; }

        public int? IdEstadoAprendiz { get; set; }

        public string? nomEstadoAprendiz { get; set; }

        public virtual EstadoAprendiz? IdEstadoAprendizNavigation { get; set; }

        public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendices { get; set; } = new List<SeguimientoInstructorAprendiz>();
    }
}
