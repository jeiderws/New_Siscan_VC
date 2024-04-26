using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class InstructorRepository : IGenericRepository<Instructor>
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public InstructorRepository(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public async Task<bool> Delete(string numDoc)
        {
            try
            {
                Instructor instructor = _dbSiscanContext.Instructors.First(i => i.NumeroDocumentoInstructor == numDoc);
                _dbSiscanContext.Remove(instructor);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Instructor>> GetAll()
        {
            try
            {
                IQueryable<Instructor> queryInstructor = _dbSiscanContext.Instructors;
                return queryInstructor;
            }
            catch { return null; }
        }

        public async Task<Instructor> GetForDoc(string numeroDoc)
        {
            try
            {
                return await _dbSiscanContext.Instructors.FindAsync(numeroDoc);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Instructor model)
        {
            try
            {
                _dbSiscanContext.Instructors.Add(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Instructor model)
        {
            try
            {
                _dbSiscanContext.Instructors.Update(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
