using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftCode.BBS.EntityFramework
{
    /// <summary>
    /// 用户文章收藏表
    /// </summary>
    public class UserCollectionArticle : RootEntityTKey<int>
    {
        public int UserId { get; set; }

        public int ArticleId { get; set; }
    }
}
