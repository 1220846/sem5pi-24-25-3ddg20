using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.OperationRequests;

namespace DDDSample1.Infrastructure.OperationRequests
{
    internal class OperationRequestEntityTypeConfiguration: IEntityTypeConfiguration<OperationRequest>{

         public void Configure(EntityTypeBuilder<OperationRequest> builder)
        {
            // cf. https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx
            
            //builder.ToTable("Categories", SchemaNames.DDDSample1);
            builder.HasKey(b => b.Id);
            //builder.Property<bool>("_active").HasColumnName("Active");

            /*builder.Property(b => b.Name).HasConversion(b => b.Name, b => new SpecializationName(b))
            .IsRequired().HasMaxLength(255);

            builder.HasMany(s => s.OperationTypeSpecializations)
                .WithOne().HasForeignKey("SpecializationId").OnDelete(DeleteBehavior.Cascade);
*/
        builder.OwnsOne(o => o.DeadlineDate, d =>{
                d.Property(p => p.Date)
                 .HasColumnName("DeadlineDate")
                 .IsRequired(); 
            });

            builder.Property(o => o.Priority)
                   .IsRequired(); 
        }
    }
}