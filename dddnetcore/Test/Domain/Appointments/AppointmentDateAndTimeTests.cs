using System;
using DDDSample1.Domain.Appointments;
using Xunit;

namespace DDDSample1.Tests.Domain.Appointments
{
    public class AppoitmentDateAndTimeTests
    {
        [Fact]
        public void AppoitmentDateAndTimeConstructorShouldInitializeCorrectly()
        {
            var expectedDate = new DateTime(2024, 11, 15, 10, 30, 0);

            var appoitment = new AppointmentDateAndTime(expectedDate);

            Assert.Equal(expectedDate, appoitment.DateAndTime);
        }

        [Fact]
        public void EqualsSameDateAndTimeShouldReturnTrue()
        {
            var date = new DateTime(2024, 11, 15, 10, 30, 0);
            var appoitment1 = new AppointmentDateAndTime(date);
            var appoitment2 = new AppointmentDateAndTime(date);

            var areEqual = appoitment1.Equals(appoitment2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsDifferentDateAndTimeShouldReturnFalse()
        {
            var appoitment1 = new AppointmentDateAndTime(new DateTime(2024, 11, 15, 10, 30, 0));
            var appoitment2 = new AppointmentDateAndTime(new DateTime(2024, 11, 15, 11, 30, 0));

            var areEqual = appoitment1.Equals(appoitment2);

            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCodeSameDateAndTimeShouldReturnSameHash()
        {
            var date = new DateTime(2024, 11, 15, 10, 30, 0);
            var appoitment1 = new AppointmentDateAndTime(date);
            var appoitment2 = new AppointmentDateAndTime(date);

            var hash1 = appoitment1.GetHashCode();
            var hash2 = appoitment2.GetHashCode();

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCodeDifferentDateAndTimeShouldReturnDifferentHash()
        {
            var appoitment1 = new AppointmentDateAndTime(new DateTime(2024, 11, 15, 10, 30, 0));
            var appoitment2 = new AppointmentDateAndTime(new DateTime(2024, 11, 15, 11, 30, 0));

            var hash1 = appoitment1.GetHashCode();
            var hash2 = appoitment2.GetHashCode();

            Assert.NotEqual(hash1, hash2);
        }
    }
}
