﻿using Siscan_Vc_DAL.DataContext;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguimiento
    {
        public ViewModelSeguimiento(SeguimientoInstructorAprendiz seguimiento)
        {
            IdSeguimiento = seguimiento.IdSeguimiento;
            FechaRealizacionSeguimiento = seguimiento.FechaRealizacionSeguimiento;
            // Aprendiz
            if (seguimiento.NumeroDocumentoAprendizNavigation != null)
            {
                idTipoDocumentoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.IdTipodocumento;
                NumeroDocumentoAprendiz = seguimiento.NumeroDocumentoAprendiz;
                NombreAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.NombreAprendiz;
                ApellidoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.ApellidoAprendiz;
                CorreoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CorreoAprendiz;
                TelefonoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CelAprendiz;
                if (seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation != null && seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation.CodigoProgramaNavigation != null)
                {
                    ProgramAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation.CodigoProgramaNavigation.NombrePrograma;
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
                IdAsignacionArea = seguimiento.IdAsignacionArea;
                IdAreaEmpresa = seguimiento.IdAreaEmpresa;
            }

            // Practicas
            FechaInicio = seguimiento.FechaInicio;
            FechaFinalizacion = seguimiento.FechaFinalizacion;
            NombreModalidad = seguimiento.IdModalidadNavigation.NombreModalidad;
            idmodalidad = seguimiento.IdModalidad;

            //Proyecto
            NitProyecto = seguimiento.NitProyecto;
            NombreProyecto=seguimiento.NombreProyecto;
            ObjetivoProyecto = seguimiento.ObjetivoProyecto;
        }
        public List<Actividade> actividades { get; set; }
        public List<Observacion> observaciones { get; set; }
        public int IdSeguimiento { get; set; }
        [Required]
        public DateOnly? FechaRealizacionSeguimiento { get; set; }
        //aprendiz
        public int? idTipoDocumentoAprendiz { get; set; }
        public string TipoDocumentoAprendiz { get; set; }
        [Required]
        public string NumeroDocumentoAprendiz { get; set; } = null!;
        public string? NombreAprendiz { get; set; }
        public string? ApellidoAprendiz { get; set; }
        public string? CorreoAprendiz { get; set; }
        public string? TelefonoAprendiz { get; set; }
        public string? ProgramAprendiz { get; set; }
        public string? FichaAprendiz { get; set; }

        //Instructor
        [Required]
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

        //proyecto productivo
        [Required]
        public string? NitProyecto { get; set; }
        [Required]
        public string? NombreProyecto { get; set; }
        [Required]
        public string? ObjetivoProyecto { get; set; }


    }
}
