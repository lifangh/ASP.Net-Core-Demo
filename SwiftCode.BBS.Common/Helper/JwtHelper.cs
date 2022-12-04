using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SwiftCode.BBS.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace SwiftCode.BBS.Common.Helper
{
    /// <summary>
    /// jwt
    /// </summary>
    public class JwtHelper
    {

        /// <summary>
        /// 颁发 jwt 字符串
        /// </summary>
        /// <param name="toKenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt toKenModel)
        {
            string iss = Appsettings.app(new string[] { "Audience","Issuer"});
            string aud = Appsettings.app(new string[] { "Audience","Audience"});
            string secret = Appsettings.app(new string[] { "Audience","Secret"});


            var claims = new List<Claim>()
            {
                //这里将用户的部分信息，比如uid存到Claim 中如果想知道如何在其他地方将这个uid从token中取出来，请看下面的serializeJWT()方法，或者在整个解决方案中搜索
                new Claim(JwtRegisteredClaimNames.Jti, toKenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{ new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                //目前过期时间为1000 / S ,可自定义，要注意JWT的缓冲过期时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{ new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration,DateTime.Now.AddSeconds(1000).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud)
            };

            //可以将一个用户的多个角色赋值
            claims.AddRange(toKenModel.Role.Split(',').Select( x => new Claim(ClaimTypes.Role,x)));

            // 密钥（SymmetricSecuritKey）对安全性要求，密钥的长度太短会报异常
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken
                (
                    issuer: iss,
                    claims: claims,
                    signingCredentials: creds
                );

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;

        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            //校验token
            if (!string.IsNullOrEmpty(jwtStr) && jwtHandler.CanReadToken(jwtStr)) 
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
                object role;

                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);

                tokenModelJwt = new TokenModelJwt
                {
                    Uid = Convert.ToInt32(jwtToken.Id),
                    Role = role == null ? null : role.ToString()
                };

            }
            return tokenModelJwt;
        }
        /// <summary>
        /// 授权解析jwt
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static TokenModelJwt ParsingJwtToken(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                return null;
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            TokenModelJwt tm = SerializeJwt(tokenHeader);
            return tm;
        }
    }
}
