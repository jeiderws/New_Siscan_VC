using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class AsignacionArea
{
    public long IdAsignacionArea { get; set; }

    public int? IdArea { get; set; }

    public string? NitEmpresa { get; set; }

    public virtual AreasEmpresa? IdAreaNavigation { get; set; }

    public virtual Empresa? NitEmpresaNavigation { get; set; }

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
