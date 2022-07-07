using Swift.BBS.IRepositories.Base;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Services.BASE;

namespace Swift.BBS.Services
{
    public class ArticleServices : BaseServices<Article>, IArticleServices
    {
        public ArticleServices(IBaseRepository<Article> baseRepository) : base(baseRepository)
        {
        }

        //private IArticleRepository dal = new ArticleRepository();
        //public void Add(Article model)
        //{
        //    dal.Add(model);
        //}

        //public void Delete(Article model)
        //{
        //    dal.Delete(model);
        //}

        //public List<Article> Query(Expression<Func<Article, bool>> whereExpression)
        //{
        //    return dal.Query(whereExpression);
        //}

        //public void Update(Article model)
        //{
        //    dal.Update(model);
        //}
    }
}
