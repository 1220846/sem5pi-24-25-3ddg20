using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.RoomTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.SurgeryRooms;

namespace dddnetcore.Domain.SurgeryRooms
{
    public class SurgeryRoomService
    {
        IUnitOfWork _unitOfWork;
        private readonly ISurgeryRoomRepository _repo;

        private readonly IRoomTypeRepository _roomTypeRepo;

        public SurgeryRoomService(IUnitOfWork unitOfWork, ISurgeryRoomRepository repo, IRoomTypeRepository roomTypeRepository) {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._roomTypeRepo = roomTypeRepository;
        }

        public async Task<SurgeryRoomDto> GetByIdAsync(RoomNumber id) {
            var surgeryRoom = await this._repo.GetByIdAsync(id) ?? throw new NullReferenceException($"Not Found Surgery Room with Id: {id}");
            return new SurgeryRoomDto( surgeryRoom);
        }

        public async Task<List<SurgeryRoomDto>> GetAllAsync() {
            var list = await this._repo.GetAllAsync();
            return list.ConvertAll<SurgeryRoomDto>(SurgeryRoom => new SurgeryRoomDto(SurgeryRoom));
        }

        public async Task<SurgeryRoomDto> AddAsync(CreatingSurgeryRoomDto dto)
        {
            
            var roomType = await this._roomTypeRepo.GetByIdAsync(new RoomTypeId(dto.RoomTypeId)) ?? throw new NullReferenceException($"Not Found Room Type with Id: {dto.RoomTypeId}");

            var currentStatus = Enum.Parse<SurgeryRoomCurrentStatus>(dto.CurrentStatus.ToUpper());

            var surgerRoom = new SurgeryRoom(new RoomNumber(dto.Number),roomType, new SurgeryRoomCapacity(dto.Capacity),
                                new SurgeryRoomMaintenanceSlots(dto.MaintenanceSlots),new SurgeryRoomAssignedEquipment(dto.AssignedEquipment),currentStatus);

            await this._repo.AddAsync(surgerRoom);

            await this._unitOfWork.CommitAsync();

           return new SurgeryRoomDto(surgerRoom);
        }
    }
}