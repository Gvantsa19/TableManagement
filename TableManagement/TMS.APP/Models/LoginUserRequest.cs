using System.ComponentModel.DataAnnotations;

namespace TMS.APP.Models
{
    public class LoginUserRequest
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
