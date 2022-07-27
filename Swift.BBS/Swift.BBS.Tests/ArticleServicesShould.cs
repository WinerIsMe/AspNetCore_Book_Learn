using Microsoft.EntityFrameworkCore;
using Swift.BBS.EntityFramework.EfContext;
using Swift.BBS.Model.Models;
using Swift.BBS.Repositories;
using Swift.BBS.Repositories.BASE;
using Swift.BBS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Swift.BBS.Tests
{
    public class ArticleServicesShould
    {
        private readonly DbContextOptions<SwiftBbsContext> dbOptions;

        public ArticleServicesShould()
        {
            //1. 设置上下文
            dbOptions = new DbContextOptionsBuilder<SwiftBbsContext>()
                .UseInMemoryDatabase(databaseName: "in-memeory")
                .Options;
        }

        [Fact]
        public async Task AddNewItemAsIncompleteForAdditionalAsync()
        {
            using (var context = new SwiftBbsContext(dbOptions))
            {
                //Arrange
                var repository = new BaseRepository<Article>(context);
                var articleRepository = new ArticleRepository(context);
                ArticleServices service = new ArticleServices(repository, articleRepository);

                var fakeArticle = new Article
                {
                    Id = 1,
                    Tag = "test",
                    Title = "test",
                    Content = "test",
                    CreateUser = new UserInfo() { },
                    CreateUserId = 1
                };

                // Act动作
                // 补录三天前
                await service.AdditionalItemAsync(fakeArticle, true, 3);

                // 开始测试用例
                // Act动作
                var itemsInDatabase = await context.Articles.CountAsync();
                // Assert
                // 断言1：判断在当前作用域中，是否添加的条数是1
                Assert.Equal(1, itemsInDatabase);
                // 断言2：文章标题
                var item = await context.Articles.FirstAsync();
                Assert.Equal("test", item.Title);
                // 断言3：是否为补录三天前，当然误差值距离期望值小于一秒
                var difference = DateTime.Now.AddDays(-3) - item.CreateTime;
                Assert.True(difference < TimeSpan.FromSeconds(1));
            }
            
        }
    }
}
