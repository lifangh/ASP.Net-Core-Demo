using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获取 jwt 令牌
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy ="Admin")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            TokenModelJwt tokenModelJwt = new TokenModelJwt { Uid = 1, Role = "Admin" };

            var jwtStr = JwtHelper.IssueJwt(tokenModelJwt);

            var suc = true;

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

       
    }
}
