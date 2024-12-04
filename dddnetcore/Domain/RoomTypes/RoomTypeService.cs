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
                new RoomTypeDto{Code = roomType.Id.Code, Designation = roomType.Designation.Designation, Description = roomType.Description.Description, IsSurgical = roomType.IsSurgical.IsSurgical });

            return listDto;
        }

        public async Task<RoomTypeDto> GetByIdAsync(RoomTypeCode id)
        {
            var roomType = await this._repo.GetByIdAsync(id);
            
            if(roomType == null)
                return null;

            return new RoomTypeDto{Code = roomType.Id.Code, Designation = roomType.Designation.Designation, Description = roomType.Description.Description, IsSurgical = roomType.IsSurgical.IsSurgical };
        }

        public async Task<RoomTypeDto> AddAsync(CreatingRoomTypeDto dto)
        {
            var roomType = new RoomType(new RoomTypeCode(dto.Code),new RoomTypeDesignation(dto.Designation), new RoomTypeDescription(dto.Description), new RoomTypeIsSurgical(dto.IsSurgical));

            await this._repo.AddAsync(roomType);

            await this._unitOfWork.CommitAsync();

            return new RoomTypeDto { Code = roomType.Id.Code, Designation = roomType.Designation.Designation, Description = roomType.Description.Description, IsSurgical = roomType.IsSurgical.IsSurgical };
        }
    }
}