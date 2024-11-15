using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Appointments;
using Xunit;

namespace DDDSample1.Tests.Domain.Appointments
{
    public class AppointmentIdTests
    {
        [Fact]
        public void ConstructorShouldCreateAppointmentIdWithValidGuid(){
            var validId = Guid.NewGuid();

            var appointmentId = new AppointmentId(validId);

            Assert.Equal(validId, appointmentId.AsGuid());
        }

        [Fact]
        public void ConstructorShouldCreateAppointmentIdWithValidString(){

            var validId = Guid.NewGuid().ToString();

            var appointmentId = new AppointmentId(validId);

            Assert.Equal(new Guid(validId), appointmentId.AsGuid());
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringRepresentation(){
            var appointmentId = new AppointmentId(Guid.NewGuid());
            
            var stringRepresentation = appointmentId.AsString();

            Assert.Equal(appointmentId.AsGuid().ToString(), stringRepresentation);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual(){
            var id = Guid.NewGuid();
            var id1 = new AppointmentId(id);
            var id2 = new AppointmentId(id);

            bool areEqual = id1.Equals(id2);

            Assert.True(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent(){
            var id1 = new AppointmentId(Guid.NewGuid());
            var id2 = new AppointmentId(Guid.NewGuid());

            bool areEqual = id1.Equals(id2);

            Assert.False(areEqual);
        }
    }
}