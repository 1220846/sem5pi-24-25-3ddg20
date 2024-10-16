using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dddnetcore.Infraestructure.Staffs
{
    internal class StaffEntiryTypeConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder) {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.FirstName).HasConversion(b => b.Name, b => new StaffFirstName(b));
            builder.Property(b => b.LastName).HasConversion(b => b.Name, b => new StaffLastName(b));
            builder.Property(b => b.FullName).HasConversion(b => b.Name, b => new StaffFullName(b));
            builder.Property(b => b.ContactInformation).HasConversion(b => $"{b.Email.Email} {b.PhoneNumber.PhoneNumber}", b => new StaffContactInformation(new StaffEmail(b.Split()[0]),new StaffPhone(b.Split()[1])));
            builder.Property(b => b.LicenseNumber).HasConversion(b => b.Number, b => new LicenseNumber(b));
            builder.HasIndex(b => b.LicenseNumber).IsUnique();
            builder.HasMany(b => b.AvailabilitySlots).WithOne().HasForeignKey(b => b.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Specialization).WithMany().HasForeignKey(b => b.Id);
        }
    }
}