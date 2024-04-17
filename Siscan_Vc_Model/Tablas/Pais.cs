using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class Pais
{
    public int IdPais { get; set; }

    public string? NombrePais { get; set; }

    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
}
