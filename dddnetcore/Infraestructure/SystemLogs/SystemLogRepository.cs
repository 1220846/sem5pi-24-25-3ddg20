
using DDDSample1.Domain.SystemLogs;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.SystemLogs
{
    public class SystemLogRepository : BaseRepository<SystemLog,SystemLogId> ,ISystemLogRepository{
        
        public SystemLogRepository(DDDSample1DbContext context):base(context.SystemLogs)
        {
           
        }

    }
}