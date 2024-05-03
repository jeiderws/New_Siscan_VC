using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class AprendizRepository : IGenericRepository<Aprendiz>
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public AprendizRepository(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Aprendiz aprendiz = _dbSiscanContext.Aprendiz.First(a => a.NumeroDocumentoAprendiz == id);
                _dbSiscanContext.Remove(aprendiz);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Aprendiz>> GetAll()
        {
            try
            {
                IQueryable<Aprendiz> queryAprendiz = _dbSiscanContext.Aprendiz.Include ( a => a.IdTipodocumentoNavigation).Include(x => x.IdEstadoTytNavigation).Include( z =>z.IdEstadoAprendizNavigation);
                return queryAprendiz;
            }
            catch { return null; }
        }

       
        public async Task<Aprendiz> GetForId(string id)
        {
            try
            {
                return await _dbSiscanContext.Aprendiz.FindAsync(id);
            }
            catch { return null; }
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
                _dbSiscanContext.Aprendiz.Update(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
