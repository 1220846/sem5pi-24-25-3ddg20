using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Specializations;
using dddnetcore.Domain.Specializations;

namespace DDDSample1.Infrastructure.Specializations
{
    internal class SpecializationEntityTypeConfiguration: IEntityTypeConfiguration<Specialization>{

         public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.Property(b => b.Name).HasConversion(b => b.Name, b => new SpecializationName(b)).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Code).HasConversion(b => b.Code, b => new SpecializationCode(b)).IsRequired();
            builder.Property(b => b.Description).HasConversion(b => b.Description, b => new SpecializationDescription(b)).IsRequired(false);

            builder.HasIndex(b => b.Name).IsUnique();

            builder.HasMany(s => s.OperationTypeSpecializations)
                .WithOne().HasForeignKey("SpecializationId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}