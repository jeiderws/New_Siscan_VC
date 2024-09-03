using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_DAL.DataContext;

public partial class Instructor
{
    [Required(ErrorMessage ="Por favor, introduce un numero de docmento.")]
    [StringLength(10, ErrorMessage ="Por favor ingrese un numero de documento valido.")]
    public string NumeroDocumentoInstructor { get; set; } = null!;

    public string? NombreInstructor { get; set; }

    public string? ApellidoInstructor { get; set; }

    public string? CelInstructor { get; set; }

    [EmailAddress(ErrorMessage = "Por favor, introduce una dirección de correo electrónico válida.")]
    public string? CorreoInstructor { get; set; }

    public int IdTipodocumento { get; set; }

    public virtual ICollection<AsignacionFicha> AsignacionFichas { get; set; } = new List<AsignacionFicha>();

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual TipoDocumento IdTipodocumentoNavigation { get; set; } = null!;

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
