using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.OperationRequests;
using System;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Patients;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Infrastructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration: IEntityTypeConfiguration<OperationRequest>{

         public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {

            builder.HasKey(o=>o.Id);

            builder.Property(o=>o.DeadlineDate).HasConversion(o=>o.Date, o=>new DeadlineDate(o)).IsRequired();

            builder.Property(b => b.Priority).HasConversion<string>().IsRequired();

            builder.Property(b => b.Status).HasConversion<string>().IsRequired();

            builder.Property(o=>o.StaffId).HasConversion(o=>o.Id, o=>new StaffId(o)).IsRequired();
            
            builder.Property(o=>o.MedicalRecordNumber).HasConversion(o=>o.Id, o=>new MedicalRecordNumber(o)).IsRequired();
            
            builder.Property(o=>o.OperationTypeId).HasConversion(o=>o.Value, o=>new OperationTypeId(o)).IsRequired();
           
        }
    }
}