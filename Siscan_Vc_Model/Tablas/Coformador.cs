using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_DAL.DataContext;

public partial class Coformador
{
    public int IdCoformador { get; set; }

    [Required(ErrorMessage ="Por favor, Ingrese el nombre del coformador.")]
    public string? NombreCoformador { get; set; }

    [Required(ErrorMessage = "Por favor, Ingrese el apellido del coformador.")]
    public string? ApellidoCoformador { get; set; }

    [Required(ErrorMessage = "Por favor, Ingrese el numero de documento del coformador.")]
    public string? NumeroDocumentoCoformador { get; set; }

    [Required(ErrorMessage = "Por favor, Ingrese el numero de celular del coformador.")]
    [Phone(ErrorMessage ="Por favor, Ingrese un numero de celular valido.")]
    public string? CelCoformador { get; set; }

    [Required(ErrorMessage = "Por favor, Ingrese el numero de documento del coformador.")]
    [EmailAddress(ErrorMessage ="Por favor, Ingrese una direccion de correo electronico valido.")]
    public string? CorreoCoformador { get; set; }

    [Required(ErrorMessage ="Por favor, Ingrese el nit de la empresa.")]
    public string? NitEmpresa { get; set; }

    public virtual Empresa? NitEmpresaNavigation { get; set; }

    public virtual ICollection<SeguimientoInstructorAprendiz>? SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
