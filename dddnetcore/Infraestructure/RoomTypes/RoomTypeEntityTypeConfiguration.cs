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

            builder.Property(b => b.Name).HasConversion(b => b.Name, b => new RoomTypeName(b))
            .IsRequired().HasMaxLength(255);

            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}