using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.RoomTypes;

namespace DDDSample1.Infrastructure.RoomTypes
{
    internal class RoomTypeEntityTypeConfiguration: IEntityTypeConfiguration<RoomType>{

         public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            builder.Property(b => b.Designation).HasConversion(b => b.Designation, b => new RoomTypeDesignation(b))
            .IsRequired().HasMaxLength(100);

            builder.HasIndex(b => b.Designation).IsUnique();

            builder.Property(b => b.Description).HasConversion(b => b.Description, b => new RoomTypeDescription(b)).IsRequired(false); 

            builder.Property(b => b.IsSurgical)
            .HasConversion(b => b.IsSurgical,b => new RoomTypeIsSurgical(b)).IsRequired();
        }
    }
}