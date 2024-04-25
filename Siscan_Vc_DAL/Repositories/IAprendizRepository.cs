using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class IAprendizRepository : IGenericRepository<Aprendiz>
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public IAprendizRepository(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public async Task<bool> Delete(string numDoc)
        {
            try
            {
                Aprendiz aprendiz = _dbSiscanContext.Aprendiz.First(a => a.NumeroDocumentoAprendiz == numDoc);
                _dbSiscanContext.Remove(aprendiz);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public Task<IQueryable<Aprendiz>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Aprendiz> GetForDoc(string numeroDoc)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(Aprendiz model)
        {
            try
            {
                _dbSiscanContext.Aprendiz.Add(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Aprendiz model)
        {
            try
            {
                _dbSiscanContext.Update(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
