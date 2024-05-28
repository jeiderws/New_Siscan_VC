using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguimiento
    {
        public ViewModelSeguimiento(SeguimientoInstructorAprendiz seguimiento)
        {
            IdSeguimiento = seguimiento.IdSeguimiento;

            // Aprendiz
            if (seguimiento.NumeroDocumentoAprendizNavigation != null)
            {
                NumeroDocumentoAprendiz = seguimiento.NumeroDocumentoAprendiz;
                NombreAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.NombreAprendiz;
                ApellidoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.ApellidoAprendiz;
                CorreoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CorreoAprendiz;
                TelefonoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CelAprendiz;
                if (seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation != null &&
                    seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation.ProgramaNavigation != null)
                {
                    ProgramAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation.ProgramaNavigation.NombrePrograma;
                }
                FichaAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.Ficha?.ToString();
            }

            // Instructor
            if (seguimiento.NumeroDocumentoInstructorNavigation != null)
            {
                NumeroDocumentoInstructor = seguimiento.NumeroDocumentoInstructor;
                NombreInstructor = seguimiento.NumeroDocumentoInstructorNavigation.NombreInstructor;
                ApellidoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.ApellidoInstructor;
                CorreoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.CorreoInstructor;
                TelefonoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.CelInstructor;
            }

            // Coformador
            if (seguimiento.IdCoformadorNavigation != null)
            {
                NombreCoformador = seguimiento.IdCoformadorNavigation.NombreCoformador;
                ApellidoCoformador = seguimiento.IdCoformadorNavigation.ApellidoCoformador;
                CorreoCoformador = seguimiento.IdCoformadorNavigation.CorreoCoformador;
                TelefonoCoformador = seguimiento.IdCoformadorNavigation.CelCoformador;
            }

            // Empresa
            if (seguimiento.NitEmpresaNavigation != null)
            {
                NitEmpresa = seguimiento.NitEmpresa;
                NombreEmpresa = seguimiento.NitEmpresaNavigation.NombreEmpresa;
                AreaEmpresa = seguimiento.IdAreaEmpresaNavigation.NombreArea;
            }

            // Practicas
            FechaInicio = seguimiento.FechaInicio;
            FechaFinalizacion = seguimiento.FechaFinalizacion;
            NombreModalidad = seguimiento.IdModalidadNavigation.NombreModalidad;
            idmodalidad = seguimiento.IdModalidad;
        }
        public long IdSeguimiento { get; set; }
        //aprendiz
        public string NumeroDocumentoAprendiz { get; set; } = null!;
        public string? NombreAprendiz { get; set; }
        public string? ApellidoAprendiz { get; set; }
        public string? CorreoAprendiz { get; set; }
        public string? TelefonoAprendiz { get; set; }
        public string? ProgramAprendiz { get; set; }
        public string? FichaAprendiz { get; set; }
        
        //Instructor
        public string? NumeroDocumentoInstructor { get; set; } = null!;
        public string? NombreInstructor { get; set; } = null!;
        public string? ApellidoInstructor { get; set; } = null!;
        public string? CorreoInstructor { get; set; } = null!;
        public string? TelefonoInstructor { get; set; } = null!;
        
        //coformador
        public int? IdCoformador { get; set; }
        public string? NombreCoformador { get; set; }
        public string? ApellidoCoformador { get; set; }
        public string? NumDocumentoCoformador { get; set; } = null!;
        public string? CorreoCoformador { get; set; } = null!;
        public string? TelefonoCoformador { get; set; } = null!;

        //practicas
        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFinalizacion { get; set; }

        public string? NombreModalidad { get; set; }
        public int? idmodalidad { get; set; }

        //Empresa
        public int? IdAreaEmpresa { get; set; }
        public string? NitEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? AreaEmpresa { get; set; }
        public long? IdAsignacionArea { get; set; }



    }
}
