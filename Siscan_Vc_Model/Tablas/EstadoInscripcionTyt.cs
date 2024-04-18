using Siscan_Vc_Model;
using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class EstadoInscripcionTyt
{
    public int IdEstadotyt { get; set; }

    public string? DescripcionEstadotyt { get; set; }

    public virtual ICollection<Aprendiz> Aprendiz { get; set; } = new List<Aprendiz>();

    public virtual ICollection<InscripcionTyt> InscripcionTyts { get; set; } = new List<InscripcionTyt>();
}
