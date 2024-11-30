using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.SurgeryRooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnetcore.Infraestructure.SurgeryRooms
{
    public class SurgeryRoomEntityTypeConfiguration : IEntityTypeConfiguration<SurgeryRoom>
    {
        public void Configure(EntityTypeBuilder<SurgeryRoom> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.RoomCapacity).HasConversion(b => b.Capacity, b => new SurgeryRoomCapacity(b)).IsRequired();
            builder.Property(b => b.MaintenanceSlots).HasConversion(b => b.MaintenanceSlots, b => new SurgeryRoomMaintenanceSlots(b)).IsRequired();
            builder.Property(b => b.AssignedEquipment).HasConversion(b => b.AssignedEquipment, b => new SurgeryRoomAssignedEquipment(b)).IsRequired();
            builder.HasOne(b => b.RoomType).WithMany().HasForeignKey("RoomTypeId").IsRequired();        
        }
    }
}