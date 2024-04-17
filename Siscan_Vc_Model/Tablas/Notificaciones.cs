using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class Notificaciones
{
    public int IdNotificacion { get; set; }

    public string? DescripcionNotificion { get; set; }

    public int IdEstado { get; set; }

    public virtual EstadoAprendiz IdEstadoNavigation { get; set; } = null!;
}
