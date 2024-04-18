using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Acudiente
{
    public int IdAcudiente { get; set; }

    public string? NombreAcudiente { get; set; }

    public string? ApellidoAcudiente { get; set; }

    public string? CorreoAcudiente { get; set; }

    public string? CelularAcudiente { get; set; }

    public virtual ICollection<Aprendiz> Aprendizs { get; set; } = new List<Aprendiz>();
}
