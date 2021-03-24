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
            // ��������
            services.AddCors(c =>
            {
                // ���ò���
                c.AddPolicy("LimitRequests", policy =>
                {
                    // ֧�ֶ�������˿ڣ�ע��˿ںź�Ҫ��/б�ˣ�����localhost:8000/���Ǵ��
                    // http://127.0.0.1:1818 �� http://localhost:1818 �ǲ�һ���ģ�����д����
                    policy
                    .SetIsOriginAllowed((x) => true)//������������                   
                    .AllowAnyHeader()//��������ͷ
                    .AllowAnyMethod()//�������ⷽ��
                    .AllowCredentials();//ָ������cookie
                });
            });
            services.AddDistributedMemoryCache();//����session֮ǰ����������ڴ�
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // ����session
            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromSeconds(2000);//����session�Ĺ���ʱ��
                //options.Cookie.HttpOnly = false;//���������������ͨ��js��ø�cookie��ֵ
            });
            // ע�����
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;  // ���ؽӿڲ������շ�������
            });

            //Mapperӳ��
            services.AddAutoMapperSetup();
            // Swgger
            services.AddSwaggerGen(options =>
            {
                //����ApiGroupNames����ö��ֵ���ɽӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //��ȡö��ֵ�ϵ�����
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerDoc(f.Name, new OpenApiInfo
                    {
                        Title = info?.Title,
                        Version = info?.Version,
                        Description = info?.Description
                    });
                });              
                //û�м����Եķֵ����NoGroup��
                options.SwaggerDoc("NoGroup", new OpenApiInfo
                {
                    Title = "Ĭ�Ϸ���"
                });
                //�жϽӿڹ����ĸ�����
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    if (docName == "NoGroup")
                    {
                        //������ΪNoGroupʱ��ֻҪû�����ԵĶ����������
                        return string.IsNullOrEmpty(apiDescription.GroupName);
                    }
                    else
                    {
                        return apiDescription.GroupName == docName;
                    }
                });

                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "Api.xml");//������Ǹո����õ�xml�ļ���
                options.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�

                #region Token�󶨵�ConfigureServices
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
                // ������Ȩ��
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //��header�����token,���ݵ���̨
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                #endregion

            });

            // 1.����Ȩ���ԡ����Բ�����controller�У�д��� roles 
            // controller д�� [Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                // �û�
                options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                // ����Ա
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin", "User"));
            });

            #region ���ڶ�����������֤����
            //��ȡ�����ļ�
            var keyByteArray = Encoding.ASCII.GetBytes(JwtHelper.Secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // ������֤����
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,  // ��Կ
                ValidateIssuer = true,
                ValidIssuer = JwtHelper.Issuer, //������
                ValidateAudience = true,
                ValidAudience = JwtHelper.Audience,//������
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30000),
                RequireExpirationTime = true,
            };

            //2.1����֤����core�Դ��ٷ�JWT��֤
            // ����Bearer��֤
            services.AddAuthentication("Bearer")
             // ���JwtBearer����
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // ������ڣ����<�Ƿ����>��ӵ�������ͷ��Ϣ��
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
            #endregion

            //services.AddControllers().AddControllersAsServices(); //����������ʵ������

            // �ٶȸ��ı���
            services.AddUEditorService();

            //ȫ���쳣����
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        /// <summary>
        /// Autofacע��
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
            // ����ģʽ
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // ���� Swagger �м�� 
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {               
                //����ApiGroupNames����ö��ֵ���ɽӿ��ĵ���Skip(1)����ΪEnum��һ��FieldInfo�����õ�һ��Intֵ
                typeof(ApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
                {
                    //��ȡö��ֵ�ϵ�����
                    var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                    options.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info != null ? info.Title : f.Name);
                });
                options.SwaggerEndpoint("/swagger/NoGroup/swagger.json", "Ĭ�Ϸ���");
                options.DocExpansion(DocExpansion.None); //->�ӿ��ĵ������ʱ�Զ��۵�
            });
            // session
            app.UseSession();
            // web������̬�ļ����м�� û������޷�����ͼƬ
            app.UseStaticFiles();
            // ·���м��
            app.UseRouting();
            //��� Cors �����м��
            app.UseCors("LimitRequests");
            // �ȿ�����֤
            app.UseAuthentication();
            // Ȼ������Ȩ�м��
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
