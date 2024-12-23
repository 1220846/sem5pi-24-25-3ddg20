using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Infrastructure.OperationTypes
{
    internal class OperationTypeEntityTypeConfiguration: IEntityTypeConfiguration<OperationType>{

         public void Configure(EntityTypeBuilder<OperationType> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.Property(b => b.Name).HasConversion(b => b.Name, b => new OperationTypeName(b)).IsRequired().HasMaxLength(255);

            builder.HasIndex(b => b.Name).IsUnique();

            builder.Property(b => b.EstimatedDuration).HasConversion(b => b.Minutes, b => new EstimatedDuration(b)).IsRequired();

            builder.HasMany(b => b.OperationTypeSpecializations).WithOne(b => b.OperationType).HasForeignKey("OperationTypeId").OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.AnesthesiaTime).HasConversion(b => b.Minutes, b => new AnesthesiaTime(b)).IsRequired();

            builder.Property(b => b.CleaningTime).HasConversion(b => b.Minutes, b => new CleaningTime(b)).IsRequired();

            builder.Property(b => b.SurgeryTime).HasConversion(b => b.Minutes, b => new SurgeryTime(b)).IsRequired();

            builder.Property(b => b.OperationTypeStatus).HasConversion<string>().IsRequired();
        }
    }
}