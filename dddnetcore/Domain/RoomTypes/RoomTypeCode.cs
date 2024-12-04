using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.RoomTypes
{
    public class RoomTypeCode : EntityId
    {
        private static readonly Regex CodeRegex = new Regex(@"^[a-zA-Z0-9-]{8}$", RegexOptions.Compiled);

        public string Code {get; private set;}

        [JsonConstructor]
        public RoomTypeCode(string value) : base(value)
        {
            this.Code = Validate(value);
        }

        protected override object createFromString(string text)
        {
            return Validate(text);
        }

        public override string AsString()
        {
            return Code;
        }

        private static string Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleValidationException("Code cannot be null or empty.");
            
            if (!CodeRegex.IsMatch(value))
                throw new BusinessRuleValidationException("Code must be exactly 8 characters long, containing only letters, numbers, and dashes.");
            
            return value;
        }
    }
}