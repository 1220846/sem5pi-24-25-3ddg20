using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoomService
    {
        IUnitOfWork _unitOfWork;
        private readonly ISurgeryRoomRepository _repo;

        public SurgeryRoomService(IUnitOfWork unitOfWork, ISurgeryRoomRepository repo) {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
        }

        public async Task<SurgeryRoomDto> GetByIdAsync(RoomNumber id) {
            var surgeryRoom = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Surgery Room with Id: {id}");
            return new SurgeryRoomDto( surgeryRoom);
        }

        public async Task<List<SurgeryRoomDto>> GetAllAsync() {
            var list = await this._repo.GetAllAsync();
            return list.ConvertAll<SurgeryRoomDto>(SurgeryRoom => new SurgeryRoomDto(SurgeryRoom));
        }
    }
}