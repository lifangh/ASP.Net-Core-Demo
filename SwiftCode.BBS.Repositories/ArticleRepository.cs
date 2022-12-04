using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.EntityFramework.EFContext;
using SwiftCode.BBS.IRepositories;
using SwiftCode.BBS.Models;
using SwiftCode.BBS.Repositories.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace SwiftCode.BBS.Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(SwiftCodeBbsContext context) : base(context)
        {
        }

        public Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return DbContext().Articles.Where(x => x.Id == id)
                 .Include(x => x.ArticleComments).ThenInclude(x => x.CreateUser).SingleOrDefaultAsync(cancellationToken);
        }

        public Task<Article> GetCollectionArticlesByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return DbContext().Articles.Where(x => x.Id == id)
                .Include(x => x.CollectionArticles).SingleOrDefaultAsync(cancellationToken);
        }


    }
}
