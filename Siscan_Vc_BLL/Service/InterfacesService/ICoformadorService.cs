using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface ICoformadorService
    {
        Task<bool> Insert(Coformador model);
        Task<bool> Update(Coformador model);
        Task<bool> Delete(string numDoc);
        Task<Coformador> GetForDoc(string numeroDoc);
        Task<IQueryable<Coformador>> GetAll();
        Task<Coformador> GetForName(string name);
        Task<Coformador> GetForEmpresa(string nitEmpresa);
    }
}
