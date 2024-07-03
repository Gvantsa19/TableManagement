using System.Security.Claims;

namespace TMS.Infrastructure.Configuration
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims);
    }
}
