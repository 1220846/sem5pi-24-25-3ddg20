using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;

namespace DDDSample1.DataAnnotations.Staffs{
    public interface IStaffRepository : IRepository<Staff, StaffId> {

    public Task<List<Staff>> GetStaffsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
        string phoneNumber = null, string id = null, string licenseNumber = null, 
        string status = null, int pageNumber = 1, int pageSize = 10);
    
    public Task<Staff> UpdateAsync(Staff staff);
    }
}