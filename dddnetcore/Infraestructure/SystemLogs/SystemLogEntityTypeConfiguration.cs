using System;
using DDDSample1.Domain.SystemLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDSample1.Infrastructure.SystemLogs
{
    internal class SystemLogEntityTypeConfiguration: IEntityTypeConfiguration<SystemLog>{

         public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.Property(b => b.Operation).HasConversion(
                       b => b.ToString(), 
                       b => (Operation)Enum.Parse(typeof(Operation), b)).IsRequired();

            builder.Property(b => b.Entity).HasConversion(
                       b => b.ToString(), 
                       b => (Entity)Enum.Parse(typeof(Entity), b)).IsRequired();
            builder.Property(b => b.Content).IsRequired();
            builder.Property(b => b.EntityId).IsRequired();
            builder.Property(b  => b.Created_At).IsRequired();
        }
    }
}