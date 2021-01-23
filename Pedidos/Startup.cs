using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pedidos.Data;
using Pedidos.Hangfire;
using Pedidos.Hubs;
using Pedidos.Models.Enums;

namespace Pedidos
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
            services.AddControllersWithViews()
                   .AddSessionStateTempDataProvider();
            services.AddRazorPages()
                    .AddSessionStateTempDataProvider();

            services.AddSignalR();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(o =>
                   {
                       o.LoginPath = "/Cuenta/Login";
                       o.Cookie.Name = "Pedidos";
                   });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(RolesSistema.Establecimiento.ToString(), pol => pol.RequireClaim(ClaimTypes.Role, RolesSistema.Establecimiento.ToString()));
            });

            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

            string connection = "";

#if DEBUG
            connection = Configuration.GetConnectionString("ConnectionStringDev");
#else
            connection = Configuration.GetConnectionString("ConnectionStringProduction");
#endif

            var local = Configuration.GetConnectionString("ConnectionStringLocal");

            services.AddHangfire(x => x.UseSqlServerStorage(local));
            services.AddHangfireServer();

            services.AddDbContext<AppDbContext>(optoins => optoins.UseSqlServer(local));
            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()); });

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberGroupSeparator = " ";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                // Authorization = new[] { new IDashboardAuthorizationFilter() }
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            RecurringJob.RemoveIfExists("RelatorioVendasSaidasAnual");

            RecurringJob.AddOrUpdate(
            recurringJobId: "RelatorioVendasSaidasAnual",
            methodCall: () => new Hangfire.Jobs().RelatorioVendasSaidasAnual(),
            cronExpression: Cron.Hourly(),
            timeZone: TimeZoneInfo.Local
         );

            //app.UseStatusCodePages(async context =>
            //{
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            //        response.StatusCode == (int)HttpStatusCode.InternalServerError)
            //        response.Redirect("/Login");

            //    //if (response.StatusCode == (int)HttpStatusCode.NotFound)
            //    //    response.Redirect("/Login/Salir");
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<CardapioHub>("/cardapiohub");
            });
        }

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Debug.Assert(typeToConvert == typeof(DateTime));
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
            }
        }

        public class CustomBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(ModelBinderProviderContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (context.Metadata.ModelType == typeof(decimal))
                {
                    return new DecimalModelBinder();
                }

                return null;
            }
        }

        public class DecimalModelBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult == null)
                {
                    return Task.CompletedTask;
                }

                var value = valueProviderResult.FirstValue;

                if (string.IsNullOrEmpty(value))
                {
                    return Task.CompletedTask;
                }

                // Remove unnecessary commas and spaces
                value = value.Replace(",", string.Empty).Trim();

                decimal myValue = 0;
                if (!decimal.TryParse(value, out myValue))
                {
                    // Error
                    bindingContext.ModelState.TryAddModelError(
                                            bindingContext.ModelName,
                                            "Could not parse MyValue.");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(myValue);
                return Task.CompletedTask;
            }
        }

    }
}
