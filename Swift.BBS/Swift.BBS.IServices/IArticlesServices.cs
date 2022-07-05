using Swift.BBS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.IServices
{
    public interface IArticlesServices
    {
        void Add(Article model);
        void Delete(Article model);
        void Update(Article model);
        List<Article> Query(Expression<Func<Article, bool>> whereExpression);
    }
}
