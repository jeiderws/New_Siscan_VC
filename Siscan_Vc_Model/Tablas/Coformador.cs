using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Coformador
{
    public int IdCoformador { get; set; }

    public string? NombreCoformador { get; set; }

    public string? ApellidoCoformador { get; set; }

    public string? NumeroDocumentoCoformador { get; set; }

    public string? CelCoformador { get; set; }

    public string? CorreoCoformador { get; set; }

    public string? NitEmpresa { get; set; }

    public virtual Empresa? NitEmpresaNavigation { get; set; }

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
