using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string? NombreDepartamento { get; set; }

    public int? IdPais { get; set; }

    public virtual ICollection<Ciudad> Ciudads { get; set; } = new List<Ciudad>();

    public virtual Pais? IdPaisNavigation { get; set; }
}
