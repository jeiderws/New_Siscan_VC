using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewInstructor
    {
        public Instructor Instructor { get; set; }
        public List<SelectListItem> OpcionesTpDoc { get; set; }
        public int OpcSeleccionada { get; set; }
    }
}
