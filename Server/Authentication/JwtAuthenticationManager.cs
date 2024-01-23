using ADIRA.Shared.BusinessDataObjects;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADIRA.Server.Authentication
{
    public class JwtAuthenticationManager
    {
        public const string Jwt_Security_Key = "my-long-and-random-jwt-key-for-development";
        private const int Jwt_Token_Validity_Mins = 20;
        private readonly UserAccountService _userAccountService;
        public JwtAuthenticationManager(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        public UserSession GenerateJwtToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            //validating the user credentials
            var useraccount = _userAccountService.GetUserAccountByUserName(username);
            if (useraccount == null || useraccount.Password != password)
            {
                return null;
            }
            //generating Jwt Token
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(Jwt_Token_Validity_Mins);
            var tokenKey = Encoding.ASCII.GetBytes(Jwt_Security_Key);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,useraccount.UserName),
                new Claim(ClaimTypes.Role,useraccount.Role)
            });
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            //returning the usersession object
            var userSession = new UserSession()
            {
                UserName = useraccount.UserName,
                Role = useraccount.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
            return userSession;

        }
    }
}
