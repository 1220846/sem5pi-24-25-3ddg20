using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.AppointmentsStaffs;

namespace DDDSample1.Infrastructure.AppointmentsStaffs
{
    internal class AppointmentStaffEntityTypeConfiguration: IEntityTypeConfiguration<AppointmentStaff>{

         public void Configure(EntityTypeBuilder<AppointmentStaff> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.HasOne(b => b.Appointment)
            .WithMany(b => b.AppointmentStaffs)
            .HasForeignKey("AppointmentId")
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Staff)
            .WithMany(b => b.AppointmentStaffs)
            .HasForeignKey("StaffId")
            .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}