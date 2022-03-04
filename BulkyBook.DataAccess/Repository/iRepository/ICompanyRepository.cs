using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.iRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        // Methods like update because they have different implementations depending on the repository we will implement them here instead of in IRepository
        void Update(Company obj);
    }
}
