using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.OperationRequests;
using System;

namespace DDDSample1.Infrastructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration: IEntityTypeConfiguration<OperationRequest>{

         public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            builder.OwnsOne(o => o.DeadlineDate, d =>{d.Property(p => p.Date).HasColumnName("DeadlineDate").IsRequired();});

            builder.Property(o => o.Priority).HasConversion(p => p.ToString(), p => (Priority)Enum.Parse(typeof(Priority), p)).HasColumnName("Priority").IsRequired();

            builder.Property(o => o.Status).HasConversion(s => s.ToString(), s => (OperationRequestStatus)Enum.Parse(typeof(OperationRequestStatus), s)).HasColumnName("Status").IsRequired();

            builder.Property(o => o.OperationTypeId).IsRequired();

            builder.OwnsOne(o => o.MedicalRecordNumber, mrn =>{mrn.Property(p => p.Id).HasColumnName("MedicalRecordNumber").IsRequired();});

            builder.Property(o => o.StaffId).IsRequired();
        }
    }
}