using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Dto;
using DAL.Repository;
using DAL.Schema;
using Dapper.FluentMap;
using FamousQuoteQuiz.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace FamousQuoteQuizesUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            FluentMapper.Initialize(cfg => cfg.AddMap(new UserSchema()));
            FluentMapper.Initialize(cfg => cfg.AddMap(new QuoteSchema()));
            FluentMapper.Initialize(cfg => cfg.AddMap(new AuthorSchema()));

            services.AddSingleton<LogedUserProvider>();
            services.AddSingleton(typeof(ILogFacility<>), typeof(SeriLogFacility<>));
            services.AddSingleton(Log.Logger);
            services.AddTransient<IDbConnection>(sp =>
            {
                var connectionString = new ConnectionStringProvider().ConnectionString;
                var result = new SqlConnection(connectionString);

                result.Open();

                return result;
            });
            services.AddTransient(typeof(IRepository<UserDto>), typeof(UserRepository));
            services.AddTransient(typeof(IRepository<QuoteDto>), typeof(QuoteRepository));
            services.AddTransient(typeof(IRepository<AuthorDto>), typeof(AuthorRepository));
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
