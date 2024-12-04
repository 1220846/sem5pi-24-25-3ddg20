using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.RoomTypes
{
    public class RoomTypeDesignation
    {
        private static readonly Regex AlphanumericRegex = new Regex(@"^[a-zA-Z0-9\s]*$", RegexOptions.Compiled);

        public string Designation { get; private set; }

        public RoomTypeDesignation(string designation)
        {
            if (string.IsNullOrWhiteSpace(designation))
                throw new BusinessRuleValidationException("The designation of room type cannot be null or empty!");

            if (designation.Length > 100)
                throw new BusinessRuleValidationException("The designation of room type cannot exceed 100 characters!");

            if (!AlphanumericRegex.IsMatch(designation))
                throw new BusinessRuleValidationException("The designation of room type must be alphanumeric and may include spaces!");

            this.Designation = designation;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Designation.Equals(((RoomTypeDesignation)obj).Designation);
        }

        public override int GetHashCode()
        {
            return Designation.GetHashCode();
        }
    }
}
