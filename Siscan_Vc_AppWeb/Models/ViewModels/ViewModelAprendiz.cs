using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelAprendiz
    {
        public ViewModelAprendiz(Aprendiz aprendiz){

            NumeroDocumentoAprendiz=aprendiz.NumeroDocumentoAprendiz;
            NombreAprendiz = aprendiz.NombreAprendiz;
            ApellidoAprendiz=aprendiz.ApellidoAprendiz ;
            CelAprendiz = aprendiz.CelAprendiz;
            CorreoAprendiz = aprendiz.CorreoAprendiz;
            DireccionAprendiz = aprendiz.DireccionAprendiz;
            NombreCompletoAcudiente=aprendiz.NombreCompletoAcudiente;
            CorreoAcuediente = aprendiz.CorreoAcuediente;
            CelularAcudiente=aprendiz.CelularAcudiente;

         
        }
        public string NumeroDocumentoAprendiz { get; set; } = null!;

        public string? NombreAprendiz { get; set; }

        public string? ApellidoAprendiz { get; set; }

        public string? CelAprendiz { get; set; }

        public string? CorreoAprendiz { get; set; }

        public string? DireccionAprendiz { get; set; }

        public string? NombreCompletoAcudiente { get; set; }

        public string? CorreoAcuediente { get; set; }

        public string? CelularAcudiente { get; set; }
        public int? IdEstadoTyt { get; set; }

        public int? IdTipodocumento { get; set; }

        public int? Ficha { get; set; }

        public int? IdCiudad { get; set; }

        public int? IdEstadoAprendiz { get; set; }
        public ViewModelTipoDocumento? VmTipoDocumento { get; set; }
    }
    
    
}


    


