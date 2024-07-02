using MediacApi.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediacApi.Services
{
    public class JWTService
    {
        private readonly IConfiguration config;
        private SymmetricSecurityKey _jwtKey;

        public JWTService(IConfiguration config)
        {
            this.config = config;
            this._jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
        }

        public string CreateJWT(User user)
        {
            var userCLaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname , user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("My own claim name", "Seffy")
            };

            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(userCLaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(config["JWT:ExpireInDays"])),
                SigningCredentials = credentials,
                Issuer = config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwt);
        }
    }
}
