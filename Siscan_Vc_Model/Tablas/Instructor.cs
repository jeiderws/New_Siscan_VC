using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Instructor
{
    public string NumeroDocumentoInstructor { get; set; } = null!;

    public string? NombreInstructor { get; set; }

    public string? ApellidoInstructor { get; set; }

    public string? CelInstructor { get; set; }

    public string? CorreoInstructor { get; set; }

    public int IdTipodocumento { get; set; }

    public virtual ICollection<AsignacionFicha> AsignacionFichas { get; set; } = new List<AsignacionFicha>();

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual TipoDocumento IdTipodocumentoNavigation { get; set; } = null!;

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
