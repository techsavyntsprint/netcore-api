using APICore.API.Utils.JsonLocalization;
using APICore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace APICore.API
{
    public static class ServicesExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContextPool<CoreDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        // Replace with your connection string.
                        config.GetConnectionString("ApiConnection"),
                        // Replace with your server version and type. Use MariaDbServerVersion for MariaDB.
                        new MySqlServerVersion(new Version(8, 0, 21))
                     //Consider to use a specific charset according with your needs in your modelbuilder inside your context
                     )
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // define swagger docs and other options
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Core",
                    Version = "v1",
                    Description = "API Core"
                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer authorization token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lowercase!!!
                    BearerFormat = "Bearer {token}",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        // defines scope - without a protocol use an empty array for global scope
                        { securityScheme, Array.Empty<string>() }
                    }
                );
                var basePath = AppContext.BaseDirectory;
                var fileName = Path.Combine(basePath, "APICore.API.xml");
                var fileName2 = Path.Combine(basePath, "APICore.Common.xml");
                options.IncludeXmlComments(fileName);
                options.IncludeXmlComments(fileName2);
            });
        }

        public static void ConfigureTokenAuth(this IServiceCollection services, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("BearerTokens")["Key"]);
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
                    ValidAudience = config.GetSection("BearerTokens")["Audience"],
                    ValidateAudience = true,
                    ValidIssuer = config.GetSection("BearerTokens")["Issuer"],
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };
            });
        }

        public static void ConfigurePerformance(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
           {
               options.AddDefaultPolicy(builder =>
                   builder.SetIsOriginAllowed(_ => true)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
           });
        }

        public static void ConfigureI18N(this IServiceCollection services)
        {
            //services.AddControllers().AddDataAnnotationsLocalization().AddViewLocalization();
            //CultureInfo[] supportedCultures = new[]
            //    {
            //            new CultureInfo("en-US"),
            //            new CultureInfo("es-ES")
            //    };
            //services.AddJsonLocalization(options => options.ResourcesPath = "i18n");
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});

            //services.AddJsonLocalization(options =>
            //{
            //    options.CacheDuration = TimeSpan.FromMinutes(15);
            //    options.ResourcesPath = "i18n";
            //    options.SupportedCultureInfos = new HashSet<CultureInfo>()
            //        {
            //          new CultureInfo("en-US")
            //        };
            //});

            #region Localization

            //services.AddSingleton<IdentityLocalizationService>();
            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "i18n";
            });
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                opts => { opts.ResourcesPath = "i18n"; })
            .AddDataAnnotationsLocalization(options =>
            {
            });
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            #endregion Localization
        }

        public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            services.AddHealthChecks()
                   .AddMySql(config.GetConnectionString("ApiConnection"));
        }

        public static void ConfigureDetection(this IServiceCollection services)
        {
            services.AddDetection();
            // Needed by Wangkanai Detection
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public static void ConfigureHsts(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }
    }
}