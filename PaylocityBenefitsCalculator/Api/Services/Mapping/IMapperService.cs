using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Mapping
{
    public interface IMapperService
    {
        /*
         * I'm implement a basic mapping service as I'm not too happy with doing this inside the services.
         * This also seems simple enough to do it without any library, but we'll see if that changes.
         * 
         */
        GetEmployeeDto MapEmployeeToDto(Employee employee);
        GetDependentDto MapDependentToDto(Dependent dependent);
    }
}
