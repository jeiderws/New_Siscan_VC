using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class InscripcionTyt
{
    public string CodigoInscripcion { get; set; } = null!;

    public int Idciudad { get; set; }

    public int IdAprendiz { get; set; }

    public int? IdConvocatoria { get; set; }

    public int IdEstadotyt { get; set; }

    public virtual ConvocatoriaTyt? IdConvocatoriaNavigation { get; set; }

    public virtual EstadoInscripcionTyt IdEstadotytNavigation { get; set; } = null!;

    public virtual Ciudad IdciudadNavigation { get; set; } = null!;
}
