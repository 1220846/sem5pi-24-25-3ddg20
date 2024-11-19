using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.Staffs;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace dddnetcore.Infraestructure.Staffs
{
    public class StaffRepository : BaseRepository<Staff, StaffId>, IStaffRepository 
    {
        private readonly DDDSample1DbContext _context;

        public StaffRepository(DDDSample1DbContext context):base(context.Staffs) {
            _context = context;
        }

        public async Task<List<Staff>> GetStaffsAsync(string firstName = null, string lastName = null, string fullName = null, string email = null, Guid? specializationId = null,
            string phoneNumber = null, string id = null, string licenseNumber = null,
            string status = null, int pageNumber = 1, int pageSize = 10) {
            try {
                var query = _context.Staffs.AsQueryable();
    
                if (!string.IsNullOrEmpty(id))
                    query = query.Where(staff => staff.Id.Equals(new StaffId(id)));
                if (!string.IsNullOrEmpty(firstName))
                    query = query.Where(staff => staff.FirstName.Equals(new StaffFirstName(firstName)));
                if (!string.IsNullOrEmpty(lastName))
                    query = query.Where(staff => staff.LastName.Equals(new StaffLastName(lastName)));
                if (!string.IsNullOrEmpty(fullName))
                    query = query.Where(staff => staff.FullName.Equals(new StaffFullName(fullName)));
                if (!string.IsNullOrEmpty(email))
                    query = query.Where(staff => staff.ContactInformation.Email.Equals(new StaffEmail(email)));
                if (!string.IsNullOrEmpty(phoneNumber))
                    query = query.Where(staff => staff.ContactInformation.PhoneNumber.Equals(new StaffPhone(phoneNumber)));
                if (!string.IsNullOrEmpty(licenseNumber))
                    query = query.Where(staff => staff.LicenseNumber.Equals(new LicenseNumber(licenseNumber)));           
                if (!string.IsNullOrEmpty(status)) {
                    if (Enum.TryParse<StaffStatus>(status, true, out var parsedStatus)) {
                        query = query.Where(staff => staff.Status == parsedStatus);
                    } else
                        return new List<Staff>();
                }
                if (specializationId.HasValue && specializationId.Value != Guid.Empty)
                    query = query.Where(staff => staff.Specialization.Id.Equals(new SpecializationId(specializationId.Value)));
                query = query.Include(o => o.AvailabilitySlots);
                query = query.Include(o => o.Specialization);
                query = query.Include(o => o.User);

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();

            } catch (BusinessRuleValidationException) {
                throw new BusinessRuleValidationException("Filters badly formatted!");
            }
        }

        public async Task<Staff> UpdateAsync(Staff staff) {
            _context.Staffs.Update(staff);

            await _context.SaveChangesAsync();

            return staff;
        }

        public async Task<int> GetStaffsCountAsync() {
            return await _context.Staffs.CountAsync();
        }
    }
}