using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProjektZaliczeniowy.Authorization;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Middleware;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Models.Validators;
using ProjektZaliczeniowy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy
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
            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentucation").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Berrer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };

            });
            services.AddScoped<IAuthorizationHandler, ArticleResourceOperationRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, CommentResourceOperationRequirementHandler>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<ArticleDbContext>();
            services.AddScoped<ArticleSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEnd", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithOrigins(Configuration["AllowedOrigins"])
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ArticleSeeder seeder)
        {
            app.UseCors("FrontEnd");
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","ProjektZaliczeniowy");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
