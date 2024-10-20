using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Infrastructure.Patients
{
    internal class AnonymizedPatientDataEntityTypeConfiguration: IEntityTypeConfiguration<AnonymizedPatientData>{

        public void Configure(EntityTypeBuilder<AnonymizedPatientData> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.AgeRange).HasMaxLength(25).IsRequired();

            builder.Property(b => b.Gender).HasMaxLength(25).IsRequired();

            builder.Property(b => b.MedicalConditions).IsRequired();

            builder.Property(b => b.AppointmentHistory).IsRequired();

            builder.Property(b => b.AnonymizedDate).IsRequired();
        }
    }
}