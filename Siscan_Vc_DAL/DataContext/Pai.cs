using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Pai
{
    public int IdPais { get; set; }

    public string? NombrePais { get; set; }

    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();
}
