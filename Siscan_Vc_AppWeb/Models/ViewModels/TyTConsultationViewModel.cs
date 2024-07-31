using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class TyTConsultationViewModel
    {
        public string FichaId { get; set; }
        public string SemestreConvocatoria { get; set; }
        public bool ChangeStatusToRealizadas { get; set; }
        public List<AprendizViewModel> Aprendices { get; set; }
    }

    public class AprendizViewModel
    {
        public string NumeroDocumentoAprendiz { get; set; }
        public string NombreAprendiz { get; set; }
        public string ApellidoAprendiz { get; set; }
        public string CelAprendiz { get; set; }
        public string CorreoAprendiz { get; set; }
        public string DireccionAprendiz { get; set; }
        public string NombreCompletoAcudiente { get; set; }
        public string CorreoAcudiente { get; set; }
        public string CelularAcudiente { get; set; }
        public int? IdEstadoTyt { get; set; }
        public int? IdTipodocumento { get; set; }
        public string Ficha { get; set; }
        public int? IdCiudad { get; set; }
        public int? IdEstadoAprendiz { get; set; }
        public string CodigoInscripcion { get; set; }
        public bool HasCompletedTyt { get; set; }
        public bool ChangeStatusToRealizadas { get; set; }
    }
}
