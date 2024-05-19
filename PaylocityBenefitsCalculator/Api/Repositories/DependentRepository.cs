using Api.Data;
using Api.Models;

namespace Api.Repositories
{
    public class DependentRepository : IDependentRepository
    {
        /*
         * A DB context would probably go here, but I'm using the provided employee list. 
         * I moved that list into a separate class called ProvidedDataProvider inside the Data folder. 
         * There's more info on why I did this is in that file and the IDataProvider file.
         * 
         */

        private readonly IDataProvider _dataProvider;

        public DependentRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public async Task<List<Dependent>> GetAllAsync()
        {
            return await Task.FromResult(_dataProvider.GetDependents());
        }
        public async Task<Dependent?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dataProvider.GetDependents().FirstOrDefault(d => d.Id == id));
        }
        public async Task<List<Dependent>> GetByEmployeeId(int employeeId)
        {
            return await Task.FromResult(_dataProvider.GetDependents().Where(d => d.EmployeeId == employeeId).ToList());
        }




    }
}
