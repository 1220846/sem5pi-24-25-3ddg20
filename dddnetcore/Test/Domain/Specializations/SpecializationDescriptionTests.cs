using System;
using Xunit;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.Shared;

public class SpecializationDescriptionTests
{
    [Fact]
    public void CreateValidSpecializationDescriptionShouldSucceed()
    {
        
        var validDescription = "This is a valid description.";

        
        var specializationDescription = new SpecializationDescription(validDescription);

        
        Assert.NotNull(specializationDescription);
        Assert.Equal(validDescription, specializationDescription.Description);
    }

    [Fact]
    public void CreateNullDescriptionShouldSucceed()
    {
        
        string nullDescription = null;

        
        var specializationDescription = new SpecializationDescription(nullDescription);

        
        Assert.NotNull(specializationDescription);
        Assert.Null(specializationDescription.Description);
    }

    [Fact]
    public void EqualsSameDescriptionShouldReturnTrue()
    {
        
        var description1 = new SpecializationDescription("Same description");
        var description2 = new SpecializationDescription("Same description");

        
        var areEqual = description1.Equals(description2);

        
        Assert.True(areEqual);
    }

    [Fact]
    public void EqualsDifferentDescriptionShouldReturnFalse()
    {
        
        var description1 = new SpecializationDescription("Description 1");
        var description2 = new SpecializationDescription("Description 2");

        
        var areEqual = description1.Equals(description2);

        
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCodeSameDescriptionShouldReturnSameHash()
    {
        
        var description1 = new SpecializationDescription("Same description");
        var description2 = new SpecializationDescription("Same description");

        
        var hash1 = description1.GetHashCode();
        var hash2 = description2.GetHashCode();

        
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCodeDifferentDescriptionShouldReturnDifferentHash()
    {
        
        var description1 = new SpecializationDescription("Description 1");
        var description2 = new SpecializationDescription("Description 2");

        
        var hash1 = description1.GetHashCode();
        var hash2 = description2.GetHashCode();

        
        Assert.NotEqual(hash1, hash2);
    }
}
