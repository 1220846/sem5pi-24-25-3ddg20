using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes
{
    public class RoomTypeService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IRoomTypeRepository _repo;

        public RoomTypeService(IUnitOfWork unitOfWork, IRoomTypeRepository repo){

            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<List<RoomTypeDto>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();
            
            List<RoomTypeDto> listDto = list.ConvertAll<RoomTypeDto>(roomType => 
                new RoomTypeDto{Id = roomType.Id.AsGuid(), Name = roomType.Name.Name});

            return listDto;
        }

        public async Task<RoomTypeDto> GetByIdAsync(RoomTypeId id)
        {
            var roomType = await this._repo.GetByIdAsync(id);
            
            if(roomType == null)
                return null;

            return new RoomTypeDto{Id = roomType.Id.AsGuid(), Name = roomType.Name.Name};
        }

        public async Task<RoomTypeDto> AddAsync(CreatingRoomTypeDto dto)
        {
            var roomType = new RoomType(new RoomTypeName(dto.Name));

            await this._repo.AddAsync(roomType);

            await this._unitOfWork.CommitAsync();

            return new RoomTypeDto { Id = roomType.Id.AsGuid(), Name = roomType.Name.Name};
        }
    }
}