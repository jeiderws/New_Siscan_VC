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
            try {
                Coformador coformador = _dbcontext.Coformadors.First(c=>c.IdCoformador.ToString()==id);
                _dbcontext.Remove(coformador);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public Task<IQueryable<Coformador>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Coformador> GetForId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(Coformador model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Coformador model)
        {
            throw new NotImplementedException();
        }
    }
}
