using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service
{
    public class AprendizService : IAprendizService
    {
        private readonly IGenericRepository<Aprendiz> _aprendizrepo;
        public AprendizService(IGenericRepository<Aprendiz> repoaprendiz)
        {
            _aprendizrepo = repoaprendiz;
        }
        public Task<bool> Delete(string numDoc)
        {
            return _aprendizrepo.Delete(numDoc);
        }

        public Task<IQueryable<Aprendiz>> GetAll()
        {
            return _aprendizrepo.GetAll();
        }

        public async Task<Aprendiz> GetForDoc(string numeroDoc)
        {
            return await _aprendizrepo.GetForDoc(numeroDoc);
        }

        public async Task<Aprendiz> GetForName(string name)
        {
            try
            {
                IQueryable<Aprendiz> queryAprendiz=await _aprendizrepo.GetAll();
                Aprendiz aprendiz=queryAprendiz.Where(a=>a.NombreAprendiz==name).FirstOrDefault();
                return aprendiz;
            }
            catch { return null; }
        }

        public Task<bool> Insert(Aprendiz model)
        {
            return _aprendizrepo.Insert(model);
        }

        public async Task<bool> Update(Aprendiz model)
        {
            return await _aprendizrepo.Update(model);
        }
    }
}
