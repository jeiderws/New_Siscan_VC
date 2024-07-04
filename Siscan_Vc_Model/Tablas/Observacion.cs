using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Observacion
{
    public int IdObservacion { get; set; }

    public string? Observaciones { get; set; }

    public int? IdSeguimiento { get; set; }

    public virtual SeguimientoInstructorAprendiz? IdSeguimientoNavigation { get; set; }
}
