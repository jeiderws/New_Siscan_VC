using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class Instructor
{
    public string NumeroDocumentoInstructor { get; set; } = null!;

    public string? NombreInstructor { get; set; }

    public string? ApellidoInstructor { get; set; }

    public string? CelInstructor { get; set; }

    public string? CorreoInstructor { get; set; }

    public int IdTipodocumento { get; set; }

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual TipoDocumento IdTipodocumentoNavigation { get; set; } = null!;
}
