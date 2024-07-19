using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class SeguimientoArchivo
{
    public long IdSeguimientoArchivo { get; set; }

    public string? NumeroDocumentoAprendiz { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? NumeroDocumentoCoformador { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public int? IdModalidad { get; set; }

    public long? IdAsignacionArea { get; set; }

    public int? IdAreaEmpresa { get; set; }

    public string? NitEmpresa { get; set; }
}
