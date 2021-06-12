using Microsoft.IdentityModel.Tokens;
using Nancy;
using Nancy.Authentication.JwtBearer;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Text;

namespace CSharpNancyJWTAuth.Core
{

    //https://github.com/catcherwong/Nancy.Authentication.JwtBearer
    public class JwtBearerAuthenticationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            var keyByteArray = Encoding.ASCII.GetBytes("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==");
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = false,
                //ValidIssuer = "SOME URL HERE IF ValidateIssuer=true",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = false,
                //ValidAudience = "SOME NAME HERE IF ValidateAudience =true",

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            var configuration = new JwtBearerAuthenticationConfiguration
            {
                TokenValidationParameters = tokenValidationParameters,
                Challenge = "Guest"//if not use this,default to Bearer
            };

            pipelines.EnableJwtBearerAuthentication(configuration);

        }
    }

}
