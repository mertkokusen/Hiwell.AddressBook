using FluentValidation.AspNetCore;
using Hiwell.AddressBook.API.Filters;
using Hiwell.AddressBook.Core;
using Hiwell.AddressBook.Core.Extensions;
using Hiwell.AddressBook.Core.Interfaces;
using Hiwell.AddressBook.Core.UseCases;
using Hiwell.AddressBook.EF.PostGreSQL;
using Hiwell.AddressBook.EF.Sqlite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;

namespace Hiwell.AddressBook.API
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
            services.AddControllers(options =>
            {
                options.Filters.Add<ModelStateValidationFilter>();
            }).AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<RequestValidatorsSourceAssembly>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hiwell.AddressBook.API", Version = "v1" });
            });


#if RELEASE
            services.AddPostGreSql();
#else
            services.AddSqlite();
#endif

            services.ConfigureCoreDependecies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hiwell.AddressBook.API v1");
                c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

#if RELEASE
            app.EnsurePostGreDbCreated();
#else
            app.EnsureSqliteDbCreated(deleteExistingDatabase: true);
#endif
        }
    }
}
