using System.Globalization;
using HealthyTasty.Infrastructure.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;

namespace HealthyTasty.Infrastructure.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        private readonly JwtOptions _options;

        public JwtHelper(JwtOptions options)
        {
            _options = options;
        }

        public JsonWebToken GenerateAccessToken(long userId, Roles role)
        {
            var now = DateTimeOffset.Now;

            var expires = now.AddMinutes(_options.ExpiresIn).DateTime;

            var claims = new List<KeyValuePair<string, object>>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString()),
                new(ClaimTypes.Role, role.ToString())
            };

            return new JsonWebToken
            {
                AccessToken = BuildJwtToken(claims, expires, _options.AccessSecret),
                Expires = expires.ToString(CultureInfo.CurrentCulture)
            };
        }

        public string GenerateRefreshToken()
            => BuildJwtToken(new List<KeyValuePair<string, object>>(), 
                DateTimeOffset.Now.AddDays(7).DateTime, _options.RefreshSecret);

        public bool VerifyRefreshToken(string token)
        {
            try
            {
                new JwtBuilder()
                    .WithAlgorithm(new HMACSHA512Algorithm())
                    .WithSecret(Encoding.ASCII.GetBytes(_options.RefreshSecret))
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildJwtToken(IEnumerable<KeyValuePair<string, object>> claims, DateTime expires, string secret)
            => new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(Encoding.ASCII.GetBytes(secret))
                .AddClaims(claims)
                .Issuer(_options.Issuer)
                .Audience(_options.Audience)
                .ExpirationTime(expires)
                .Encode();
    }

    public interface IJwtHelper
    {
        JsonWebToken GenerateAccessToken(long userId, Roles role);
        string GenerateRefreshToken();
        bool VerifyRefreshToken(string token);
    }
}
