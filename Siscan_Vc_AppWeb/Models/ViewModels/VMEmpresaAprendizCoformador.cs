using Siscan_Vc_DAL.DataContext;

namespace Siscan_Vc_AppWeb.Models.ViewModels
{
    public class VMEmpresaAprendizCoformador
    {
        public Empresa empresa {  get; set; }

        public List<Aprendiz> aprendices { get; set; }

        public List<Coformador> coformadores { get; set;}
    }
}
