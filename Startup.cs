using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace Shop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

  
        public void ConfigureServices(IServiceCollection services)
        {
            //Para não ter problema com os cors no localhost
            services.AddCors();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1" });
            });

            //----------------Configurando o JWT

            //Chave de incriptação do token
             var key = Encoding.ASCII.GetBytes(Settings.Secret);

            //Informando que nestá aplicação possui uma Autentificacao e que o esquema é em JWT
             services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(x => //Informamos o formato do token e como ele irá validar este token
             {
                 x.RequireHttpsMetadata = false;//Requerir HTTPS
                 x.SaveToken = true;//Se irá salvar o token
                 x.TokenValidationParameters = new TokenValidationParameters // Como ele irá validar o token
                 {
                     ValidateIssuerSigningKey = true, //Ele precisa validar a chave
                     IssuerSigningKey = new SymmetricSecurityKey(key),//Informamos a chave e o formato dela
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Configura Cors localhost
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            //Configura a autorização e autentificação
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
