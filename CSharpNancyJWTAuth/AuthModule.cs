using Microsoft.IdentityModel.Tokens;
using Nancy;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CSharpNancyJWTAuth
{
    public class AuthModule : NancyModule
    {
        public AuthModule() : base("/auth")
        {
            Post("/login", parameters =>
            {
                //TODO
                //IMPLEMENT YOUR AUTHENTICATION LOGIC HERE

                var token = GetJwtToken("test");
                return new
                {
                    Token = token,
                };
            });
        }

        private string GetJwtToken(string client_id)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, client_id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            //must the same as your setting in your boostrapper class  
            var symmetricKeyAsBase64 = "Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==";
            int expireInMinutes = 10;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                //issuer: "url",
                //audience: "name",
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expireInMinutes)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(expireInMinutes).TotalSeconds
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
