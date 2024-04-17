using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.DataContext.Repositories
{
    public interface IGenericRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Insert(TEntityModel modelo);
        Task<bool> Update(TEntityModel modelo);
        Task<bool> Delete(string numeroDocumento);
        Task<TEntityModel> GetById(int id);
        Task<IQueryable<TEntityModel>> GetAll();

    }
}
