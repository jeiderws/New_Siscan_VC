using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_DAL.DataContext;

public partial class SeguimientoInstructorAprendiz
{
    public int IdSeguimiento { get; set; }

    public string NumeroDocumentoAprendiz { get; set; } = null!;

    [Required(ErrorMessage ="Por favor, ingresa un numero de documento de el instructor de seguimiento.")]
    public string? NumeroDocumentoInstructor { get; set; } = null!;

    public int? IdCoformador { get; set; }

    [Required(ErrorMessage ="Por favor, ingresa una fecha.")]
    public DateOnly? FechaInicio { get; set; }

    [Required(ErrorMessage = "Por favor, ingresa una fecha.")]
    public DateOnly? FechaFinalizacion { get; set; }

    public string? NitProyecto { get; set; }

    public string? NombreProyecto { get; set; }

    public string? ObjetivoProyecto { get; set; }

    [Required(ErrorMessage ="Por favor, selecciona una modalidad.")]
    public int? IdModalidad { get; set; }

    public long? IdAsignacionArea { get; set; }

    [Required(ErrorMessage = "Por favor, selecciona un area.")]
    public int? IdAreaEmpresa { get; set; }

    public string? NitEmpresa { get; set; }

    [Required(ErrorMessage = "Por favor, ingresa una fecha.")]
    public DateOnly? FechaRealizacionSeguimiento { get; set; }

    public virtual ICollection<Actividade> Actividades { get; set; } = new List<Actividade>();

    public virtual AreasEmpresa? IdAreaEmpresaNavigation { get; set; }

    public virtual AsignacionArea? IdAsignacionAreaNavigation { get; set; }

    public virtual Coformador? IdCoformadorNavigation { get; set; }

    public virtual Modalidad? IdModalidadNavigation { get; set; }

    public virtual Empresa? NitEmpresaNavigation { get; set; }

    public virtual Aprendiz NumeroDocumentoAprendizNavigation { get; set; } = null!;

    public virtual Instructor NumeroDocumentoInstructorNavigation { get; set; } = null!;

    public virtual ICollection<Observacion> Observacions { get; set; } = new List<Observacion>();
}
