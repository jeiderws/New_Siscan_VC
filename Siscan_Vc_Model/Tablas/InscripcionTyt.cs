using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class InscripcionTyt
{
    public string CodigoInscripcion { get; set; } = null!;

    public int? Idciudad { get; set; }

    public string? NumeroDocumentoAprendiz { get; set; }

    public int? IdConvocatoria { get; set; }

    public int? IdEstadotyt { get; set; }

    public virtual ConvocatoriaTyt? IdConvocatoriaNavigation { get; set; }

    public virtual EstadoInscripcionTyt IdEstadotytNavigation { get; set; } = null!;

    public virtual Ciudad IdciudadNavigation { get; set; } = null!;

    public virtual Aprendiz? NumeroDocumentoAprendizNavigation { get; set; }
}
