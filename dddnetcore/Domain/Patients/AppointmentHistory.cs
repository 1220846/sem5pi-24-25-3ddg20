using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class AppointmentHistory : IValueObject{
        public String History{get; private set;}
        public AppointmentHistory(String history){
            this.History = history;
        }
        public void UpdateAppointmentHistory(String appointment){
            this.History = appointment + this.History;
        }
    }
}