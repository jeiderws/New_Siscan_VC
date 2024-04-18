using Siscan_Vc_Model;
using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class TipoDocumento
{
    public int IdTipoDocumento { get; set; }

    public string TipoDocumento1 { get; set; } = null!;

    public virtual ICollection<Aprendiz> Aprendiz { get; set; } = new List<Aprendiz>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
