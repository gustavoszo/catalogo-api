namespace CatalogoApi.Repositories
{
    public interface IUnitOfWork
    {

        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }

        void Commit();

    }
}
