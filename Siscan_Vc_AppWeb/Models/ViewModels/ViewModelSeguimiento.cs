﻿using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ViewModelSeguimiento
    {
        public ViewModelSeguimiento(SeguimientoInstructorAprendiz seguimiento)
        {
            IdSeguimiento = seguimiento.IdSeguimiento;
            
            //aprendiz
            NumeroDocumentoAprendiz = seguimiento.NumeroDocumentoAprendiz;
            NombreAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.NombreAprendiz;
            ApellidoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.ApellidoAprendiz;
            CorreoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CorreoAprendiz;
            TelefonoAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.CelAprendiz;
            ProgramAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.FichaNavigation.Programa.NombrePrograma;
            FichaAprendiz = seguimiento.NumeroDocumentoAprendizNavigation.Ficha.ToString();
           
            //Instructor
            NombreInstructor = seguimiento.NumeroDocumentoInstructorNavigation.NombreInstructor;
            ApellidoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.ApellidoInstructor;
            CorreoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.CorreoInstructor;
            TelefonoInstructor = seguimiento.NumeroDocumentoInstructorNavigation.CelInstructor;

            //coformador
            NombreCoformador = seguimiento.IdCoformadorNavigation.NombreCoformador;
            ApellidoCoformador = seguimiento.IdCoformadorNavigation.ApellidoCoformador;
            CorreoCoformador = seguimiento.IdCoformadorNavigation.CorreoCoformador;
            TelefonoCoformador = seguimiento.IdCoformadorNavigation.CelCoformador;

            //Empresa
            NitEmpresa = seguimiento.NitEmpresa;
            NombreEmpresa = seguimiento.NitEmpresaNavigation.NombreEmpresa;
            AreaEmpresa = seguimiento.IdAreaEmpresaNavigation.NombreArea;

            //practicas
            FechaInicio = seguimiento.FechaInicio;
            FechaFinalizacion = seguimiento.FechaFinalizacion;
            NombreModalidad = seguimiento.IdModalidadNavigation.NombreModalidad;

        }
        public long IdSeguimiento { get; set; }
        //aprendiz
        public string NumeroDocumentoAprendiz { get; set; } = null!;
        public string? NombreAprendiz { get; set; }
        public string? ApellidoAprendiz { get; set; }
        public string? CorreoAprendiz { get; set; }
        public string? TelefonoAprendiz { get; set; }
        public string ProgramAprendiz { get; set; }
        public string FichaAprendiz { get; set; }
        
        //Instructor
        public string NumeroDocumentoInstructor { get; set; } = null!;
        public string NombreInstructor { get; set; } = null!;
        public string ApellidoInstructor { get; set; } = null!;
        public string CorreoInstructor { get; set; } = null!;
        public string TelefonoInstructor { get; set; } = null!;
        
        //coformador
        public int? IdCoformador { get; set; }
        public string? NombreCoformador { get; set; }
        public string? ApellidoCoformador { get; set; }
        public string NumDocumentoCoformador { get; set; } = null!;
        public string CorreoCoformador { get; set; } = null!;
        public string TelefonoCoformador { get; set; } = null!;

        //practicas
        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFinalizacion { get; set; }

        public string? NombreModalidad { get; set; }

        //Empresa
        public int? IdAreaEmpresa { get; set; }
        public string? NitEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? AreaEmpresa { get; set; }
        public long? IdAsignacionArea { get; set; }



    }
}