using Swift.BBS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Swift.BBS.IRepositories
{
    public interface IArticleRepository
    {
        void Add(Article model);
        void Delete(Article model);
        void Update(Article model);
        List<Article> Query(Expression<Func<Article, bool>> whereExpression);
    }
}
