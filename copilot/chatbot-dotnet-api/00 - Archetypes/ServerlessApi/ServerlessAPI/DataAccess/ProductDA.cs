using Contract.Interfaces.Product;
using Contract.Models;

namespace DataAccess
{
    public class ProductDA : IProductDA
    {
        int nextId = 2;

        public List<Product> Products { get; }

        public ProductDA()
        {
            Products = [new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 1.99M }];
        }

        public async Task Create(Product product)
        {
            product.Id = nextId++;
            Products.Add(product);
        }

        public async Task Delete(int id)
        {
            var product = await Get(id).ConfigureAwait(false);
            if (product is null)
                return;

            Products.Remove(product);
        }

        public async Task<Product?> Get(int id)
        {
            return Products.Find(p => p.Id == id);
        }

        public async Task<List<Product>> GetAll()
        {
            return Products;
        }

        public async Task Update(int id, Product product)
        {
            var index = Products.FindIndex(p => p.Id == id);
            if (index == -1)
                return;

            Products[index] = product;
        }
    }
}
