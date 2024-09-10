using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_DAL.DataContext;

public partial class Empresa
{
    [Required(ErrorMessage ="Por favor, Ingrese el nit de la empresa.")]
    public string Nitmpresa { get; set; } = null!;

    [Required(ErrorMessage ="Por favor, Ingrese el nombre de la empresa.")]
    public string? NombreEmpresa { get; set; }

    [Required(ErrorMessage ="Por favor, Ingrese el nombre del representante legal de la empresa.")]
    public string? RepresentanteLegal { get; set; }

    [Required(ErrorMessage ="Por favor, Ingrese la direccion de la empresa.")]
    public string? DireccionEmpresa { get; set; }

    [Required(ErrorMessage ="Por favor, Ingrese el telefono de contacto de la empresa.")]
    [Phone]
    public string? TelefonoEmpresa { get; set; }

    [Required(ErrorMessage ="Por favor, Seleccione la ciudad donde se encuentra la empresa.")]
    public int? IdCiudad { get; set; }

    public virtual ICollection<AsignacionArea>? AsignacionAreas { get; set; } = new List<AsignacionArea>();

    public virtual ICollection<Coformador>? Coformadors { get; set; } = new List<Coformador>();

    public virtual Ciudad? IdCiudadNavigation { get; set; }

    public virtual ICollection<SeguimientoInstructorAprendiz>? SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
