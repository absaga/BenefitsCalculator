namespace Api.Repositories
{
    public interface IRepository<T> where T : class
    {
        /*
         * I'm creating a repository layer to handle communication with the data provider.
         * This should make it so that I can test or swap the repository separately.
         * 
         * Normally I would have a method for each CRUD operation, but for now I'll only use GetAll and GetById.
         */

        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
    }
}
