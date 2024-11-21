using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.OperationTypes;
using Newtonsoft.Json;

namespace dddnetcore.Domain.Hospital
{
    public class HospitalService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "./Domain/Hospital/Loquitas.json");
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IOperationTypeRepository _operationTypeRepository;

        public HospitalService(IAppointmentRepository appointmentRepository, IOperationTypeRepository operationType) {
            this._appointmentRepository = appointmentRepository;
            this._operationTypeRepository = operationType;
        }   


        public async Task<Hospital> GetHospital() {
            if (!System.IO.File.Exists(_filePath))
                return null;
            
            var json = System.IO.File.ReadAllText(_filePath);
            Hospital hospital = JsonConvert.DeserializeObject<Hospital>(json);
            hospital.busyRooms = [];

            var appointments = await _appointmentRepository.GetAllAsync();

            var jsonContent = System.IO.File.ReadAllText(_filePath);
            var hospitalMap = JsonConvert.DeserializeObject<Hospital>(jsonContent);

            DateTime currentTime = DateTime.Now;
            foreach (Appointment appointment in appointments) {
                int roomNumber = GetRoomNumberNumber(appointment.RoomNumber);

                if (roomNumber != 0 && appointment.Status != AppointmentStatus.CANCELED) {
                    OperationType operationType = await _operationTypeRepository.GetByIdAsync(appointment.OperationRequest.OperationTypeId);
                    DateTime endTime = appointment.DateAndTime.DateAndTime.AddMinutes(operationType.EstimatedDuration.Minutes);
                    if (currentTime >= appointment.DateAndTime.DateAndTime && currentTime <= endTime) {
                        hospital.busyRooms.Add(roomNumber);
                    }
                }
            
            }

            System.IO.File.WriteAllText(_filePath, JsonConvert.SerializeObject(hospital, Formatting.Indented));

            return hospital;
        }


        private int GetRoomNumberNumber(RoomNumber roomNumber)
        {
            if (roomNumber == null || string.IsNullOrEmpty(roomNumber.Id))
                return 0;

            var match = System.Text.RegularExpressions.Regex.Match(roomNumber.Id, @"\d+");

            if (match.Success)
                return int.Parse(match.Value); // Converte o número encontrado para inteiro

            return 0;
        }

    }   
}