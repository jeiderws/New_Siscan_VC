using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class AsignacionFicha
{
    public string? NumeroDocumentoInstructor { get; set; }

    public string? Ficha { get; set; }

    public int AsignacionFichaId { get; set; }

    public virtual Ficha? FichaNavigation { get; set; }

    public virtual Instructor? NumeroDocumentoInstructorNavigation { get; set; }
}
