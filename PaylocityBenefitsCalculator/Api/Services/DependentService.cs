using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;
using Api.Services.Mapping;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        /*
         * I'm creating this service to handle communication with the controller and the repository.
         * What I'm thinking is that by abstracting each layer it will make it easier to both
         * test each one independently (or expand them in the future).
         * 
         */

        private readonly IDependentRepository _dependentRepository;
        private readonly IMapperService _mapperService;
        public DependentService(IDependentRepository dependentRepository, IMapperService mapperService)
        {
            _dependentRepository = dependentRepository;
            _mapperService = mapperService;
        }
        public async Task<GetDependentDto?> GetByIdAsync(int id)
        {
            var dependent = await _dependentRepository.GetByIdAsync(id);

            if (dependent == null)
            {
                return null;
            }

            return _mapperService.MapDependentToDto(dependent);
        }
        public async Task<List<GetDependentDto>> GetAllAsync()
        {
            var dependents = await _dependentRepository.GetAllAsync();
            return dependents.Select(_mapperService.MapDependentToDto).ToList();
        }
    }
}
