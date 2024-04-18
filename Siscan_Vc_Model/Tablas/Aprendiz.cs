﻿
using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Aprendiz
{
    public string NumeroDocumentoAprendiz { get; set; } = null!;

    public string? NombreAprendiz { get; set; }

    public string? ApellidoAprendiz { get; set; }

    public string? CelAprendiz { get; set; }

    public string? CorreoAprendiz { get; set; }

    public string? DireccionAprendiz { get; set; }

    public int? IdEstadoTyt { get; set; }

    public int? IdTipodocumento { get; set; }

    public int? Ficha { get; set; }

    public int? IdCiudad { get; set; }

    public int? IdAcudientes { get; set; }

    public int? IdEstadoAprendiz { get; set; }

    public virtual Ficha? FichaNavigation { get; set; }

    public virtual Acudientes? IdAcudientesNavigation { get; set; }

    public virtual Ciudad? IdCiudadNavigation { get; set; }

    public virtual EstadoAprendiz? IdEstadoAprendizNavigation { get; set; }

    public virtual EstadoInscripcionTyt? IdEstadoTytNavigation { get; set; }

    public virtual TipoDocumento? IdTipodocumentoNavigation { get; set; }
}
