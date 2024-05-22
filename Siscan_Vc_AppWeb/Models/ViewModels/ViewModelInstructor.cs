using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelInstructor
    {
        public ViewModelInstructor(Instructor ins)
        {
            NumeroDocumentoInstructor = ins.NumeroDocumentoInstructor;
            NombreInstructor = ins.NombreInstructor;
            ApellidoInstructor = ins.ApellidoInstructor;
            CelInstructor = ins.CelInstructor;
            CorreoInstructor = ins.CorreoInstructor;
            IdTipodocumento = ins.IdTipodocumento;
            Tipodocumento = ins.IdTipodocumentoNavigation.TipoDocumento1;
        }
        public string NumeroDocumentoInstructor { get; set; } = null!;

        public string? NombreInstructor { get; set; }

        public string? ApellidoInstructor { get; set; }

        public string? CelInstructor { get; set; }

        public string? CorreoInstructor { get; set; }

        public int IdTipodocumento { get; set; }

        public string Tipodocumento { get; set; }

    }
}
