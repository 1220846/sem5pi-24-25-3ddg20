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
            if(!ValidateUsername(username)){
                throw new BusinessRuleValidationException("Invalid Username");
            }
            this.Name=username;
        }

        public static Username Create(Role role, string value)
        {
            string formattedUsername = GenerateBackofficeUsername(value, role);
            if(!ValidateUsername(formattedUsername)){
                throw new BusinessRuleValidationException("Invalid Format Email!");
            }
            return new Username(formattedUsername);
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

        public static string GenerateBackofficeUsername(string mechanographicNumber, Role role)
        {
            string roleInitial = GetRoleInitial(role);
            return $"{roleInitial}{mechanographicNumber}@{GetBackofficeDomain()}";
        }

        private static string GetRoleInitial(Role role)
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

        private static string GetBackofficeDomain()
        {
            string domain = Environment.GetEnvironmentVariable("SARM_DOMAIN");
            return string.IsNullOrEmpty(domain) ? "sarm.com" : domain;
        }

        private static bool ValidateUsername(string email)
        {
            var regex= new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        }


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
