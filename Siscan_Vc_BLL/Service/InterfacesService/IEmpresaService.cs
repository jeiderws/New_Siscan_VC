using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IEmpresaService
    {
        Task<bool> Insert(Empresa model);
        Task<bool> Update(Empresa model);
        Task<bool> Delete(string nit);
        Task<Empresa> GetForNit(string nit);
        Task<IQueryable<Empresa>> GetAll();
        Task<Empresa> GetForName(string name);
    }
}
