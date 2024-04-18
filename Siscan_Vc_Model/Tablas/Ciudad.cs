using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Ciudad
{
    public int IdCiudad { get; set; }

    public string? NombreCiudad { get; set; }

    public int IdDepartamento { get; set; }

    public virtual ICollection<Aprendiz> Aprendiz { get; set; } = new List<Aprendiz>();

    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<InscripcionTyt> InscripcionTyts { get; set; } = new List<InscripcionTyt>();

    public virtual ICollection<Sedes> Sedes { get; set; } = new List<Sedes>();
}
