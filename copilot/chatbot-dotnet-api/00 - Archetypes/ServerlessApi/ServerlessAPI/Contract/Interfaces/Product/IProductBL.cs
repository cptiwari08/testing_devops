namespace Contract.Interfaces.Product
{
    public interface IProductBL
    {
        Task<List<Models.Product>> GetAll();
        Task<Models.Product?> Get(int id);
        Task Create(Models.Product product);
        Task Update(int id, Models.Product product);
        Task Delete(int id);
    }
}
