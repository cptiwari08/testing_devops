using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Interfaces.Product
{
    public interface IProductDA
    {
        Task<List<Models.Product>> GetAll();
        Task<Models.Product?> Get(int id);
        Task Create(Models.Product product);
        Task Update(int id, Models.Product product);
        Task Delete(int id);
    }
}
