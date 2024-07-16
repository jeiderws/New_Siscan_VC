using Microsoft.AspNetCore.Mvc.Rendering;
using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class ModelViewEmpresa
    {
        public List<SelectListItem> listaOpcCiudad { get; set; }
        public List<SelectListItem> listaOpcDepartamento { get; set; }

        public Empresa empresa {  get; set; }
        public Coformador coformador {  get; set; }
    }
}
 