using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class AsignacionFicha
{
    public string? NumeroDocumentoInstructor { get; set; }

    public int? Ficha { get; set; }

    public virtual Ficha? FichaNavigation { get; set; }

    public virtual Instructor? NumeroDocumentoInstructorNavigation { get; set; }
}
