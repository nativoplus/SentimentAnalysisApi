using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentimentInfrastructure.Extensions;
using SentimentInterfaces.SentimentService;
using SentimentMediator;
using SentimentServices.Services;
using SentimentViewModels.SentimentService;
using System.IO;

namespace SentimentAnalysisApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add MediatR
            services.AddMediatR(typeof(SentimentRequestHandler).Assembly);

            services.AddSingleton<ISentimentService<SourceData, Prediction>>(ctx =>
            {
                string mlModelFullPath = Path.Combine(HostingEnvironment.ContentRootPath, Configuration["MlModel:Path"]);
                return new SentimentService<SourceData, Prediction>(mlModelFullPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
