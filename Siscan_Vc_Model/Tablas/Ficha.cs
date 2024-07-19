using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Ficha
{
    public string Ficha1 { get; set; } = null!;

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public string? CodigoPrograma { get; set; }

    public string? NumeroDocumentoInstructor { get; set; }

    public int? IdSede { get; set; }

    public virtual ICollection<Aprendiz> Aprendizs { get; set; } = new List<Aprendiz>();

    public virtual ICollection<AsignacionFicha> AsignacionFichas { get; set; } = new List<AsignacionFicha>();

    public virtual Programas? CodigoProgramaNavigation { get; set; }

    public virtual Sede? IdSedeNavigation { get; set; }

    public virtual Instructor? NumeroDocumentoInstructorNavigation { get; set; }
}
