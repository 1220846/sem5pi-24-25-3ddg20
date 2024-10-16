using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Patients{
    public class DateOfBirth: IValueObject{
        public DateTime Date{get; private set;}
        public DateOfBirth(DateTime date){
            if (date > DateTime.Now)
                throw new BusinessRuleValidationException("The date of birth can't be in the future!");
            this.Date = date;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (DateOfBirth)obj;
            return Date == other.Date;
        }

        public override int GetHashCode(){
            return Date.GetHashCode();
        }
    }
}