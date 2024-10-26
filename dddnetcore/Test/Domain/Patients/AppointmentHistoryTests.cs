using System;
using Xunit;
using DDDSample1.Domain.Patients;

namespace DDDSample1.Tests.Domain.Patients
{
    public class AppointmentHistoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeHistory()
        {
            // Arrange
            string initialHistory = "Initial appointment history.";

            // Act
            var appointmentHistory = new AppointmentHistory(initialHistory);

            // Assert
            Assert.Equal(initialHistory, appointmentHistory.History);
        }
    }
}
