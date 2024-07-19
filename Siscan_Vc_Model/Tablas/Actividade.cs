using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Actividade
{
    public int IdActividad { get; set; }

    public string? DescripcionActividad { get; set; }

    public int? IdSeguimiento { get; set; }

    public virtual SeguimientoInstructorAprendiz? IdSeguimientoNavigation { get; set; }
}
