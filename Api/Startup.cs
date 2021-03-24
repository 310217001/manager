using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pi.PiManager.Api.ApiGroup;
using Pi.PiManager.Api.Extensions;
using Pi.PiManager.Api.Filter;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UEditorNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Pi.PiManager.Api
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
            // 跨域配置
            services.AddCors(c =>
            {
                // 配置策略
                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    // http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
                    policy
                    .SetIsOriginAllowed((x) => true)//允许任意域名                   
                    .AllowAnyHeader()//允许任意头
                    .AllowAnyMethod()//允许任意方法
                    .AllowCredentials();//指定处理cookie
                });
            });
            services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 启用session
            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromSeconds(2000);//设置session的过期时间
                //options.Cookie.HttpOnly = false;//设置在浏览器不能通过js获得该cookie的值
            });
            // 注册服务
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;  // 返回接口不采用驼峰命名法
            });

            //Mapper映射
            services.AddAutoMapperSetup();
            // Swgger
            services.AddSwaggerGen(options =>
            {
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });              
                //没有加特性的分到这个NoGroup上
                options.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "默认分组"
                });
                //判断接口归于哪个分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //当分组为NoGroup时，只要没加特性的都属于这个组
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Api.xml");//这个就是刚刚配置的xml文件名
                options.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                #region Token绑定到ConfigureServices
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                // 开启加权锁
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //在header中添加token,传递到后台
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                #endregion

            });

            // 1.【授权策略】可以不用在controller中，写多个 roles 
            // controller 写法 [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                // 用户
                options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                // 管理员
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin", "User"));
            });

            #region 【第二步：配置认证服务】
            //读取配置文件
            var keyByteArray = Encoding.ASCII.GetBytes(JwtHelper.Secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,  // 密钥
                ValidateIssuer = true,
                ValidIssuer = JwtHelper.Issuer, //发行人
                ValidateAudience = true,
                ValidAudience = JwtHelper.Audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30000),
                RequireExpirationTime = true,
            };

            //2.1【认证】、core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication("Bearer")
             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
            #endregion

            //services.AddControllers().AddControllersAsServices(); //控制器当做实例创建

            // 百度富文本框
            services.AddUEditorService();

            //全局异常捕获
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        /// <summary>
        /// Autofac注入
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<Filter.CorsMiddleware>();
            // 开发模式
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 启动 Swagger 中间件 
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {               
                //遍历ApiGroupNames所有枚举值生成接口文档，Skip(1)是因为Enum第一个FieldInfo是内置的一个Int值
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //获取枚举值上的特性
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                options.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "默认分组");
                options.DocExpansion(DocExpansion.None); //->接口文档界面打开时自动折叠
            });
            // session
            app.UseSession();
            // web基本静态文件的中间件 没有这个无法解析图片
            app.UseStaticFiles();
            // 路由中间件
            app.UseRouting();
            //添加 Cors 跨域中间件
            app.UseCors("LimitRequests");
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
