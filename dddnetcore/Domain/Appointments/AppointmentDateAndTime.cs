using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Appointments
{
    public class AppointmentDateAndTime : IValueObject
    {
        public DateTime DateAndTime{get; private set;}

        private AppointmentDateAndTime() { }

        public AppointmentDateAndTime(DateTime date){
            this.DateAndTime = date;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (AppointmentDateAndTime)obj;
            return DateAndTime == other.DateAndTime;
        }

        public override int GetHashCode(){
            return DateAndTime.GetHashCode();
        }
    }
}