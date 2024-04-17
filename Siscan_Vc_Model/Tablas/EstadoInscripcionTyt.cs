using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class EstadoInscripcionTyt
{
    public int IdEstadotyt { get; set; }

    public string? DescripcionEstadotyt { get; set; }

    public virtual ICollection<Aprendiz> Aprendizs { get; set; } = new List<Aprendiz>();

    public virtual ICollection<InscripcionTyt> InscripcionTyts { get; set; } = new List<InscripcionTyt>();
}
