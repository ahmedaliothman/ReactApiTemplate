using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Api.Models.BasicAuthentication;
using Api.Models.Config;
using Api.Models.DB;
using Api.Models.JWT;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;


  

namespace PaciApi {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {
            services.AddControllers ();
             // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            string connStr =
                Configuration.GetConnectionString ("DevelopConnection");
            services
                .AddDbContext<Api.Models.DB.ApiDBContext> (options =>
                    options.UseSqlServer (connStr));
            services
                .AddCors (action =>
                    action
                    .AddPolicy ("all",
                        builder =>
                        builder
                        .AllowAnyOrigin ()
                        .AllowAnyMethod ()
                        .AllowAnyHeader ()));

            #region  SwaggerConfigure JWT
            //Register the Swagger generator, defining 1 or more Swagger documents
            services
                .AddSwaggerGen (c => {
                 c.SwaggerDoc ("v1", new OpenApiInfo { Title = "TheCodeBuzz-Service", Version = "v1" });

                    c
                        .AddSecurityDefinition ("Bearer",
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                                Description =
                                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                    Name = "Authorization",
                                    In = Microsoft
                                    .OpenApi
                                    .Models
                                    .ParameterLocation
                                    .Header,
                                    Type =
                                    Microsoft
                                    .OpenApi
                                    .Models
                                    .SecuritySchemeType
                                    .ApiKey
                            });
                });

            #endregion

            #region  SwaggerConfigureBasciAuthntication
            // services.AddSwaggerGen (c => {
            //     c.SwaggerDoc ("v1", new OpenApiInfo { Title = "TheCodeBuzz-Service", Version = "v1" });

            //     c.AddSecurityDefinition ("basic", new OpenApiSecurityScheme {
            //         Name = "Authorization",
            //             Type = SecuritySchemeType.Http,
            //             Scheme = "basic",
            //             In = ParameterLocation.Header,
            //             Description = "Basic Authorization header using the Bearer scheme."
            //     });

            //     c.AddSecurityRequirement (new OpenApiSecurityRequirement {
            //         {
            //             new OpenApiSecurityScheme {
            //                 Reference = new OpenApiReference {
            //                     Type = ReferenceType.SecurityScheme,
            //                         Id = "basic"
            //                 }
            //             },Secret
            //             new string[] { }
            //         }
            //     });

            // });
            #endregion

            #region jwtConfigure
            //services.AddOptions();
            // configure strongly typed settings objects
             var appSettingsSection = Configuration.GetSection ("AppSettings");
             services.Configure<AppSettings> (appSettingsSection);

            // configure jwt authentication
             var appSettings = appSettingsSection.Get<AppSettings> ();
            var key = Encoding.ASCII.GetBytes (appSettings.Secret);
            services
                .AddAuthentication (x => {
                    x.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters =
                        new TokenValidationParameters {
                            ValidateIssuerSigningKey = false,
                            IssuerSigningKey = new SymmetricSecurityKey (key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                            ClockSkew = TimeSpan.Zero
                        };
                });
            services.AddScoped<IjwtServices, jwtServices> ();
            #endregion

            #region  BasicAuthintication

            // services.AddAuthentication ("BasicAuthentication").
            // AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler> ("BasicAuthentication", null);
            // // configure DI for application services
            // services.AddScoped<IUserService, UserService> ();
            #endregion 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {

            #region UseSwagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app
                .UseSwagger (c => {
                    c.SerializeAsV2 = true;
                });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger ();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            
            app.UseSwaggerUI(option => option.SwaggerEndpoint("v1/swagger.json", "API"));

            #endregion

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
           
            
            
            //enabling calling file directory containing static files
            app.UseHttpsRedirection ();
            app.UseAuthentication ();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting ();
             //to enable cors
            app.UseCors (x =>
                    x
                    .SetIsOriginAllowed (origin => true)
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ());
            app.UseAuthorization ();
            app.UseEndpoints (endpoints => {
                    endpoints.MapControllers ();
                });
            app.UseSpa(spa =>
               {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
                 });


            
           
        }
    }
}