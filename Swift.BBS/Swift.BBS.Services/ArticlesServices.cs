using Swift.BBS.IRepositories;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Services
{
    public class ArticlesServices : IArticlesServices
    {
        private IArticleRepository dal = new ArticleRepository();
        public void Add(Article model)
        {
            dal.Add(model);
        }

        public void Delete(Article model)
        {
            dal.Delete(model);
        }

        public List<Article> Query(Expression<Func<Article, bool>> whereExpression)
        {
            return dal.Query(whereExpression);
        }

        public void Update(Article model)
        {
            dal.Update(model);
        }
    }
}
