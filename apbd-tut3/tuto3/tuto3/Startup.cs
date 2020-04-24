using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using tuto3.DAL;
using tuto3.Handlers;
using tuto3.Middlewares;

namespace tuto3
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
            services.AddScoped<IDbService, SqlServerDbService>();
            services.AddControllers().AddXmlSerializerFormatters();

            // Adding our auth handler to app's auth schemes as a basic scheme
            //services.AddAuthentication("AuthenticationBasic").
            //    AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);

            // Adding JSON Web Token Bearer auth scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "Gakko",
                        ValidAudience = "Students",
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                    };
                });

            // Swagger documentation service injection 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adding Swagger documentation service 
            // v1 used for version control purposes
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Studnet API v1");
            });


            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.UseMiddleware<LoggerMiddleware>();

            // Check if a request is sent by a Student, on condition that path contains word "secret"
            // (better create a custom middleware class for this)
            app.UseWhen(context => context.Request.Path.ToString().Contains("secret"), app =>
                app.Use(async (context, next) =>
                {
                    if (!context.Request.Headers.ContainsKey("Index"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Index number required");
                        return;
                    }

                    string index = context.Request.Headers["Index"].ToString();

                    var student = service.GetStudent(index);
                    if (student == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Incorrect Index number");
                        return;
                    }
                    await next();
                })
            );

            app.UseRouting();

            //Enabling auth check of requests
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
