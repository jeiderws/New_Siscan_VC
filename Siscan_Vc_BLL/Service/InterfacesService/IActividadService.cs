using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IActividadService
    {
        Task<bool> Insert(Actividade model);
        Task<bool> Update(Actividade model);
        Task<bool> Delete(string id);
        Task<Actividade> GetForId(string id);
        Task<IQueryable<Actividade>> GetAll();
        Task<Actividade> GetForSegui(int idsegui);  
    }
}
