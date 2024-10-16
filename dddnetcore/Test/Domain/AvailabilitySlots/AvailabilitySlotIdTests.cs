using System;
using Xunit;
using dddnetcore.Domain.AvailabilitySlots;

public class AvailabilitySlotIdTests
{
    [Fact]
    public void CreateAvailabilitySlotIdFromGuidShouldSucceed()
    {
        
        var guid = Guid.NewGuid();

       
        var availabilitySlotId = new AvailabilitySlotId(guid);

        
        Assert.NotNull(availabilitySlotId);
        Assert.Equal(guid, availabilitySlotId.AsGuid());
    }

    [Fact]
    public void CreateAvailabilitySlotIdFromStringShouldSucceed()
    {
        
        var guidString = Guid.NewGuid().ToString();

       
        var availabilitySlotId = new AvailabilitySlotId(guidString);

        
        Assert.NotNull(availabilitySlotId);
        Assert.Equal(guidString, availabilitySlotId.AsString());
    }

    [Fact]
    public void AsGuidShouldReturnGuid()
    {
        
        var guid = Guid.NewGuid();
        var availabilitySlotId = new AvailabilitySlotId(guid);

       
        var result = availabilitySlotId.AsGuid();

        
        Assert.Equal(guid, result);
    }

    [Fact]
    public void AsStringShouldReturnStringRepresentationOfGuid()
    {
        
        var guid = Guid.NewGuid();
        var availabilitySlotId = new AvailabilitySlotId(guid);

       
        var result = availabilitySlotId.AsString();

        
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void CreateAvailabilitySlotIdFromInvalidStringShouldThrowFormatException()
    {
        
        var invalidString = "invalid-guid";

        
        Assert.Throws<FormatException>(() => new AvailabilitySlotId(invalidString));
    }
}