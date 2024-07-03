using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TMS.Infrastructure.Configuration;

namespace TMS.APP.Factory
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtConfiguration _jwtConfiguration;
        public JwtFactory(IOptions<JwtConfiguration> jwtOptions)
        {
            _jwtConfiguration = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtConfiguration);
        }
        public string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtConfiguration.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64),
            }
            .Concat(additionalClaims);

            var jwt = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(_jwtConfiguration.ValidFor),
                signingCredentials: _jwtConfiguration.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtConfiguration options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.");
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentException(nameof(JwtConfiguration.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentException(nameof(JwtConfiguration.JtiGenerator));
            }
        }
    }
}
