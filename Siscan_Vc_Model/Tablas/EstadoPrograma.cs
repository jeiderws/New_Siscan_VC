using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class EstadoPrograma
{
    public int IdEstadoPrograma { get; set; }

    public string? DescripcionEstadoPrograma { get; set; }

    public virtual ICollection<Programas> Programas { get; set; } = new List<Programas>();
}
