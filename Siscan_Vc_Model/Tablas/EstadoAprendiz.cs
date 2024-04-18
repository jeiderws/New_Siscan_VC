using Siscan_Vc_Model;
using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class EstadoAprendiz
{
    public int IdEstado { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<Aprendiz> Aprendiz { get; set; } = new List<Aprendiz>();

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();
}
