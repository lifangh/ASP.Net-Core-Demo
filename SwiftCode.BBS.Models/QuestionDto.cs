using SwiftCode.BBS.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.Models
{
    public class QuestionDto : EntityTKeyDto<int>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 问答数量
        /// </summary>
        public int QuestionCommentCount { get; set; }

    }
}
