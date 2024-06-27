﻿using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class EstadoAprendiz
{
    public int IdEstado { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<Aprendiz> Aprendizs { get; set; } = new List<Aprendiz>();

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();
}
