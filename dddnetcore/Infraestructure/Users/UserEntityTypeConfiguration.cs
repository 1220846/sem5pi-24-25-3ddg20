using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Users;
using System;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Infrastructure.Users
{
    internal class UserEntityTypeConfiguration: IEntityTypeConfiguration<User>{

         public void Configure(EntityTypeBuilder<User> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.Property(b => b.Email).HasConversion(b => b.Address, b => new Email(b)).IsRequired();

            builder.HasIndex(b => b.Email)
                   .IsUnique();

            builder.Property(b => b.Role).HasConversion(
                       v => v.ToString(), 
                       v => (Role)Enum.Parse(typeof(Role), v)).IsRequired();

            builder.HasOne(b => b.Staff).WithOne(b => b.User).HasForeignKey<Staff>(b => b.Username);
            builder.HasOne(b => b.Patient).WithOne(b => b.User).HasForeignKey<Patient>(b => b.Username);
        }
    }
}