using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using SoccerStatResourceServer.AuthenticationSettings;
using SoccerStatResourceServer.Repository;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using SoccerStatResourceServer.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace SoccerStatResourceServer
{
    public class Startup
    {
        private readonly string localhostConnection = "localhostConnection";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            string connectionString = Configuration.GetConnectionString("ResourceDatabase");
            services.AddDbContext<ResourceDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));

            JwtSettings jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddTransient<ResourceDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>) );
            services.AddTransient<IUploadImage, UploadImageService>();

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = false,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.RefreshTokenSecret))
            };

            services.AddCors(options =>
            {
                options.AddPolicy(name: localhostConnection, builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .WithMethods("PUT", "POST", "GET", "DELETE"));
            });

            //services.Configure<FormOptions>(o =>
            //{
            //    o.ValueLengthLimit = int.MaxValue;
            //    o.MultipartBodyLengthLimit = int.MaxValue;
            //    o.MemoryBufferThreshold = int.MaxValue;
            //});
            #region
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SoccerStatAuthenticationServer", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into fiels the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoccerStatResourceServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            string uploadsDir = Path.Combine(env.WebRootPath, "uploads");
            if(!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = "/images",
                FileProvider = new PhysicalFileProvider(uploadsDir)
            });
            app.UseAuthorization();
            app.UseAuthorization();
            app.UseCors(localhostConnection);
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(localhostConnection);
            });
        }
    }
}
