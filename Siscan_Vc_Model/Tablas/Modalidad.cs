﻿using System;
using System.Collections.Generic;

namespace Siscan_Vc_DAL.DataContext;

public partial class Modalidad
{
    public int IdModalidad { get; set; }

    public string? NombreModalidad { get; set; }

    public virtual ICollection<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; } = new List<SeguimientoInstructorAprendiz>();
}
