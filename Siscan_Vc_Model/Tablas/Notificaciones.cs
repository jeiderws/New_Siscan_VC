using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Notificaciones
{
    public int IdNotificacion { get; set; }

    public string? DescripcionNotificion { get; set; }

    public int IdEstado { get; set; }

    public virtual EstadoAprendiz IdEstadoNavigation { get; set; } = null!;
}
