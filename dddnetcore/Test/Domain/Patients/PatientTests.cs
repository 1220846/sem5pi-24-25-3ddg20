using System;
using Xunit;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Tests.Domain.Patients
{
    public class PatientTests
    {
        private readonly MedicalRecordNumber _testId = new MedicalRecordNumber("202410000004");
        private readonly Address _testAddress = new Address("Initial address : 1389-100");
        private readonly DateOfBirth _testDateOfBirth = new DateOfBirth(new DateTime(1990, 1, 1));
        private readonly EmergencyContact _testEmergencyContact = new EmergencyContact("912345678");
        private readonly Gender _testGender = Gender.MALE;
        private readonly MedicalConditions _testMedicalConditions = new MedicalConditions("None");
        private readonly PatientContactInformation _testContactInformation = new PatientContactInformation(
            new PatientEmail("test@example.com"), new PatientPhone("912345678"));
        private readonly PatientFirstName _testFirstName = new PatientFirstName("John");
        private readonly PatientLastName _testLastName = new PatientLastName("Doe");
        private readonly PatientFullName _testFullName = new PatientFullName("John Doe");
        private readonly User _testUser = null;

        [Fact]
        public void Constructor_ShouldInitializePatient()
        {
            // Act
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);

            // Assert
            Assert.Equal(_testId, patient.Id);
            Assert.Equal(_testAddress, patient.Address);
            Assert.Equal(_testDateOfBirth, patient.DateOfBirth);
            Assert.Equal(_testEmergencyContact, patient.EmergencyContact);
            Assert.Equal(_testGender, patient.Gender);
            Assert.Equal(_testMedicalConditions, patient.MedicalConditions);
            Assert.Equal(_testContactInformation, patient.ContactInformation);
            Assert.Equal(_testFirstName, patient.FirstName);
            Assert.Equal(_testLastName, patient.LastName);
            Assert.Equal(_testFullName, patient.FullName);
            Assert.Equal(_testUser, patient.User);
        }

        [Fact]
        public void ChangeFirstName_ShouldUpdateFirstName()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newFirstName = new PatientFirstName("Jane");

            // Act
            patient.ChangeFirstName(newFirstName);

            // Assert
            Assert.Equal(newFirstName, patient.FirstName);
        }

        [Fact]
        public void ChangeLastName_ShouldUpdateLastName()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newLastName = new PatientLastName("Smith");

            // Act
            patient.ChangeLastName(newLastName);

            // Assert
            Assert.Equal(newLastName, patient.LastName);
        }

        [Fact]
        public void ChangeFullName_ShouldUpdateFullName()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newFullName = new PatientFullName("Jane Smith");

            // Act
            patient.ChangeFullName(newFullName);

            // Assert
            Assert.Equal(newFullName, patient.FullName);
        }

        [Fact]
        public void ChangeEmail_ShouldUpdateEmail()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newEmail = new PatientEmail("newemail@example.com");

            // Act
            patient.ChangeEmail(newEmail);

            // Assert
            Assert.Equal(newEmail, patient.ContactInformation.Email);
        }

        [Fact]
        public void ChangePhoneNumber_ShouldUpdatePhoneNumber()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newPhoneNumber = new PatientPhone("911234567");

            // Act
            patient.ChangePhoneNumber(newPhoneNumber);

            // Assert
            Assert.Equal(newPhoneNumber, patient.ContactInformation.PhoneNumber);
        }

        [Fact]
        public void ChangeAddress_ShouldUpdateAddress()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newAppointmentHistory = new Address("Follow-up address : 3890-289");

            // Act
            patient.ChangeAddress(newAppointmentHistory);

            // Assert
            Assert.Equal(newAppointmentHistory, patient.Address);
        }

        [Fact]
        public void ChangeMedicalConditions_ShouldUpdateMedicalConditions()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var newMedicalConditions = new MedicalConditions("Allergy to peanuts");

            // Act
            patient.ChangeMedicalConditions(newMedicalConditions);

            // Assert
            Assert.Equal(newMedicalConditions, patient.MedicalConditions);
        }

        [Fact]
        public void UpdateUser_ShouldUpdateUser()
        {
            // Arrange
            var patient = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var email = new Email("user@example.com");
            var role = Role.NURSE;
            var username = Username.Create(role,"D202400003");

            var newUser = new User(username, email, role);

            // Act
            patient.UpdateUser(newUser);

            // Assert
            Assert.Equal(newUser, patient.User);
        }

        [Fact]
        public void Equals_ShouldReturnTrue_ForSameId()
        {
            // Arrange
            var patient1 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var patient2 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);

            // Act
            bool areEqual = patient1.Equals(patient2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_ShouldReturnFalse_ForDifferentId()
        {
            // Arrange
            var patient1 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var patient2 = new Patient(new MedicalRecordNumber("67890"), _testAddress, _testDateOfBirth, 
                                       _testEmergencyContact, _testGender, _testMedicalConditions, 
                                       _testContactInformation, _testFirstName, _testLastName, 
                                       _testFullName, _testUser);

            // Act
            bool areEqual = patient1.Equals(patient2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHash_ForSameId()
        {
            // Arrange
            var patient1 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var patient2 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);

            // Act
            int hash1 = patient1.GetHashCode();
            int hash2 = patient2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHash_ForDifferentId()
        {
            // Arrange
            var patient1 = new Patient(_testId, _testAddress, _testDateOfBirth, _testEmergencyContact,
                                       _testGender, _testMedicalConditions, _testContactInformation,
                                       _testFirstName, _testLastName, _testFullName, _testUser);
            var patient2 = new Patient(new MedicalRecordNumber("67890"), _testAddress, _testDateOfBirth,
                                       _testEmergencyContact, _testGender, _testMedicalConditions,
                                       _testContactInformation, _testFirstName, _testLastName,
                                       _testFullName, _testUser);

            // Act
            int hash1 = patient1.GetHashCode();
            int hash2 = patient2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }
    }
}
