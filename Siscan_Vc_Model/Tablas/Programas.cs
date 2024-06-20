﻿using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Programas
{
    public string CodigoPrograma { get; set; } = null!;

    public string? NombrePrograma { get; set; }

    public int? IdNivelPrograma { get; set; }

    public int? IdEstadoPrograma { get; set; }

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual EstadoPrograma? IdEstadoProgramaNavigation { get; set; }

    public virtual NivelPrograma? IdNivelProgramaNavigation { get; set; }
}
