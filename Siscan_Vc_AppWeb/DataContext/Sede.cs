using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class Sede
{
    public int IdSede { get; set; }

    public string? NombreSede { get; set; }

    public string? CelSede { get; set; }

    public string? DireccionSede { get; set; }

    public int? IdCiudad { get; set; }

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual Ciudad? IdCiudadNavigation { get; set; }
}
