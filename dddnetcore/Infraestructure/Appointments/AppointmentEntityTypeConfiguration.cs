using System;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.OperationRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace dddnetcore.Infraestructure.Appointments
{
    internal class AppointmentEntityTypeConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder) {
            builder.HasKey(b => b.Id);
            builder.HasOne(b => b.OperationRequest).WithOne().HasForeignKey<Appointment>("OperationRequestId").IsRequired();
            builder.HasIndex(b => b.OperationRequestId).IsUnique();
            builder.HasOne(b => b.SurgeryRoom).WithMany().HasForeignKey("RoomNumber").IsRequired();
            builder.Property(b => b.Status).HasConversion<string>().IsRequired();
            builder.Property(b => b.DateAndTime).HasConversion(b=> b.DateAndTime, b => new AppointmentDateAndTime(b)).IsRequired();
            builder.HasMany(b => b.AppointmentStaffs).WithOne(b => b.Appointment).HasForeignKey("AppointmentId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}