using System;
using Xunit;
using dddnetcore.Domain.Specializations;
using DDDSample1.Domain.Shared;

public class SpecializationCodeTests
{
    [Fact]
    public void CreateValidSpecializationCodeShouldSucceed()
    {
        
        var validCode = "123456";

        
        var specializationCode = new SpecializationCode(validCode);

        
        Assert.NotNull(specializationCode);
        Assert.Equal(validCode, specializationCode.Code);
    }

    [Fact]
    public void CreateNullSpecializationCodeShouldThrowException()
    {
        
        string invalidCode = null;

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code cannot be null or empty!", exception.Message);
    }

    [Fact]
    public void CreateEmptySpecializationCodeShouldThrowException()
    {
        
        string invalidCode = "";

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code cannot be null or empty!", exception.Message);
    }

    [Fact]
    public void CreateShortSpecializationCodeShouldThrowException()
    {
        
        var invalidCode = "12345"; // Too short

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code must be an 6-18 digit sequence!", exception.Message);
    }

    [Fact]
    public void CreateLongSpecializationCodeShouldThrowException()
    {
        
        var invalidCode = "1234567890123456789"; // Too long

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code must be an 6-18 digit sequence!", exception.Message);
    }

    [Fact]
    public void CreateAlphabeticSpecializationCodeShouldThrowException()
    {
        
        var invalidCode = "ABC123"; // Contains letters

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code must be an 6-18 digit sequence!", exception.Message);
    }

    [Fact]
    public void CreateSpecialCharactersInCodeShouldThrowException()
    {
        
        var invalidCode = "1234.56"; // Contains invalid characters

        
        var exception = Assert.Throws<BusinessRuleValidationException>(() => new SpecializationCode(invalidCode));
        Assert.Equal("Specialization code must be an 6-18 digit sequence!", exception.Message);
    }

    [Fact]
    public void EqualsSameCodeShouldReturnTrue()
    {
        
        var code1 = new SpecializationCode("123456");
        var code2 = new SpecializationCode("123456");

        
        var areEqual = code1.Equals(code2);

        
        Assert.True(areEqual);
    }

    [Fact]
    public void EqualsDifferentCodeShouldReturnFalse()
    {
        
        var code1 = new SpecializationCode("123456");
        var code2 = new SpecializationCode("654321");

        
        var areEqual = code1.Equals(code2);

        
        Assert.False(areEqual);
    }

    [Fact]
    public void GetHashCodeSameCodeShouldReturnSameHash()
    {
        
        var code1 = new SpecializationCode("123456");
        var code2 = new SpecializationCode("123456");

        
        var hash1 = code1.GetHashCode();
        var hash2 = code2.GetHashCode();

        
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCodeDifferentCodeShouldReturnDifferentHash()
    {
        
        var code1 = new SpecializationCode("123456");
        var code2 = new SpecializationCode("654321");

        
        var hash1 = code1.GetHashCode();
        var hash2 = code2.GetHashCode();

        
        Assert.NotEqual(hash1, hash2);
    }
}
