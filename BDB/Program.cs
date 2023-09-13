using BDB.Authorization;
using BDB.Helpers;
using DAL;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using DAL.Repositories.Services.Common;
using DAL.Repositories.Services.SignalIR;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Extensions;
using UnitechMedical.Helpers;
using BDB.Helpers.MessageService;
using DinkToPdf.Contracts;
using DinkToPdf;
using Serilog.Core;
using NLog.Web;
using NLog;
using BDB.Helpers.Common;

namespace BDB
{
    public class Program
    {
    
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            var policyName = "defaultCorsPolicy";
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                //LogManager.Configuration.Variables["mydir"] = builder.Configuration["LogPath"].ToString();
                AddServices(builder, policyName);// Add services to the container.

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

   

                var app = builder.Build();
                ConfigureRequestPipeline(app, policyName, builder); // Configure the HTTP request pipeline.

                SeedDatabase(app); //Seed initial database

                app.Run();
            }
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

       
        }


        private static void AddServices(WebApplicationBuilder builder, string policyName)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var authServerUrl = builder.Configuration["AuthServerUrl"].TrimEnd('/');

            string migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name; //BDB



            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));

            // add identity
            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Setting token provider time span

            builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromMinutes(15));

            // Configure Identity options and password complexity here
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            // Adds IdentityServer.
            builder.Services.AddIdentityServer(o =>
            {
                o.IssuerUri = authServerUrl;
            })
              .AddDeveloperSigningCredential()
              .AddInMemoryPersistedGrants()
              .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
              .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
              .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
              .AddInMemoryClients(IdentityServerConfig.GetClients())
              .AddAspNetIdentity<User>()
              .AddResourceOwnerValidator<Authorization.UserValidator>()
              .AddProfileService<ProfileService>();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = authServerUrl;
                   options.RequireHttpsMetadata = false; // Note: Set to true in production
                   options.ApiName = IdentityServerConfig.ApiName;
               });
            // Add cors
            //builder.Services.AddCors();

            //builder.Services.AddSignalR();
            // var policyName = "defaultCorsPolicy";

            var allowedUrl = builder.Configuration["allowedUrl"];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.SetIsOriginAllowed(origin => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddSignalR();

            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            builder.Services.AddControllersWithViews();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = IdentityServerConfig.ApiFriendlyName, Version = "v1" });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>()
                            {
                                { IdentityServerConfig.ApiName, IdentityServerConfig.ApiFriendlyName }
                            }
                        }
                    }
                });
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
            });

            builder.Services.AddAutoMapper(typeof(Program));

            // Configurations
            builder.Services.Configure<AppSettings>(builder.Configuration);

            // Business Services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISmsService, SmsService>();
            builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
            builder.Services.AddScoped<IPdfConverterService, PdfConverterService>();

            // Repositories
            builder.Services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddScoped<IReportService, ReportService>();

            // DB Creation and Seeding
            builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();

            //File Logger
            builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

            //Whatsapp

            WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
            whatsAppConfig.WhatsAppBusinessPhoneNumberId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessPhoneNumberId"];
            whatsAppConfig.WhatsAppBusinessAccountId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessAccountId"];
            whatsAppConfig.WhatsAppBusinessId = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["WhatsAppBusinessId"];
            whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
            builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);

        }


        private static void ConfigureRequestPipeline(WebApplication app, string policyName,WebApplicationBuilder webApplicationBuilder)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.ConfigureExceptionHandler(webApplicationBuilder);
            app.UseHttpsRedirection();

            //  app.UseCors();

            app.MapHub<BroadcastHub>("/notify");
            app.UseCors(policyName);

            //     app.UseCors(builder => builder
            //.AllowAnyOrigin()
            //.AllowAnyHeader()
            //.AllowAnyMethod());



            //app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger UI - BDB";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{IdentityServerConfig.ApiFriendlyName} V1");
                c.OAuthClientId(IdentityServerConfig.SwaggerClientID);
                c.OAuthClientSecret("no_password"); //Leaving it blank doesn't work
            });
            //app.MapHub<BroadcastHub>("/notify");
            app.MapControllers();
        }

        private static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                    databaseInitializer.SeedAsync().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);

                    throw new Exception(LoggingEvents.INIT_DATABASE.Name, ex);
                }
            }
        }
    }
}
