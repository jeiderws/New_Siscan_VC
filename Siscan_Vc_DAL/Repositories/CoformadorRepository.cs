using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class CoformadorRepository : IGenericRepository<Coformador>
    {
        private readonly DbSiscanContext _dbcontext;
        public CoformadorRepository(DbSiscanContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Coformador coformador = _dbcontext.Coformadors.First(c => c.IdCoformador.ToString() == id);
                _dbcontext.Remove(coformador);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Coformador>> GetAll()
        {
            try
            {
                IQueryable<Coformador> queryCoformador = _dbcontext.Coformadors.Include(a => a.NitEmpresaNavigation);
                return queryCoformador;
            }
            catch { return null; }
        }

        public async Task<Coformador> GetForId(string id)
        {
            try
            {
                return await _dbcontext.Coformadors.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Coformador model)
        {
            try
            {
                _dbcontext.Coformadors.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Coformador model)
        {
            try
            {
                _dbcontext.Coformadors.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
