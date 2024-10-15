using System;
using Xunit;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Shared;

public class StaffContactInformationTests
{
    [Fact]
    public void CreateValidStaffContactInformationShouldSucceed()
    {
        
        var email = new StaffEmail("john.doe@example.com");
        var phone = new StaffPhone("912345678");

       
        var contactInfo = new StaffContactInformation(email, phone);

        
        Assert.NotNull(contactInfo);
        Assert.Equal(email, contactInfo.Email);
        Assert.Equal(phone, contactInfo.PhoneNumber);
    }

    [Fact]
    public void StaffContactInformationEqualsShouldReturnTrueForEqualObjects()
    {
        
        var email1 = new StaffEmail("john.doe@example.com");
        var phone1 = new StaffPhone("912345678");
        var contactInfo1 = new StaffContactInformation(email1, phone1);

        var email2 = new StaffEmail("john.doe@example.com");
        var phone2 = new StaffPhone("912345678");
        var contactInfo2 = new StaffContactInformation(email2, phone2);

        
        Assert.True(contactInfo1.Equals(contactInfo2));
    }

    [Fact]
    public void StaffContactInformationEqualsShouldReturnFalseForDifferentObjects()
    {
        
        var email1 = new StaffEmail("john.doe@example.com");
        var phone1 = new StaffPhone("912345678");
        var contactInfo1 = new StaffContactInformation(email1, phone1);

        var email2 = new StaffEmail("jane.doe@example.com");
        var phone2 = new StaffPhone("923456789");
        var contactInfo2 = new StaffContactInformation(email2, phone2);

        
        Assert.False(contactInfo1.Equals(contactInfo2));
    }

    [Fact]
    public void StaffContactInformationGetHashCodeShouldBeEqualForEqualObjects()
    {
        
        var email1 = new StaffEmail("john.doe@example.com");
        var phone1 = new StaffPhone("912345678");
        var contactInfo1 = new StaffContactInformation(email1, phone1);

        var email2 = new StaffEmail("john.doe@example.com");
        var phone2 = new StaffPhone("912345678");
        var contactInfo2 = new StaffContactInformation(email2, phone2);

        
        Assert.Equal(contactInfo1.GetHashCode(), contactInfo2.GetHashCode());
    }

    [Fact]
    public void StaffContactInformationGetHashCodeShouldBeDifferentForDifferentObjects()
    {
        
        var email1 = new StaffEmail("john.doe@example.com");
        var phone1 = new StaffPhone("912345678");
        var contactInfo1 = new StaffContactInformation(email1, phone1);

        var email2 = new StaffEmail("jane.doe@example.com");
        var phone2 = new StaffPhone("923456789");
        var contactInfo2 = new StaffContactInformation(email2, phone2);

        
        Assert.NotEqual(contactInfo1.GetHashCode(), contactInfo2.GetHashCode());
    }
}