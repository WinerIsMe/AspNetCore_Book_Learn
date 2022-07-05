using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swift.BBS.Common.Helper;
using System;
using System.IO;
using System.Text;

namespace Swift.BBS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //注入appsetting访问帮助类
            services.AddSingleton(new Appsetting(Configuration));

            //注册Swagger服务
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "SwiftCode.BBS.API",
                    Description = "框架说明文档",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "SwiftCode",
                        Email = ""
                    }
                });

                //绑定dll的描述文件，用于显示接口说明等
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SwiftCode.BBS.xml");
                c.IncludeXmlComments(xmlPath, true);

                #region 开启Swagger的Token授权功能
                //开启小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT 授权（数据将在请求头中进行传输）直接在下框中输入 Bearer {token} （注意，两者之间是一个空格）",
                    Name = "Authorization", //jwt默认的参数名称
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header, //jwt默认存放Authorization信息的位置 - 请求头中
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion

            #region 基于策略的授权机制
            services.AddAuthorization(option => {
                //单独角色
                option.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //或的关系，满足其中一种角色即可
                option.AddPolicy("Admin", policy => policy.RequireRole("Admin", "System"));
                //且的关系，两个角色必须都满足，才算满足
                option.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));
            });
            #endregion

            #region 注册授权服务
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(c => {
                var audienceConfig = Configuration["Audience:Audience"];
                var symmentricKeyAsBase64 = Configuration["Audience:Secret"];
                var iss = Configuration["Audience:Issuer"];
                var keyByteArray = Encoding.ASCII.GetBytes(symmentricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);
                c.TokenValidationParameters = new TokenValidationParameters { 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = iss,
                    ValidateAudience = true,
                    ValidAudience = audienceConfig,
                    ValidateLifetime = true,
                    //这个是缓冲过期时间，即使配置了过期时间，这里也是要考虑进去的，过期时间+缓冲，默认是7分钟，可以直接设置为0
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //配置中间件，启用Swagger
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });
            #endregion

            app.UseRouting();
            //先进行v 认证
            app.UseAuthentication();
            //然后在进行授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
