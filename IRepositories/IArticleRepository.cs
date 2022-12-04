using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace SwiftCode.BBS.IRepositories
{
    public interface IArticleRepository : IBaseRepository<Article>
    {

        Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Article> GetCollectionArticlesByIdAsync(int id, CancellationToken cancellationToken = default);
    }


}
