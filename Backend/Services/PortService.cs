using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class PortService : IPortService
    {
        private readonly IPortRepository _portRepository;

        public PortService(IPortRepository portRepository)
        {
            _portRepository = portRepository;
        }

        public int Create(PortDto portDto)
        {
            // pohui let it be
            var ownerId = 1;

            var port = new Port
            {
                Name = portDto.Name,
                Description = portDto.Description,
                IsApproved = portDto.IsApproved,
                CreatedOn = DateTime.UtcNow,
                OwnerId = ownerId,
            };

            return _portRepository.Create(port);
        }

        public void Delete(int id)
        {
            if (!_portRepository.CheckIfExistsById(id))
            {
                throw new KeyNotFoundException($"Port with ID {id} not found.");
            }

            _portRepository.Delete(id);
        }
    }
}
