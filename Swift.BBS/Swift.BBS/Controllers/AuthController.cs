using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swift.BBS.Common.Helper;
using Swift.BBS.IServices.BASE;
using Swift.BBS.Model;
using Swift.BBS.Model.Models;
using Swift.BBS.Model.ViewModels.UserInfo;
using System.Threading.Tasks;

namespace Swift.BBS.Controllers
{
    /// <summary>
    /// 授权
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IBaseServices<UserInfo> userInfoService;
        private readonly IMapper mapper;

        public AuthController(IBaseServices<UserInfo> userInfoService,IMapper mapper)
        {
            this.userInfoService = userInfoService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPassWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Login(string loginName,string loginPassWord)
        {
            var jwtStr = string.Empty;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPassWord))
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "用户名或密码不可为空"
                };
            }

            var pass = MD5Helper.MD5Encrypt32(loginPassWord);
            var userInfo = await userInfoService.FindAsync(x => x.LoginName == loginName && x.LoginPassWord == pass);
            if(userInfo == null)
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "认证失败"
                };
            }
            jwtStr = GetUserJwt(userInfo);

            return new MessageModel<string>()
            {
                success = true,
                msg = "获取成功",
                response = jwtStr
            };
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Register(CreateUserInfoInputDto inputDto)
        {
            var userInfo = await userInfoService.FindAsync(x => x.LoginName == inputDto.LoginName);
            if(userInfo != null)
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "账号已存在"
                };
            }
            userInfo = await userInfoService.FindAsync(x => x.Email == inputDto.Email);
            if (userInfo != null)
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "邮箱已存在"
                };
            }
            userInfo = await userInfoService.FindAsync(x => x.Phone == inputDto.Phone);
            if (userInfo != null)
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "手机号已存在"
                };
            }
            userInfo = await userInfoService.FindAsync(x => x.UserName == inputDto.UserName);
            if (userInfo != null)
            {
                return new MessageModel<string>()
                {
                    success = false,
                    msg = "用户名已存在"
                };
            }
            inputDto.LoginPassWord = MD5Helper.MD5Encrypt32(inputDto.LoginPassWord);

            var user = mapper.Map<UserInfo>(inputDto);
            user.CreateTime = System.DateTime.Now;
            await userInfoService.InsertAsync(user, true);
            var jwtStr = GetUserJwt(user);

            return new MessageModel<string>()
            {
                success = true,
                msg = "注册成功",
                response = jwtStr
            };
        }

        private string GetUserJwt(UserInfo userInfo)
        {
            var tokenModel = new TokenModelJwt { Uid = userInfo.Id, Role = "User" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);
            return jwtStr;
        }
    }
}
