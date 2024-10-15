using System;
using Xunit;
using dddnetcore.Domain.AvailabilitySlots;

public class AvailabilitySlotIdTests
{
    [Fact]
    public void CreateAvailabilitySlotId_FromGuid_ShouldSucceed()
    {
        
        var guid = Guid.NewGuid();

       
        var availabilitySlotId = new AvailabilitySlotId(guid);

        
        Assert.NotNull(availabilitySlotId);
        Assert.Equal(guid, availabilitySlotId.AsGuid());
    }

    [Fact]
    public void CreateAvailabilitySlotId_FromString_ShouldSucceed()
    {
        
        var guidString = Guid.NewGuid().ToString();

       
        var availabilitySlotId = new AvailabilitySlotId(guidString);

        
        Assert.NotNull(availabilitySlotId);
        Assert.Equal(guidString, availabilitySlotId.AsString());
    }

    [Fact]
    public void AsGuid_ShouldReturnGuid()
    {
        
        var guid = Guid.NewGuid();
        var availabilitySlotId = new AvailabilitySlotId(guid);

       
        var result = availabilitySlotId.AsGuid();

        
        Assert.Equal(guid, result);
    }

    [Fact]
    public void AsString_ShouldReturnStringRepresentationOfGuid()
    {
        
        var guid = Guid.NewGuid();
        var availabilitySlotId = new AvailabilitySlotId(guid);

       
        var result = availabilitySlotId.AsString();

        
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void CreateAvailabilitySlotId_FromInvalidString_ShouldThrowFormatException()
    {
        
        var invalidString = "invalid-guid";

        
        Assert.Throws<FormatException>(() => new AvailabilitySlotId(invalidString));
    }
}