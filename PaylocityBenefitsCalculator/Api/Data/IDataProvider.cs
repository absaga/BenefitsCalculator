using Api.Models;

namespace Api.Data
{
    public interface IDataProvider
    {
        /*
         * I'm creating this to put the already provided list of employees and dependents.
         * I'm thinking that if I want to later change the data or create some type of automated testing that
         * creates different lists of employees and dependents, I could just create a new data provider 
         * with the mock data and inject it.
         * 
         */

        List<Employee> GetEmployees();
        List<Dependent> GetDependents();
    }
}
