using DDDSample1.Domain.RoomTypes;
using Xunit;

public class RoomTypeDescriptionTests
{
    [Fact]
    public void CreateValidRoomTypeDescriptionShouldSucceed()
    {
        
        var validDescription = "This is a valid description.";

        
        var roomTypeDescription = new RoomTypeDescription(validDescription);

        
        Assert.NotNull(roomTypeDescription);
        Assert.Equal(validDescription, roomTypeDescription.Description);
    }

    [Fact]
    public void CreateNullDescriptionShouldSucceed()
    {
        
        string nullDescription = null;

        
        var roomTypeDescription = new RoomTypeDescription(nullDescription);

        
        Assert.NotNull(roomTypeDescription);
        Assert.Null(roomTypeDescription.Description);
    }

    [Fact]
    public void EqualsSameDescriptionShouldReturnTrue()
    {
        
        var description1 = new RoomTypeDescription("Same description");
        var description2 = new RoomTypeDescription("Same description");

        
        var areEqual = description1.Equals(description2);

        
        Assert.True(areEqual);
    }

    [Fact]
    public void EqualsDifferentDescriptionShouldReturnFalse()
    {
        
        var description1 = new RoomTypeDescription("Description 1");
        var description2 = new RoomTypeDescription("Description 2");

        
        var areEqual = description1.Equals(description2);

        
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCodeSameDescriptionShouldReturnSameHash()
    {
        
        var description1 = new RoomTypeDescription("Same description");
        var description2 = new RoomTypeDescription("Same description");

        
        var hash1 = description1.GetHashCode();
        var hash2 = description2.GetHashCode();

        
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCodeDifferentDescriptionShouldReturnDifferentHash()
    {
        
        var description1 = new RoomTypeDescription("Description 1");
        var description2 = new RoomTypeDescription("Description 2");

        
        var hash1 = description1.GetHashCode();
        var hash2 = description2.GetHashCode();

        
        Assert.NotEqual(hash1, hash2);
    }
}
