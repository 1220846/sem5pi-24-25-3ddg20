using System;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Staffs;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Shared;

namespace dddnetcore.Infraestructure.Staffs
{
    public class StaffRepository : BaseRepository<Staff, StaffId>, IStaffRepository 
    {
        private readonly DDDSample1DbContext _context;

        public StaffRepository(DDDSample1DbContext context):base(context.Staffs) {
            _context = context;
        }
    }
}