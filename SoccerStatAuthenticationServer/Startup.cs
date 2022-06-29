using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SoccerStatAuthenticationServer.Repository;
using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.Repository.UserRepository;
using SoccerStatAuthenticationServer.Repository.TokenRepository;
using SoccerStatAuthenticationServer.AuthenticationSettings;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SoccerStatAuthenticationServer.Services.TokenGenerators;
using SoccerStatAuthenticationServer.Services.PasswordHasher;
using SoccerStatAuthenticationServer.Services.Authenticator;
using SoccerStatAuthenticationServer.Services.ValidationParameters;
using SoccerStatAuthenticationServer.Services.UserService;

namespace SoccerStatAuthenticationServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string localhostConnection = "localhostConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            string connectionString = Configuration.GetConnectionString("AuthenticationDatabase");

            JwtSettings jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddDbContext<AuthenticationServerDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();

            TokenValidationParameters accessTokenValidationParameters = new ValidationParametersFactory(jwtSettings).AccessTokenValidationParameters;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = accessTokenValidationParameters;
                });
            services.AddCors(options =>
            {
                options.AddPolicy(name: localhostConnection, builder => builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .WithMethods("PUT", "POST", "GET", "DELETE"));
            });

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoccerStatAuthenticationServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(localhostConnection);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(localhostConnection);
            });
        }
    }
}
