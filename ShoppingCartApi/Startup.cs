using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi
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
            // services.AddSingleton(typeof(IDbCollectionOperationsRepository), typeof(DbCollectionOperationsRepository));
            // services.AddSingleton(typeof(IDocumentDBRepository<T,string>), typeof(DocumentDBRepository));
            services.AddSingleton<IDocumentDBRepository<UserDetailsModel>>(new DocumentDBRepository<UserDetailsModel>());
            services.AddSingleton<IDocumentDBRepository<ItemModel>>(new DocumentDBRepository<ItemModel>());
            services.AddSingleton(typeof(IDbCollectionOperationsRepository<UserDetailsModel, string>), typeof(DbCollectionOperationsRepository));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            // Shows UseCors with named policy.
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
