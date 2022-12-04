using AutoMapper;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.Models;
using SwiftCode.BBS.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Extensions.AutoMapper
{
    /// <summary>
    ///  配置构造函数，用来创建关系映射
    /// </summary>
    public class AutoMapperProfile : Profile
    {
       
        /// <summary>
        ///
        /// </summary>
        public AutoMapperProfile()
        {
            //第一个参数是源对象，第二个参数是目的对象
            CreateMap<CreateUserInfoInputDto, UserInfo>();
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfo, UserInfoDetailsDto>();
            
            CreateMap<CreateQuestionInputDto, Question>();
            CreateMap<UpdateQuestionInputDto, Question>();
            CreateMap<Question, QuestionDto>()
                .ForMember(a => a.QuestionCommentCount, o => o.MapFrom(x => x.QuestionComments.Count));
            CreateMap<Question, QuestionDetailsDto>();

            CreateMap<QuestionComment, QuestionCommentDto>()
                .ForMember(a => a.UserName, o => o.MapFrom(x => x.CreateUser.UserName))
                .ForMember(a => a.HeadPortrait, o => o.MapFrom(x => x.CreateUser.HeadPortrait));
            CreateMap<CreateQuestionCommentsInputDto, QuestionComment>();

            CreateMap<CreateUserInfoInputDto, UserInfo>();
            CreateMap<UserInfo, UserInfoDto>();
            CreateMap<UserInfo, UserInfoDetailsDto>();
        }
    }
    /// <summary>
    /// 静态全局 AutoMapper 配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }
    }
}
