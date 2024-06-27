using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class EstadoPrograma
{
    public int IdEstadoPrograma { get; set; }

    public string? DescripcionEstadoPrograma { get; set; }

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();
}
