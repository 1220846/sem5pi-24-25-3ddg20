using System;
using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;
using Newtonsoft.Json;

namespace DDDSample1.Domain.Staffs {

    public class StaffId : EntityId {

        public string Id {get; private set;}

        public StaffId(string id) : base(CreateValidGuidOrId(id)) // Ajuste aqui
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessRuleValidationException("Staff id cannot be null or empty!");

            if (!Regex.IsMatch(id, @"^[ODN]\d{9}$"))
                throw new BusinessRuleValidationException("Staff id is badly formatted!");

            Id = id; // Isso pode não ser necessário se for gerido pela classe base
        }

        private static string CreateValidGuidOrId(string id)
        {
            if (Guid.TryParse(id, out var _))
            {
                return id; // Se for um GUID válido, retorna
            }

            // Se não for um GUID, retorna um valor fixo para evitar a exceção
            return Guid.NewGuid().ToString();
        }

        override
        protected Object createFromString(String text){
            return new Guid(text);
        }

        override
        public String AsString() {
            Guid obj = (Guid) base.ObjValue;
            return obj.ToString();
        }

        public Guid AsGuid() {
            return (Guid) base.ObjValue;
        }
    }
}