using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace dddnetcore.Domain.Specializations
{
    public class SpecializationCode : IValueObject
    {
        public String Code {get; private set;}

        public SpecializationCode(String code) {
            if (string.IsNullOrEmpty(code))
                throw new BusinessRuleValidationException("Specialization code cannot be null or empty!");
            if (!Regex.IsMatch(code, @"^\d{6,18}$"))
                throw new BusinessRuleValidationException("Specialization code must be an 6-18 digit sequence!");
            this.Code = code;
        }

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Code.Equals(((SpecializationCode)obj).Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}