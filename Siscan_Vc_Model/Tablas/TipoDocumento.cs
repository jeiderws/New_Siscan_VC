using System;
using System.Collections.Generic;

namespace Siscan_Vc_Model;

public partial class TipoDocumento
{
    public int IdTipoDocumento { get; set; }

    public string TipoDocumento1 { get; set; } = null!;

    public virtual ICollection<Aprendiz> Aprendizs { get; set; } = new List<Aprendiz>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
