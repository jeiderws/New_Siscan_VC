using System;
using System.Collections.Generic;

namespace Siscan_Vc_AppWeb.DataContext;

public partial class SeguimientoInstructorAprendiz
{
    public int IdSeguimiento { get; set; }

    public string NumeroDocumentoAprendiz { get; set; } = null!;

    public string NumeroDocumentoInstructor { get; set; } = null!;

    public int? IdCoformador { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public string? Actividades { get; set; }

    public int? IdModalidad { get; set; }

    public long? IdAsignacionArea { get; set; }

    public int? IdAreaEmpresa { get; set; }

    public string? NitEmpresa { get; set; }

    public virtual AreasEmpresa? IdAreaEmpresaNavigation { get; set; }

    public virtual AsignacionArea? IdAsignacionAreaNavigation { get; set; }

    public virtual Coformador? IdCoformadorNavigation { get; set; }

    public virtual Modalidad? IdModalidadNavigation { get; set; }

    public virtual Empresa? NitEmpresaNavigation { get; set; }

    public virtual Aprendiz NumeroDocumentoAprendizNavigation { get; set; } = null!;

    public virtual Instructor NumeroDocumentoInstructorNavigation { get; set; } = null!;

    public virtual ICollection<Observacion> Observacions { get; set; } = new List<Observacion>();
}
