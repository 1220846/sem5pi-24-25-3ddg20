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
            builder.HasOne(b => b.SurgeryRoom).WithOne().HasForeignKey<Appointment>("RoomNumber").IsRequired();
            builder.Property(b => b.Status).HasConversion<string>().IsRequired();
            builder.Property(b=>b.DateAndTime).HasConversion(b=>b.DateAndTime, b=>new AppoitmentDateAndTime(b)).IsRequired();
        }
    }
}