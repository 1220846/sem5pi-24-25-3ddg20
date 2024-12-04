using System;
using DDDSample1.Domain.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnetcore.Infraestructure.Staffs
{
    internal class StaffEntityTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder) {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.FirstName).HasConversion(b => b.Name, b => new StaffFirstName(b)).IsRequired();
            builder.Property(b => b.LastName).HasConversion(b => b.Name, b => new StaffLastName(b)).IsRequired();
            builder.Property(b => b.FullName).HasConversion(b => b.Name, b => new StaffFullName(b)).IsRequired();
            //builder.Property(b => b.ContactInformation).HasConversion(b => $"{b.Email.Email} {b.PhoneNumber.PhoneNumber}", b => new StaffContactInformation(new StaffEmail(b.Split()[0]),new StaffPhone(b.Split()[1])));
            //builder.HasIndex(b => b.ContactInformation).IsUnique();
            builder.OwnsOne(b => b.ContactInformation, ci =>
            {
                ci.Property(c => c.Email).HasColumnName("Email").HasConversion(b => b.Email, b => new StaffEmail(b)).IsRequired();
                ci.HasIndex(c => c.PhoneNumber).IsUnique();
                ci.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber").HasConversion(b => b.PhoneNumber, b => new StaffPhone(b)).IsRequired();
                ci.HasIndex(c => c.PhoneNumber).IsUnique();
            });
            builder.Property(b => b.LicenseNumber).HasConversion(b => b.Number, b => new LicenseNumber(b));
            builder.HasIndex(b => b.LicenseNumber).IsUnique();
            builder.Property(b => b.Status).HasConversion<string>().IsRequired();
            builder.HasMany(b => b.AvailabilitySlots).WithOne().HasForeignKey("StaffId").OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Specialization).WithMany().HasForeignKey("SpecializationId").IsRequired();
            builder.HasOne(b => b.User).WithOne(b => b.Staff).HasForeignKey<Staff>(b => b.Username).IsRequired();
            builder.HasMany(b => b.AppointmentStaffs).WithOne(b => b.Staff).HasForeignKey("StaffId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}