namespace Product.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyCollection<Domain.Entities.Product>> GetAllProductsAsync();
        Task<Domain.Entities.Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Domain.Entities.Product product);
        Task UpdateProductAsync(Domain.Entities.Product product);
        Task DeleteProductAsync(int id);
    }

}
