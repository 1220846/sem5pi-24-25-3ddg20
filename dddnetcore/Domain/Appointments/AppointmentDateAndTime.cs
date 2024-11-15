using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Appointments
{
    public class AppoitmentDateAndTime : IValueObject
    {
        public DateTime DateAndTime{get; private set;}

        private AppoitmentDateAndTime() { }

        public AppoitmentDateAndTime(DateTime date){
            this.DateAndTime = date;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (AppoitmentDateAndTime)obj;
            return DateAndTime == other.DateAndTime;
        }

        public override int GetHashCode(){
            return DateAndTime.GetHashCode();
        }
    }
}