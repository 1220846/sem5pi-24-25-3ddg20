using System;
using Xunit;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.OperationTypes;

namespace DDDSample1.Tests.Domain.OperationRequests
{
    public class OperationRequestTests
    {
        [Fact]
        public void OperationRequest_Creation_Success()
        {
            var patientId = new MedicalRecordNumber("MR12345");
            var staffId = new StaffId(Guid.NewGuid());
            var operationTypeId = new OperationTypeId(Guid.NewGuid());
            var deadline = new DeadlineDate(DateTime.UtcNow.AddDays(30));
            var priority = Priority.ELECTIVE;

            var request = new OperationRequest(patientId, staffId, operationTypeId, deadline, priority);

            Assert.NotNull(request.Id);
            Assert.Equal(patientId, request.MedicalRecordNumber);
            Assert.Equal(staffId, request.StaffId);
            Assert.Equal(operationTypeId, request.OperationTypeId);
            Assert.Equal(deadline, request.DeadlineDate);
            Assert.Equal(priority, request.Priority);
            Assert.Equal(OperationRequestStatus.WAITING, request.Status);
        }

        [Fact]
        public void ChangePriority_Updates_Priority()
        {
            var request = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId(Guid.NewGuid()),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.ELECTIVE
            );
            var newPriority = Priority.URGENT;

            request.ChangePriority(newPriority);

            Assert.Equal(newPriority, request.Priority);
        }

        [Fact]
        public void ChangeDeadline_Updates_Deadline()
        {
            var request = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId(Guid.NewGuid()),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.EMERGENCY
            );
            var newDeadline = new DeadlineDate(DateTime.UtcNow.AddDays(60));

            request.ChangeDeadline(newDeadline);

            Assert.Equal(newDeadline, request.DeadlineDate);
        }

        [Fact]
        public void ChangeDeadline_NullDeadline_ThrowsArgumentNullException()
        {
            var request = new OperationRequest(
                new MedicalRecordNumber("MR12345"),
                new StaffId(Guid.NewGuid()),
                new OperationTypeId(Guid.NewGuid()),
                new DeadlineDate(DateTime.UtcNow.AddDays(30)),
                Priority.ELECTIVE
            );

            Assert.Throws<ArgumentNullException>(() => request.ChangeDeadline(null));
        }
    }
}
