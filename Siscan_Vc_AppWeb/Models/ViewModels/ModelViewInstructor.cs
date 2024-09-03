using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;
using System.ComponentModel.DataAnnotations;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewInstructor
    {
        public Instructor Instructor { get; set; }
        public List<SelectListItem> OpcionesTpDoc { get; set; }
        [Required]
        public int OpcSeleccionada { get; set; }
    }
}
