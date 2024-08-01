using Api.Models;

namespace Api.Repositories
{
    /*
     * I'm creating a separate repository for dependents as I'll need a way to get these by
     * the employee ID. This method would not be needed in the employee repository.
     * 
     */

    public interface IDependentRepository : IRepository<Dependent>
    {
        Task<List<Dependent>> GetByEmployeeId(int employeeId);
    }
}
