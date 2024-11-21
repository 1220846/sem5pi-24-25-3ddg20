using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Appointments;
using Newtonsoft.Json;

namespace dddnetcore.Domain.Hospital
{
    public class HospitalService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../frontend/pblic/mazes/Loquitas.json");
        private readonly IAppointmentRepository _appointmentRepository;

        public HospitalService(IAppointmentRepository appointmentRepository) {
            this._appointmentRepository = appointmentRepository;
        }   

        public async Task<Hospital> GetHospital() {
            if (!System.IO.File.Exists(_filePath))
                return null;
            
            var appointments = await _appointmentRepository.GetAllAsync();

            var jsonContent = System.IO.File.ReadAllText(_filePath);
            var hospitalMap = JsonConvert.DeserializeObject<Hospital>(jsonContent);

            DateTime currentTime = DateTime.Now;

            foreach (var appointment in appointments) {

            }

            // TODO acabar
            return null;
        }


    }   
}