using Api.Data;
using Api.Models;

namespace Api.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        /*
         * A DB context would probably go here, but I'm using the provided employee list. 
         * I moved that list into a separate class called ProvidedDataProvider inside the Data folder. 
         * There's more info on why I did this is in that file and the IDataProvider file.
         * 
         */

        private readonly IDataProvider _dataProvider;

        public EmployeeRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await Task.FromResult(_dataProvider.GetEmployees());
        }
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dataProvider.GetEmployees().FirstOrDefault(e => e.Id == id));
        }

    }
}
