using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PA.ScrumPoker.Data.Cache;
using PA.ScrumPoker.Data.Dtos;
using PA.ScrumPoker.Web.Extensions;
using PA.ScrumPoker.Web.Repo;
using Swashbuckle.AspNetCore.Swagger;

namespace PA.ScrumPoker.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //distributed cache settings
            services.AddDistributedMemoryCache().AddSingleton(typeof(ICache<>), typeof(Cache<>));
            services.Configure<CacheOptions>(options =>
            {
                Configuration.GetSection("Caching").Bind(options);
            });

            //CORS
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(Configuration.GetValue<string>("AllowedOrigins").Split(','))
                .AllowCredentials();
            }));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });
            //signalR
            services.SetupServicesForSignalR();

            //repos
            services.AddSingleton<IRepository<RoomDto>, RoomRepo>();
            services.AddSingleton<IRepository<UserDto>, UserRepo>();

         

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ScrumPoker API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.SetupAppForSignalR();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommonEntity API V1");
            });

            app.UseMvc();
        }
    }
}
