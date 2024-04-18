using Siscan_Vc_Model;
using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Empresa
{
    public string Nitmpresa { get; set; } = null!;

    public string? NombreEmpresa { get; set; }

    public string? RepresentanteLegal { get; set; }

    public string? DireccionEmpresa { get; set; }

    public string? TelefonoEmpresa { get; set; }

    public int? IdCiudad { get; set; }

    public virtual ICollection<AsignacionArea> AsignacionAreas { get; set; } = new List<AsignacionArea>();

    public virtual ICollection<Coformador> Coformadors { get; set; } = new List<Coformador>();

    public virtual Ciudad? IdCiudadNavigation { get; set; }
}
