using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.SystemLogs
{
    public interface ISystemLogRepository: IRepository<SystemLog, SystemLogId>
    {

    }
}