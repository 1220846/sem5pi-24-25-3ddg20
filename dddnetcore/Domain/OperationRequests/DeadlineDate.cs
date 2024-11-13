using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.OperationRequests
{
    public class DeadlineDate : IValueObject
    {
        public DateTime Date { get; private set; }

        public DeadlineDate(DateTime date)
        {
            this.Date = date;
        }


        public static DeadlineDate FromString(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime parsedDate))
            {
                return new DeadlineDate(parsedDate);
            }
            else
            {
                throw new BusinessRuleValidationException($"Invalid date format: {dateString}");
            }
        }

        public bool HasExpired()
        {
            return Date <= DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Date.Equals(((DeadlineDate)obj).Date);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public override string ToString()
        {
            return Date.ToString("dd/MM/yyyy");
        }
    }
}
