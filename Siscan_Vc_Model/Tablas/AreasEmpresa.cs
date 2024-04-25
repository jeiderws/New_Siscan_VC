using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class AreasEmpresa
{
    public int IdArea { get; set; }

    public string? NombreArea { get; set; }

    public string? DescripcionArea { get; set; }

    public virtual ICollection<AsignacionArea> AsignacionAreas { get; set; } = new List<AsignacionArea>();

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
