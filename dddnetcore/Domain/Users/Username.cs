using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Users
{
    public class Username : EntityId
    {
        public string Name { get; private set; }

        [JsonConstructor]
        public Username(string username) : base(username)
        {
            //this.Name = ValidateEmail(username);
            this.Name=username;
        }

        public Username(Role role, string identifier) : base($"{role}{identifier}")
        {
            this.Name = GenerateBackofficeUsername(identifier, role);
        }

        protected override object createFromString(string text)
        {
            return text; 
        }

        public override string AsString()
        {
            return Name;
        }

        private bool IsMechanographicNumber(string username)
        {
            // Validate if the username follows the pattern for mechanographic numbers (e.g., D240003)
            return Regex.IsMatch(username, @"^[A-Z]{1}\d{6}$");
        }

        public string GenerateBackofficeUsername(string mechanographicNumber, Role role)
        {
            string roleInitial = GetRoleInitial(role);
            return $"{roleInitial}{mechanographicNumber}@{GetBackofficeDomain()}";
        }

        private string GetRoleInitial(Role role)
        {
            return role switch
            {
                Role.DOCTOR => "D",
                Role.NURSE => "N",
                Role.ADMIN => "O",
                Role.TECHNICIAN => "O",
                _ => throw new BusinessRuleValidationException("Invalid role for username generation!")
            };
        }

        private string GetBackofficeDomain()
        {
            string domain = Environment.GetEnvironmentVariable("SARM_DOMAIN");
            return string.IsNullOrEmpty(domain) ? "sarm.com" : domain;
        }

        /*private string ValidateEmail(string email)
        {
            var regex= new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                throw new BusinessRuleValidationException("Invalid email format!");
            }
            return email;
        }*/


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Name.Equals(((Username)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
