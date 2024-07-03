namespace TMS.APP.Models
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public int StatusCode { get; set; }
    }
}
