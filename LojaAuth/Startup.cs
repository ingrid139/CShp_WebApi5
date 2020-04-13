using AutoMapper;
using LojaAuth.Api.Models;
using LojaAuth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LojaAuth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public StartupIdentityServer IdentitServerStartup { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;

            //config ambiente se não for teste
            if (!environment.IsEnvironment("Testing"))
                IdentitServerStartup = new StartupIdentityServer(environment);
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add politica para user Ingrid
            services.AddMvcCore()
               .AddAuthorization(opt => {
                   opt.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Email, "ingrid@codenation.com"));
               })
               .AddJsonFormatters();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<LojaContexto>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<ICompraService, CompraService>();
            services.AddScoped<IPromocoesService, PromocoesService>();

            // config prop IdentitServerStartup
            if (IdentitServerStartup != null)
                IdentitServerStartup.ConfigureServices(services);

            // config autenticação para API - jwt bearer 
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "codenation";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (IdentitServerStartup != null)
                IdentitServerStartup.Configure(app, env);

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
