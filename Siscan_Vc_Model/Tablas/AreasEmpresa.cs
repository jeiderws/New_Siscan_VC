using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class AreasEmpresa
{
    public int IdArea { get; set; }

    public string? NombreArea { get; set; }

    public string? DescripcionArea { get; set; }

    public virtual ICollection<AsignacionArea> AsignacionAreas { get; set; } = new List<AsignacionArea>();
}
