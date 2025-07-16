using Contract.Interfaces.Product;
using Contract.Models;

namespace BusinessLogic
{
    public class ProductBL : IProductBL
    {
        private readonly IProductDA _productDA;

        public ProductBL(IProductDA productDA)
        {
            _productDA = productDA ?? throw new ArgumentNullException(nameof(productDA));
        }

        public async Task Create(Product product)
        {
            await _productDA.Create(product).ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            await _productDA.Delete(id).ConfigureAwait(false);
        }

        public async Task<Product?> Get(int id)
        {
            return await _productDA.Get(id).ConfigureAwait(false);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productDA.GetAll().ConfigureAwait(false);
        }

        public async Task Update(int id, Product product)
        {
            await _productDA.Update(id, product).ConfigureAwait(false);
        }
    }
}
