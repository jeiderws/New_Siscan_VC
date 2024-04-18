using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Ficha
{
    public int Ficha1 { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public string? CodigoPrograma { get; set; }

    public string Version { get; set; } = null!;

    public string? NumeroDocumentoInstructor { get; set; }

    public int? IdSede { get; set; }

    public virtual ICollection<Aprendiz> Aprendiz { get; set; } = new List<Aprendiz>();

    public virtual Sedes? IdSedeNavigation { get; set; }

    public virtual Instructor? NumeroDocumentoInstructorNavigation { get; set; }

    public virtual Programas? Programa { get; set; }
}
