﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftCode.BBS.Common.Helper;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IService;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using SwiftCode.BBS.ModelsDto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace SwiftCode.BBS.API.Controllers
{
    /// <summary>
    /// 问答
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IBaseServices<UserInfo> _userInfoService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 分页获取问答列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<QuestionDto>>> GetList(int page, int pageSize)
        {
            var entityList = await _questionService.GetPageListAsync(page, pageSize, nameof(Question.CreateTime));

            return new MessageModel<List<QuestionDto>>()
            {
                success = true,
                message = "获取成功",
                response = _mapper.Map<List<QuestionDto>>(entityList)
            };
        }

        /// <summary>
        /// 根据Id查询问答
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<QuestionDetailsDto>> Get(int id)
        {
            // 通过自定义服务层处理内部业务
            var entity = await _questionService.GetQuestionDetailsAsync(id);
            var result = _mapper.Map<QuestionDetailsDto>(entity);
            return new MessageModel<QuestionDetailsDto>()
            {
                success = true,
                message = "获取成功",
                response = result
            };
        }

        /// <summary>
        /// 创建问答
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> CreateAsync(CreateQuestionInputDto input)
        {
            var token = JwtHelper.ParsingJwtToken(HttpContext);

            var entity = _mapper.Map<Question>(input);
            entity.Traffic = 1;
            entity.CreateTime = DateTime.Now;
            entity.CreateUserId = token.Uid;
            await _questionService.InsertAsync(entity, true);

            return new MessageModel<string>()
            {
                success = true,
                message = "创建成功"
            };
        }

        /// <summary>
        /// 修改问答
        /// </summary>
        [HttpPut]
        public async Task<MessageModel<string>> UpdateAsync(int id, UpdateQuestionInputDto input)
        {
            var entity = await _questionService.GetAsync(d => d.Id == id);

            entity = _mapper.Map(input, entity);

            await _questionService.UpdateAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                message = "修改成功"
            };
        }

        /// <summary>
        /// 删除问答
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel<string>> DeleteAsync(int id)
        {
            var entity = await _questionService.GetAsync(d => d.Id == id);
            await _questionService.DeleteAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                message = "删除成功"
            };
        }


        /// <summary>
        /// 添加问答评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateQuestionComments")]
        public async Task<MessageModel<string>> CreateQuestionCommentsAsync(int id, CreateQuestionCommentsInputDto input)
        {
            var token = JwtHelper.ParsingJwtToken(HttpContext);
            await _questionService.AddQuestionComments(id, token.Uid, input.Content);
            return new MessageModel<string>()
            {
                success = true,
                message = "评论成功"
            };
        }

        /// <summary>
        /// 删除问答评论
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteQuestionComments")]
        public async Task<MessageModel<string>> DeleteQuestionCommentsAsync(int questionId, int id)
        {
            var entity = await _questionService.GetByIdAsync(questionId);
            entity.QuestionComments.Remove(entity.QuestionComments.FirstOrDefault(x => x.Id == id));
            await _questionService.UpdateAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                message = "删除评论成功"
            };
        }
    }
}
