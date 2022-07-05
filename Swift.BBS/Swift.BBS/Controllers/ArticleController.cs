using Microsoft.AspNetCore.Mvc;
using Swift.BBS.IServices;
using Swift.BBS.Model.Models;
using Swift.BBS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Swift.BBS.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ArticleController : Controller
    {
        IArticlesServices articleServices;
        public ArticleController()
        {
            articleServices = new ArticlesServices();
        }
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Article>> GetAll()
        {
            return await articleServices.GetListAsync();
        }
        /// <summary>
        /// 根据ID查询文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Article>> Get(int id)
        {
            return await articleServices.GetListAsync(c => c.Id == id);
        }
    }
}
