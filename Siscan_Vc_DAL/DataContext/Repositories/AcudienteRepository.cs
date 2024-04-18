using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.DataContext.Repositories
{
    public class AcudienteRepository:IGensericRepositoryInt<Acudientes>
    {
        private readonly DbSiscanContext _dbcontext;
        public AcudienteRepository(DbSiscanContext dbcontext)
        {
            _dbcontext = dbcontext;   
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Acudientes acudientes= _dbcontext.Acudientes.FirstOrDefault(a=> a.IdAcudiente==id);
                _dbcontext.Acudientes.Remove(acudientes);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public Task<IQueryable<Acudientes>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Acudientes> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(Acudientes modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Acudientes modelo)
        {
            throw new NotImplementedException();
        }
    }
}
