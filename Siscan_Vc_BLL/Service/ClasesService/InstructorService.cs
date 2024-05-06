using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.ClasesService
{
    public class InstructorService : IInstructorService
    {
        private readonly IGenericRepository<Instructor> _instructorrepo;
        public InstructorService(IGenericRepository<Instructor> instructorrepo)
        {
            _instructorrepo = instructorrepo;
        }
        public Task<bool> Delete(string numDoc)
        {
            return _instructorrepo.Delete(numDoc);
        }

        public Task<IQueryable<Instructor>> GetAll()
        {
            return _instructorrepo.GetAll();
        }

        public Task<Instructor> GetForDoc(string numeroDoc)
        {
            return _instructorrepo.GetForId(numeroDoc);
        }

        public async Task<Instructor> GetForName(string name)
        {
            try
            {
                IQueryable<Instructor> queryInstructor=await _instructorrepo.GetAll();
                Instructor instructor=queryInstructor.Where(i=>i.NombreInstructor==name).FirstOrDefault();
                return instructor;
            }
            catch { return null; }
        }

        public Task<bool> Insert(Instructor model)
        {
            return _instructorrepo.Insert(model);
        }

        public Task<bool> Update(Instructor model)
        {
            return _instructorrepo.Update(model);
        }
    }
}
