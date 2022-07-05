using Microsoft.AspNetCore.Mvc;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Services;
using System.Collections.Generic;

namespace Swift.BBS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        /// <summary>
        /// 根据ID查询文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Article> Get(int id)
        {
            IArticlesServices articleServices = new ArticlesServices();
            return articleServices.Query(c => c.Id == id);
        }
    }
}
