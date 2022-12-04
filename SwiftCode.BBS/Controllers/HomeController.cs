﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftCode.BBS.API.Controllers
{
    /// <summary>
    /// 主页
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IBaseServices<UserInfo> _userInfoService;
        private readonly IBaseServices<Article> _articleService;
        private readonly IBaseServices<Question> _questionService;
        private readonly IBaseServices<Advertisement> _advertisementService;
        private readonly IMapper _mapper;

        public HomeController(IBaseServices<UserInfo> userInfoService,
           IBaseServices<Article> articleService,
           IBaseServices<Question> questionService,
           IBaseServices<Advertisement> advertisementService,
           IMapper mapper)
        {
            _userInfoService = userInfoService;
            _articleService = articleService;
            _questionService = questionService;
            _advertisementService = advertisementService;
            _mapper = mapper;
        }



        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<ArticleDto>>> GetArticle()
        {
            var entityList = await _articleService.GetPageListAsync(0, 10, nameof(Article.CreateTime));
            var articleUserIdList = entityList.Select(x => x.CreateUserId);
            var userList = await _userInfoService.GetListAsync(x => articleUserIdList.Contains(x.Id));
            var response = _mapper.Map<List<ArticleDto>>(entityList);
            foreach (var item in response)
            {
                var user = userList.FirstOrDefault(x => x.Id == item.CreateUserId);
                item.UserName = user.UserName;
                item.HeadPortrait = user.HeadPortrait;
            }
            return new MessageModel<List<ArticleDto>>()
            {
                success = true,
                message = "获取成功",
                response = response
            };
        }
        /// <summary>
        /// 获取问答列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<QuestionDto>>> GetQuestion()
        {
            var questionList = await _questionService.GetPageListAsync(0, 10, nameof(Question.CreateTime));

            return new MessageModel<List<QuestionDto>>()
            {
                success = true,
                message = "获取成功",
                response = _mapper.Map<List<QuestionDto>>(questionList)
            };
        }
        /// <summary>
        /// 获取作者列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<UserInfoDto>>> GetUserInfo()
        {
            var userInfoList = await _userInfoService.GetPageListAsync(0, 5, nameof(UserInfo.CreateTime));

            var response = _mapper.Map<List<UserInfoDto>>(userInfoList);

            // 此处会多次调用数据库操作，实际项目中我们会返回字典来处理
            foreach (var item in response)
            {
                item.QuestionsCount = await _questionService.GetCountAsync(x => x.CreateUserId == item.Id);
                item.ArticlesCount = await _articleService.GetCountAsync(x => x.CreateUserId == item.Id);
            }
            return new MessageModel<List<UserInfoDto>>()
            {
                success = true,
                message = "获取成功",
                response = response
            };
        }
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> GetAdvertisement()
        {
            var advertisementList = await _advertisementService.GetPageListAsync(0, 5, nameof(Advertisement.CreateTime));
            return new MessageModel<string>();
        }

    }
}
