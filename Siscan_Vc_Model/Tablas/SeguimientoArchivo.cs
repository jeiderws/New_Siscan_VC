using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class SeguimientoArchivo
{
    public long IdSeguimientoArchivo { get; set; }

    public string? NumeroDocumentoAprendiz { get; set; }

    public string? NumeroDocumentoInstructor { get; set; }

    public string? NumeroDocumentoCoformador { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public int? IdModalidad { get; set; }

    public long? IdAsignacionArea { get; set; }

    public int? IdAreaEmpresa { get; set; }

    public string? NitEmpresa { get; set; }

    public string? NitProyecto { get; set; }

    public string? NombreProyecto { get; set; }

    public string? ObjetivoProyecto { get; set; }

    public DateOnly? FechaRealizacionSeguimiento { get; set; }
}
