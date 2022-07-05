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
            //ע��appsetting���ʰ�����
            services.AddSingleton(new Appsetting(Configuration));

            //ע��Swagger����
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "SwiftCode.BBS.API",
                    Description = "���˵���ĵ�",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "SwiftCode",
                        Email = ""
                    }
                });

                //��dll�������ļ���������ʾ�ӿ�˵����
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "SwiftCode.BBS.xml");
                c.IncludeXmlComments(xmlPath, true);

                #region ����Swagger��Token��Ȩ����
                //����С��
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //��header�����token�����ݵ���̨
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT ��Ȩ�����ݽ�������ͷ�н��д��䣩ֱ�����¿������� Bearer {token} ��ע�⣬����֮����һ���ո�",
                    Name = "Authorization", //jwtĬ�ϵĲ�������
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header, //jwtĬ�ϴ��Authorization��Ϣ��λ�� - ����ͷ��
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion

            #region ���ڲ��Ե���Ȩ����
            services.AddAuthorization(option => {
                //������ɫ
                option.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                //��Ĺ�ϵ����������һ�ֽ�ɫ����
                option.AddPolicy("Admin", policy => policy.RequireRole("Admin", "System"));
                //�ҵĹ�ϵ��������ɫ���붼���㣬��������
                option.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));
            });
            #endregion

            #region ע����Ȩ����
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
                    //����ǻ������ʱ�䣬��ʹ�����˹���ʱ�䣬����Ҳ��Ҫ���ǽ�ȥ�ģ�����ʱ��+���壬Ĭ����7���ӣ�����ֱ������Ϊ0
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

            //�����м��������Swagger
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });
            #endregion

            app.UseRouting();
            //�Ƚ���v ��֤
            app.UseAuthentication();
            //Ȼ���ڽ�����Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
