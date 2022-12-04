using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using System;
using System.Threading.Tasks;

namespace SwiftCode.BBS.API.Controllers
{
    /// <summary>
    /// 授权
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {

        private readonly IBaseServices<UserInfo> _userInfo;
        private readonly IMapper _mapper;

        public AuthController(IBaseServices<UserInfo> userInfo, IMapper mapper)
        {
            _userInfo = userInfo;
            _mapper = mapper;
        }
        private string GetUserJwt(UserInfo userInfo)
        {
            var tokenModel = new TokenModelJwt { Uid = userInfo.Id, Role = "User" };
            var jwtStr = JwtHelper.IssueJwt(tokenModel);
            return jwtStr;
        }

        /// <summary>
        ///  登录
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> Login(string Name, string Password)
        {
            var jwtStr = string.Empty;
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Password))
                return new MessageModel<string>() { success = false, message = "用户名或密码不能为空" };

            var pass = MD5Helper.MD5Encrypt32(Password);
            var userInfo = await _userInfo.GetAsync(x => x.LoginName == Name && x.LoginPassWord == pass);
            if (userInfo == null)
                return new MessageModel<string>() { success = false, message = "登录失败，密码不正确！" };

            jwtStr = GetUserJwt(userInfo);
            return new MessageModel<string>() { success = true, message = "成功！", response = jwtStr };
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Register(CreateUserInfoInputDto input)
        {
            var userInfo = await _userInfo.FindAsync(x => x.LoginName == input.LoginName);
            if (userInfo != null)
                return new MessageModel<string>()
                {
                    success = false,
                    message = "账号已存在",
                };

            userInfo = await _userInfo.FindAsync(x => x.Email == input.Email);
            if (userInfo != null)
                return new MessageModel<string>()
                {
                    success = false,
                    message = "邮箱已存在",
                };

            userInfo = await _userInfo.FindAsync(x => x.Phone == input.Phone);
            if (userInfo != null)
                return new MessageModel<string>()
                {
                    success = false,
                    message = "手机号已注册",
                };

            userInfo = await _userInfo.FindAsync(x => x.UserName == input.UserName);
            if (userInfo != null)
                return new MessageModel<string>()
                {
                    success = false,
                    message = "用户名已存在",
                };
            input.LoginPassWord = MD5Helper.MD5Encrypt32(input.LoginPassWord);

            var user = _mapper.Map<UserInfo>(input);
            user.CreateTime = DateTime.Now;
            await _userInfo.InsertAsync(user, true);
            var jwtStr = GetUserJwt(user);

            return new MessageModel<string>()
            {
                success = true,
                message = "注册成功",
                response = jwtStr
            };
        }
    }
}
