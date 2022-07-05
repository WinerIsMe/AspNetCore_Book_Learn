using Swift.BBS.IRepositories;
using Swift.BBS.Model.Models;
using Swift.BBS.Repositories.BASE;
using Swift.BBS.Repositories.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        //private SwiftBbsContext context;
        //public ArticleRepository()
        //{
        //    context = new SwiftBbsContext();
        //}
        //public void Add(Article model)
        //{
        //    context.Articles.Add(model);
        //    context.SaveChanges();
        //}

        //public void Delete(Article model)
        //{
        //    context.Articles.Remove(model);
        //    context.SaveChanges();
        //}

        //public List<Article> Query(Expression<Func<Article, bool>> whereExpression)
        //{
        //    return context.Articles.Where(whereExpression).ToList();
        //}

        //public void Update(Article model)
        //{
        //    context.Articles.Update(model);
        //    context.SaveChanges();
        //}
    }
}
