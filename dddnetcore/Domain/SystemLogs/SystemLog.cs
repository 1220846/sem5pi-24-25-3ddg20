using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.SystemLogs{

    public class SystemLog : Entity<SystemLogId>, IAggregateRoot
    {
        public Operation Operation { get;  private set; }
        public Entity Entity { get;  private set; }
        public String Content { get;  private set; }
        public String EntityId { get;  private set; }
        public DateTime Created_At { get;  private set; }
        public SystemLog(Operation operation,Entity entity, String content, String entityId){
            this.Id = new SystemLogId(Guid.NewGuid());
            this.Operation = operation;
            this.Entity = entity;
            this.Content = content;
            this.EntityId = entityId;
            this.Created_At = DateTime.Now;
        }

        protected SystemLog(){
        
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (SystemLog)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}