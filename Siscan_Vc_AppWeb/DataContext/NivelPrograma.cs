using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class NivelPrograma
{
    public int IdNivelPrograma { get; set; }

    public string? NivelPrograma1 { get; set; }

    public virtual ICollection<Programa> Programas { get; set; } = new List<Programa>();
}
