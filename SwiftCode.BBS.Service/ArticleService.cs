using Microsoft.EntityFrameworkCore;
using SwiftCode.BBS.EntityFramework;
using SwiftCode.BBS.IRepositories;
using SwiftCode.BBS.IService;
using SwiftCode.BBS.IService.BASE;
using SwiftCode.BBS.Models;
using SwiftCode.BBS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using SwiftCode.BBS.Service.BASE;

namespace SwiftCode.BBS.Service
{
    public class ArticleService : BaseServices<Article>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleService(IBaseRepository<Article> baseRepository, IArticleRepository articleRepository) : base(baseRepository)
        {
            _articleRepository = articleRepository;
        }


        public Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _articleRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Article> GetArticleDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _articleRepository.GetByIdAsync(id, cancellationToken);
            entity.Traffic += 1;

            await _articleRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);

            return entity;
        }

        public async Task AddArticleCollection(int id, int userId, CancellationToken cancellationToken = default)
        {
            var entity = await _articleRepository.GetCollectionArticlesByIdAsync(id, cancellationToken);
            entity.CollectionArticles.Add(new UserCollectionArticle()
            {
                ArticleId = id,
                UserId = userId
            });
            await _articleRepository.UpdateAsync(entity, true, cancellationToken);
        }

        public async Task AddArticleComments(int id, int userId, string content, CancellationToken cancellationToken = default)
        {
            var entity = await _articleRepository.GetByIdAsync(id, cancellationToken);
            entity.ArticleComments.Add(new ArticleComment()
            {
                Content = content,
                CreateTime = DateTime.Now,
                CreateUserId = userId
            });
            await _articleRepository.UpdateAsync(entity, true, cancellationToken);
        }

        public async Task AdditionalItemAsync(Article entity, bool v, int n = 0)
        {
            entity.CreateTime = DateTime.Now.AddDays(-n);
            await _articleRepository.InsertAsync(entity, true);
        }
    }
}
