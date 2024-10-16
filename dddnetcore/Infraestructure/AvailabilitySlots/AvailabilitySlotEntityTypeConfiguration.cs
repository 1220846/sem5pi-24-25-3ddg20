using System;
using dddnetcore.Domain.AvailabilitySlots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dddnetcore.Infraestructure.AvailabilitySlots
{
    internal class AvailabilitySlotEntityTypeConfiguration : IEntityTypeConfiguration<AvailabilitySlot>
    {
        public void Configure(EntityTypeBuilder<AvailabilitySlot> builder) {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.StartTime).HasConversion(b => b.Time, b => new StartTime(b)).IsRequired();
            builder.Property(b => b.EndTime).HasConversion(b => b.Time, b => new EndTime(b)).IsRequired();
        }
    }
}