using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class ConvocatoriaTyt
{
    public int IdConvocatoria { get; set; }

    public DateOnly? FechaPresentacion { get; set; }

    public string? SemestreConvocatoria { get; set; }

    public virtual ICollection<InscripcionTyt> InscripcionTyts { get; set; } = new List<InscripcionTyt>();
}
