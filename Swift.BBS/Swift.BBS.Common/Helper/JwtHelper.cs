using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace Swift.BBS.Common.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT令牌
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            string iss = Appsetting.app(new string[] { "Audience", "Issuer" });
            string aud = Appsetting.app(new string[] { "Audience", "Audience" });
            string secret = Appsetting.app(new string[] { "Audience", "Secret" });

            var claims = new List<Claim>
            {
                /*
                 * 特别重要：
                 * 这里将用户的部分信息，比如uid存到了Claim中，如果想知道如何在其他地方将这个uid从Token中取出来，请看下边的SerializeJwt() 方法，或者在整个解决方案中搜索这个方法。
                 * 也可以研究下HttpContext.User.Claims, 具体可以看看Policy/PermissionHandler.cs 类中是如何使用的。                 
                 */
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                //这个就是过期时间，目前是1000秒，可自行设定，注意JWT有自己的缓冲过期时间
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration,DateTime.Now.AddSeconds(1000).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud)
            };

            //可以将一个用户下的多个角色全部赋予
            claims.AddRange(tokenModel.Role.Split(',').Select(c => new Claim(ClaimTypes.Role, c)));

            //密钥（SymmentricSecurityKey对安全性的要求，密钥的长度太短会抛出异常）
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(issuer: iss, claims: claims, signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();

            //token校验
            if (!string.IsNullOrEmpty(jwtStr) && jwtHandler.CanReadToken(jwtStr))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
                object role;
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
                tokenModelJwt = new TokenModelJwt
                {
                    Uid = Convert.ToInt32(jwtToken.Id),
                    Role = role == null ? "" : role.ToString()
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

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        public int Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 职能
        /// </summary>
        public string Work { get; set; }
    }
}
