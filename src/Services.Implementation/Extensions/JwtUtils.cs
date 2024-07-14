using Entities.DAOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementation.Extensions
{
    public static class JwtUtils
    {
        private static readonly string _token = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0KTUlJQklqQU5CZ2txaGtpRzl3MEJBUUVGQUFPQ0FROEFNSUlCQ2dLQ0FRRUEzZm9HMUJiSmVlMzAyWlFidlBnVgoyVDdqSVRNc3JqZXFKNnRFTjBTMWtxOHhmU0RlQno3c3luWFF4UEE5a3YxNDNKaUxGWU4zVGxqWit6TkducG5sCkNLQlFxTTQreVJscHpuVUdUR2tmMGNMOHZlU2RCV01Gc3Q4YjJuRG9FTHB2TDZscUVLYXZDanUveU9XSkkydTMKN2dKejg0bG9GWlg5cndLeUZ3Mnd0dUtpYWUyc3FrSXNVS2UraElCdlB5UW14T2VPcEROV25sWVp5VDN4UHdFTAp3RFUzU2NkbThIU3owNERBSUxVKzhiZStGOHJJdXFFbkZyakczM1pXT3lYdENlcmhzenprWTZlZzUwM09yUU5NCjdrMUJjZERUZXFXUVN3UDlHOWNwYzd3aHQ2am55WFJicFNyOG54bDZtUGtVc0dhdzFCalV2WWxzeVpxNW1RcVcKSndJREFRQUIKLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0t";

        public static string GenerateToken(Teacher user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [new Claim("id", user.Id.ToString()),
                     new Claim("name", user.FirstName + " " + user.MiddleName + " " + user.LastName)
                    ]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static Guid ValidateToken(string token)
        {
            if (token == null)
                return Guid.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_token);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = new Guid(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return Guid.Empty;
            }
        }
    }
}
