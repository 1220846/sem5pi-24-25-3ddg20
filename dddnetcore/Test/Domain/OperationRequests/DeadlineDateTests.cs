using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using Xunit;

namespace DDDSample1.Tests.Domain.OperationRequests
{
    public class DeadlineDateTests
    {

        [Fact]
        public void ConstructorShouldCreateInstanceWhenDateIsInTheFuture()
        {
            var futureDate = DateTime.Now.AddDays(1);
            var deadlineDate = new DeadlineDate(futureDate);
            Assert.NotNull(deadlineDate);
            Assert.Equal(futureDate, deadlineDate.Date);
        }

        [Fact]
        public void FromStringShouldReturnDeadlineDateWhenStringIsValid()
        {
            var validDateString = "2040-10-14";
            var deadlineDate = DeadlineDate.FromString(validDateString);
            Assert.NotNull(deadlineDate);
            Assert.Equal(DateTime.Parse(validDateString), deadlineDate.Date);
        }

        [Fact]
        public void FromStringShouldThrowExceptionWhenStringIsInvalid()
        {
            var invalidDateString = "invalid-date";
            var exception = Assert.Throws<BusinessRuleValidationException>(() => DeadlineDate.FromString(invalidDateString));
            Assert.Equal("Invalid date format: invalid-date", exception.Message);
        }

        [Fact]
        public void HasExpiredShouldReturnFalseWhenDateIsInTheFuture()
        {
            var futureDate = DateTime.Now.AddDays(1);
            var deadlineDate = new DeadlineDate(futureDate);
            var hasExpired = deadlineDate.HasExpired();
            Assert.False(hasExpired);
        }

        [Fact]
        public void EqualsShouldReturnTrueWhenDatesAreEqual()
        {
            var date = DateTime.Now.AddDays(1);
            var deadline1 = new DeadlineDate(date);
            var deadline2 = new DeadlineDate(date);

            Assert.True(deadline1.Equals(deadline2));
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenDatesAreDifferent()
        {
            var date1 = new DeadlineDate(DateTime.Now.AddDays(1));
            var date2 = new DeadlineDate(DateTime.Now.AddDays(2));
            var areEqual = date1.Equals(date2);
            Assert.False(areEqual);
        }

        [Fact]
        public void ToStringShouldReturnDateStringInCorrectFormat()
        {
            var date = new DeadlineDate(DateTime.Parse("2040-10-14 15:30:00"));
            var dateString = date.ToString();
            Assert.Equal("14/10/2040", dateString);
        }
    }
}
