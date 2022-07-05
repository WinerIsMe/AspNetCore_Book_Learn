using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swift.BBS.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swift.BBS.Controllers
{
    [ApiController]
    [Route("api/[controller][action]")]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获取Jwt令牌
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            //将用户id和角色作为单独的自定义变量，封装进 token 字符串中
            TokenModelJwt tokenModel = new TokenModelJwt { Uid = 1, Role = "Admin" };
            //登录，获取到一定规则的Token令牌
            var jwtStr = JwtHelper.IssueJwt(tokenModel);
            var suc = true;

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
    }
}
