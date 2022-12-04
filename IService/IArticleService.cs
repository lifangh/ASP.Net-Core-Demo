
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using SwiftCode.BBS.IService.BASE;

namespace SwiftCode.BBS.IService
{
    public interface IArticleService : IBaseServices<Article>
    {
        Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Article> GetArticleDetailsAsync(int id, CancellationToken cancellationToken = default);

        Task AddArticleCollection(int id, int userId, CancellationToken cancellationToken = default);

        Task AddArticleComments(int id, int userId, string content, CancellationToken cancellationToken = default);
        Task AdditionalItemAsync(Article entity, bool index, int Size = 0);
    }
}
