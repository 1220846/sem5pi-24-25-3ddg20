using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Users;

namespace DDDSample1.Infrastructure.Patients
{
    internal class PatientEntityTypeConfiguration: IEntityTypeConfiguration<Patient>{

         public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.FirstName).HasConversion(b => b.Name, b => new PatientFirstName(b)).IsRequired();
            builder.Property(b => b.LastName).HasConversion(b => b.Name, b => new PatientLastName(b)).IsRequired();
            builder.Property(b => b.FullName).HasConversion(b => b.Name, b => new PatientFullName(b)).IsRequired();
            builder.Property(b => b.ContactInformation).HasConversion(b => $"{b.Email.Email} {b.PhoneNumber.PhoneNumber}", b => new PatientContactInformation(new PatientEmail(b.Split()[0]),new PatientPhone(b.Split()[1])));
            builder.HasIndex(b => b.ContactInformation).IsUnique();
            builder.Property(b => b.AppointmentHistory).HasConversion(b => b.History, b => new AppointmentHistory(b));
            builder.Property(b => b.DateOfBirth).HasConversion(b => b.Date, b => new DateOfBirth(b));
            builder.Property(b => b.EmergencyContact).HasConversion(b => b.PhoneNumber, b => new EmergencyContact(b));
            builder.Property(b => b.Gender).HasConversion<string>().IsRequired();
            builder.Property(b => b.MedicalConditions).HasConversion(b => b.Conditions, b => new MedicalConditions(b));
            builder.HasOne(b => b.User).WithOne().HasForeignKey<Patient>(b => b.Username).IsRequired();
        }
    }
}