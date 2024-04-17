using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class Programa
{
    public string CodigoPrograma { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string? NombrePrograma { get; set; }

    public int? IdNivelPrograma { get; set; }

    public int? IdEstadoPrograma { get; set; }

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual EstadoPrograma? IdEstadoProgramaNavigation { get; set; }

    public virtual NivelPrograma? IdNivelProgramaNavigation { get; set; }
}
