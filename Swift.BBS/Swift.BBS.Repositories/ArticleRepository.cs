using Microsoft.EntityFrameworkCore;
using Swift.BBS.EntityFramework.EfContext;
using Swift.BBS.IRepositories;
using Swift.BBS.Model.Models;
using Swift.BBS.Repositories.BASE;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Swift.BBS.Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(SwiftBbsContext context) : base(context)
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
