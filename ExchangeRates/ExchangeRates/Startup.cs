using ExchangeRates.Config;
using ExchangeRates.Repositories.Abstract;
using ExchangeRates.Repositories.Implementation;
using ExchangeRates.Services;
using ExchangeRates.Services.Abstract;
using ExchangeRates.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates
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
            services.AddControllers();
            services.AddHttpClient();
            services.Configure<MyConfig>(Configuration.GetSection("MyConfig"));
            services.AddSingleton(Configuration.GetSection("MyConfig").Get<MyConfig>());
            services.AddSingleton<IExchangeRatesService,ExchangeRatesService>();
            services.AddSingleton<IApiKeyGeneratorService, ApiKeyGeneratorService>();
            services.AddSingleton<IApiRequestsRepository, ApiRequestsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
