using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.DataContext.Repositories
{
    public interface IGensericRepositoryInt<TEntityModel> where TEntityModel : class
    {
        Task<bool> Insert(TEntityModel modelo);
        Task<bool> Update(TEntityModel modelo);
        Task<bool> Delete(int id);
        Task<TEntityModel> GetById(int id);
        Task<IQueryable<TEntityModel>> GetAll();
    }
}
