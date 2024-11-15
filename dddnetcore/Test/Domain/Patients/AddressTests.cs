using System;
using Xunit;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.Patients
{
    public class AddressTests
    {
        [Fact]
        public void Constructor_ShouldInitializeLocation()
        {
            // Arrange
            string initialLocation = "Initial location : 1389-990";

            // Act
            var address = new Address(initialLocation);

            // Assert
            Assert.Equal(initialLocation, address.Location);
        }
    }
}
