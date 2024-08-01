using Api.Dtos.Dependent;

namespace Api.Services
{
    public interface IDependentService
    {
        /*
         * This service will be used to handle the business logic for dependents.
         * This should communicate between the controller and the repository.
         * 
         */

        Task<GetDependentDto?> GetByIdAsync(int id);
        Task<List<GetDependentDto>> GetAllAsync();
    }
}
