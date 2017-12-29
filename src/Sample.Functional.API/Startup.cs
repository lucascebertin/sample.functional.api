using Microsoft.EntityFrameworkCore;
using Sample.Functional.API.Infrastructure.Contextos;
using Sample.Functional.API.Services;

namespace Sample.Functional.API
{
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Swashbuckle.AspNetCore.Swagger;


    public class Startup
    {
        public IHostingEnvironment CurrentEnvironment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ContextoDeUsuários>(options => {
                if (CurrentEnvironment.IsEnvironment("Testing"))
                    options.UseInMemoryDatabase("TestingDB");
                else
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "My API",
                    Version = "v1"
                });
            });

            services.AddTransient<ServiçosDeUsuário>();

            services.AddTransient(
                provider => new Repositório<Telefone>(
                    provider.GetService<ContextoDeUsuários>()));

            services.AddTransient(
                provider => new Repositório<Usuário>(
                    provider.GetService<ContextoDeUsuários>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                })
                .UseMvc(context =>
                {
                    context.MapRoute(
                        "default",
                        "{controller}/{action}/{id?}",
                        new
                        {
                            controller = "Account",
                            action = "Get"
                        });
                });
        }
    }
}
