namespace DDDSample1.Domain.Auth{
    public class AccessTokenRequestDto{

        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Audience { get; set; }
    }
}