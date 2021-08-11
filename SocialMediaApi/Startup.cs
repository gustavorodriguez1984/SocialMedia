using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMediaCore.CustomEntities;
using SocialMediaCore.Interfaces;
using SocialMediaCore.Services;
using SocialMediaInfrastructure.Filters;
using SocialMediaInfrastructure.Options;
using SocialMediaInfrastructure.Repositories;
using SocialMediaInfrastructure.Services;
using SocialMediaInfrastructure.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SocialMediaApi
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers(options => {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
           {
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
           }).ConfigureApiBehaviorOptions(options => {
               //options.SuppressModelStateInvalidFilter = true;  //suprimir validacion ModelState aunq este el atributo ApiController.

           });

            services.AddOptions(Configuration);
            services.AddDbContexts(Configuration);
            services.AddServices();
            services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };

            });
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options => {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //Esto ya no va porque se ingreso en el metodo AddServices de la clase ServiceColectionExtension para una mayor abstraccion.
            /////////////////////////////////////////////////////////////
            //services.AddTransient<IPostService, PostService>();
            //services.AddTransient<ISecurityService, SecurityService>();
            ////services.AddTransient<IPostRepository, PostRepository>();
            ////services.AddTransient<IUserRepository, UserRepository>();
            //services.AddScoped(typeof(IRepository<>),typeof(BaseRepository<>));
            //services.AddTransient<IUnitOfWork,UnitOfWork>();
            //services.AddSingleton<IPasswordService, PasswordService>();
            //services.AddSingleton<IUriService>(provider =>
            //{
            //    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
            //    var request = accesor.HttpContext.Request;
            //    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            //    return new UriService(absoluteUri);
            //});
            //services.AddSwaggerGen(doc =>
            //{
            //    doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Api", Version = "v1" });
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    doc.IncludeXmlComments(xmlPath);
            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options=> {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API");//la ruta en donde generamos el json d enuestra documentacion, esta ruta solo funciona en IIS EXPRESS
               // options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API"); al agregar los dos puntos funciona con subsitios en en el IIS Server
                options.RoutePrefix = string.Empty;//esta linea cuando haga el deploy en un servidor IIS la tengo que comentar.
            });
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
