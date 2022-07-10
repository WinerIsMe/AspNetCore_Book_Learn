using Swift.BBS.IRepositories.BASE;
using Swift.BBS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Swift.BBS.IRepositories
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        Task<Article> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Article> GetCollectionArticlesByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
