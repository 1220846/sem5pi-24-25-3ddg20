using System;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Specializations;
using DDDSample1.Domain.Staffs;
using Xunit;

namespace DDDSample1.Tests.Domain.AppointmentsStaffs
{
    public class AppointmentStaffIdTests
    {
        [Fact]
        public void ConstructorShouldCreateAppointmentStaffIdWithValidIds(){
            var appointmentId = new AppointmentId(Guid.NewGuid());
            var staffId = new StaffId("D202400001");

            var operationTypeStaffId = new AppointmentStaffId(appointmentId, staffId);

            Assert.Equal(appointmentId, operationTypeStaffId.AppointmentId);
            Assert.Equal(staffId, operationTypeStaffId.StaffId);
        }

        [Fact]
        public void AsStringShouldReturnCorrectStringRepresentation(){
            var appointmentId = new AppointmentId(Guid.NewGuid());
            var staffId = new StaffId("D202400001");
            var operationTypeStaffId = new AppointmentStaffId(appointmentId, staffId);

            var stringRepresentation = operationTypeStaffId.AsString();

            Assert.Equal($"{appointmentId.AsGuid()}-{staffId.Id}", stringRepresentation);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenIdsAreEqual(){
            var appointmentId = new AppointmentId(Guid.NewGuid());
            var staffId = new StaffId("D202400001");
            var id1 = new AppointmentStaffId(appointmentId, staffId);
            var id2 = new AppointmentStaffId(appointmentId, staffId);

            bool result = id1.Equals(id2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenIdsAreDifferent(){
            var appointmentId1 = new AppointmentId(Guid.NewGuid());
            var appointmentId2 = new AppointmentId(Guid.NewGuid());
            var staffId = new StaffId("D202400001");
            var id1 = new AppointmentStaffId(appointmentId1, staffId);
            var id2 = new AppointmentStaffId(appointmentId2, staffId);

            bool result = id1.Equals(id2);

            Assert.False(result);
        }
    }
}