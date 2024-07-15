using WebAPIBasis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Net;

namespace TestECPlatformWebAPI
{
    /// <summary>
    /// APIのスタートアップ
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        public Startup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            // NLog
            _loggerFactory = loggerFactory;

            // コンフィグ値の設定
            Configuration = configuration;

            // S_JIS変換用
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// アクセサ
        /// </summary>
        public IConfiguration Configuration { get; set; }
        ILoggerFactory _loggerFactory;
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost",
                                                          "https://localhost") // 指定したオリジン以外のリクエストは、CORSポリシーによって拒否
                                             .AllowCredentials() // 許可：ocalhost、ローカルIP、パブリックIPアドレスに対するHTTPおよびHTTPSの接続を許可
                                             .AllowAnyHeader()// 許可：Cookie、Authorizationヘッダーなど
                                             .AllowAnyMethod(); // 許可：すべてのヘッダーとHTTPメソッド許可

                                  });
            });

            services.AddControllers();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // [Authorize]を有効化する
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestECPlatform.Server", Version = "v1" });

                // Swaggerを設定し、XMLコメントを使用してAPIドキュメントを自動生成する
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // 変更：APIに競合するアクションがあり500エラーのため追記
                c.ResolveConflictingActions(x => x.First());
            });

            // 分散キャッシュの指定(アプリのインスタンス内で有効)
            services.AddDistributedMemoryCache();
            // セッションサービスの追加
            services.AddSession(opt =>
            {
                // オプション指定
                opt.IdleTimeout = TimeSpan.FromMinutes(20);
                opt.Cookie.IsEssential = true;
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportSystem.Server v1");
                // リリース用
                if (!env.IsDevelopment())
                    c.RoutePrefix = string.Empty;
            }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            // CORS
            app.UseCors(MyAllowSpecificOrigins);

            // [追加] 認証を使用するようにします。
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
