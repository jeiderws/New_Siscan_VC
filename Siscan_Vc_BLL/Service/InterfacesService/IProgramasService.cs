using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IProgramasService
    {
        Task<bool> Insert(Programas model);
        Task<bool> Update(Programas model);
        Task<bool> Delete(string codigoPrograma);
        Task<Programas> GetForCog(string codigoPrograma);
        Task<IQueryable<Programas>> GetAll();
        Task<Programas> GetForName(string name);
        Task<Programas> GetByCodigo(string codigo);
    }
}
