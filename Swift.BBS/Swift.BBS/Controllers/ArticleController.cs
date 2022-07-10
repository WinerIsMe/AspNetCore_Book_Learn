using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swift.BBS.Common.Helper;
using Swift.BBS.IServices;
using Swift.BBS.IServices.BASE;
using Swift.BBS.Model;
using Swift.BBS.Model.Models;
using Swift.BBS.Model.ViewModels.Article;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swift.BBS.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly IArticleServices articleServices;
        private readonly IBaseServices<UserInfo> userInfoService;
        private readonly IMapper mapper;

        public ArticleController(IArticleServices articlesServices,IBaseServices<UserInfo> userInfoService,IMapper mapper)
        {
            this.articleServices = articlesServices;
            this.userInfoService = userInfoService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 分页获取文章列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<ArticleDto>>> GetList(int page,int pageSize)
        {
            var entityList = await articleServices.GetPagedListAsync(page, pageSize, nameof(Article.CreateTime));
            var articleUserIdList = entityList.Select(x => x.CreateUserId);
            var userList = await userInfoService.GetListAsync(x => articleUserIdList.Contains(x.Id));
            var response = mapper.Map<List<ArticleDto>>(entityList);
            foreach(var item in response)
            {
                var user = userList.FirstOrDefault(x => x.Id == item.CreateUserId);
                item.UserName = user?.UserName;
                item.HeadPortrait = user?.HeadPortrait;
            }
            return new MessageModel<List<ArticleDto>>
            {
                success = true,
                msg = "获取成功",
                response = response
            };
        }
        /// <summary>
        /// 根据ID查询文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<ArticleDetailsDto>> Get(int id)
        {
            //通过自定义服务层处理内部业务
            var entity = await articleServices.GetArticleDetailsAsync(id);
            var result = mapper.Map<ArticleDetailsDto>(entity);
            return new MessageModel<ArticleDetailsDto>
            {
                success = true,
                response = result
            };
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> CreateAsync(CreateArticleInputDto inputDto)
        {
            var token = JwtHelper.ParsingJwtToken(HttpContext);

            var entity = mapper.Map<Article>(inputDto);
            entity.CreateTime = System.DateTime.Now;
            entity.CreateUserId = token.Uid;
            await articleServices.InsertAsync(entity);

            return new MessageModel<string>
            {
                success = true
            };
        }
        /// <summary>
        /// 修改文章
        /// </summary>
        [HttpPut]
        public async Task<MessageModel<string>> UpdateAsync(int id, UpdateArticleInputDto input)
        {
            var entity = await articleServices.GetAsync(d => d.Id == id);

            entity = mapper.Map(input, entity);

            await articleServices.UpdateAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                msg = "修改成功"
            };
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel<string>> DeleteAsync(int id)
        {
            var entity = await articleServices.GetAsync(d => d.Id == id);
            await articleServices.DeleteAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                msg = "删除成功"
            };
        }

        /// <summary>
        /// 收藏文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}", Name = "CreateCollection")]
        public async Task<MessageModel<string>> CreateCollectionAsync(int id)
        {
            var token = JwtHelper.ParsingJwtToken(HttpContext);
            await articleServices.AddArticleCollection(id, token.Uid);
            return new MessageModel<string>()
            {
                success = true,
                msg = "收藏成功"
            };
        }


        /// <summary>
        /// 添加文章评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateArticleComments")]
        public async Task<MessageModel<string>> CreateArticleCommentsAsync(int id, CreateArticleCommentsInputDto input)
        {
            var token = JwtHelper.ParsingJwtToken(HttpContext);
            await articleServices.AddArticleComments(id, token.Uid, input.Content);
            return new MessageModel<string>()
            {
                success = true,
                msg = "评论成功"
            };
        }


        /// <summary>
        /// 删除文章评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(Name = "DeleteArticleComments")]
        public async Task<MessageModel<string>> DeleteArticleCommentsAsync(int articleId, int id)
        {
            var entity = await articleServices.GetByIdAsync(articleId);
            entity.ArticleComments.Remove(entity.ArticleComments.FirstOrDefault(x => x.Id == id));
            await articleServices.UpdateAsync(entity, true);
            return new MessageModel<string>()
            {
                success = true,
                msg = "删除评论成功"
            };
        }


    }
}
