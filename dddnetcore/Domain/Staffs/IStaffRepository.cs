using DDDSample1.Domain.Shared;

namespace DDDSample1.DataAnnotations.Staffs{
    public interface IStaffRepository : IStaffRepository<Staffs, LicenseNumber> {}
}