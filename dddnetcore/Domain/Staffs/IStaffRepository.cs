using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;

namespace DDDSample1.DataAnnotations.Staffs{
    public interface IStaffRepository : IRepository<Staff, StaffId> {}
}