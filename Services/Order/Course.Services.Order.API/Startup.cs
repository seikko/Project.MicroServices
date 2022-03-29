using Course.Services.Order.Application.Consumer;
using Course.Services.Order.Infrastructure;
using Course.Shared.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Order.API
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



            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateOrderMessageCommandConsumer>();
                x.AddConsumer<CourseNameChanceConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });


                    cfg.ReceiveEndpoint("order-service", e =>//hangi kuyrugu okicak ? 
                     {
                         //okuma iþlemini gerceklestir.
                         e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
                     });

                    cfg.ReceiveEndpoint("course-chance", e =>//hangi kuyrugu okicak ? 
                    {
                        //okuma iþlemini gerceklestir.
                        e.ConfigureConsumer<CourseNameChanceConsumer>(context);
                    });

                });
            });

            services.AddMassTransitHostedService();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//token daki sub key'ini mapleme sub olarak gelsýn userid
            var requiredAutorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//authorize olmus user beklyorum
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {

                options.Authority = Configuration["IdentityServerUrl"];//  token dagýtmaktan gorevli urli alcak.
                options.Audience = "resource_order";//IdentityServerdaki config dosyasýndan geliyor.
                options.RequireHttpsMetadata = false;


            });

            services.AddHttpContextAccessor(); 
            services.AddScoped<ISharedIdentityServices, SharedIdentityServices>();
            services.AddMediatR(typeof(Course.Services.Order.Application.Handlers.CreateOrderCommandHandler).Assembly);
            services.AddDbContext<OrderDbContext>(opt =>
            {

                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure => {

                    configure.MigrationsAssembly("Course.Services.Order.Infrastructure");
                 });

            });


            services.AddControllers(opt=> {
                opt.Filters.Add(new AuthorizeFilter(requiredAutorizePolicy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course.Services.Order.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course.Services.Order.API v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
