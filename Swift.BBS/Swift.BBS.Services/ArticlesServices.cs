using Swift.BBS.IRepositories;
using Swift.BBS.IRepositories.BASE;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Services.BASE;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Swift.BBS.Services
{
    public class ArticleServices : BaseServices<Article>, IArticleServices
    {
        private readonly IBaseRepository<Article> baseRepository;
        private readonly IArticleRepository articleRepository;

        public ArticleServices(IBaseRepository<Article> baseRepository, IArticleRepository articleRepository) : base(baseRepository)
        {
            this.baseRepository = baseRepository;
            this.articleRepository = articleRepository;
        }


        public Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return articleRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Article> GetArticleDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await articleRepository.GetByIdAsync(id, cancellationToken);
            entity.Traffic += 1;

            await articleRepository.UpdateAsync(entity, true, cancellationToken: cancellationToken);

            return entity;
        }

        public async Task AddArticleCollection(int id, int userId, CancellationToken cancellationToken = default)
        {
            var entity = await articleRepository.GetCollectionArticlesByIdAsync(id, cancellationToken);
            entity.CollectionArticles.Add(new UserCollectionArticle()
            {
                ArticleId = id,
                UserId = userId
            });
            await articleRepository.UpdateAsync(entity, true, cancellationToken);
        }

        public async Task AddArticleComments(int id, int userId, string content, CancellationToken cancellationToken = default)
        {
            var entity = await articleRepository.GetByIdAsync(id, cancellationToken);
            entity.ArticleComments.Add(new ArticleComment()
            {
                Content = content,
                CreateTime = DateTime.Now,
                CreateUserId = userId
            });
            await articleRepository.UpdateAsync(entity, true, cancellationToken);
        }

        public async Task AdditionalItemAsync(Article entity, bool v, int n = 0)
        {
            entity.CreateTime = DateTime.Now.AddDays(-n);
            await articleRepository.InsertAsync(entity, true);
        }
    }
}
