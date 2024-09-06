using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_DAL.DataContext;

public partial class Programas
{
    [Required(ErrorMessage ="Por Favor Introduce Un Codigo de Programa")]
    public string CodigoPrograma { get; set; } = null!;
    [Required(ErrorMessage = "Por Favor Introduce Un Nombre Para el  Programa")]
    public string? NombrePrograma { get; set; }
    [Required(ErrorMessage = "Por Favor Seleccione un Nivel Para el  Programa")]
    public int? IdNivelPrograma { get; set; }
    [Required(ErrorMessage = "Por Favor Seleccione un Estado Para el  Programa")]
    public int? IdEstadoPrograma { get; set; }

    public virtual ICollection<Ficha> Fichas { get; set; } = new List<Ficha>();

    public virtual EstadoPrograma? IdEstadoProgramaNavigation { get; set; }

    public virtual NivelPrograma? IdNivelProgramaNavigation { get; set; }
}
