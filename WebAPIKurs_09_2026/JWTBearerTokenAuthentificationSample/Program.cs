
using JWTBearerTokenAuthentificationSample.Configurations;
using JWTBearerTokenAuthentificationSample.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWTBearerTokenAuthentificationSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();


            //Wir verwenden die Identity-User Datenbank benutzerdefiniert auf einen SQL Server mit dem Datenbanknamen unserer Wahl. 
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyUserIdentityDB")));

            //IdentityUser und IdentityRole sind die Entitäten, die wir für Authentifizierung etc verwenden. Customize-Objekte mit Verbung kann man auch eingeben.
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>(); //Das ganze wird mit dem ApplicationDbContext verwendet. (STORE) 

            //Konfiguration-Section wird zugegriffen 
            IConfigurationSection jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");

            //Mappen die Konfiguration auf eine Klasse 
            builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);


            //Lesen die JewBearerTokenSettings aus
            JwtBearerTokenSettings? jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

            //lesen den SecretKey aus
            byte[]? key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);


            //Sagen ASP.NET Core, dass wir Authentifizierung verwenden
            builder.Services.AddAuthentication(options =>
            {
                //Wie Authentifiziert man sich (Methode) 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //JWT Bearer Token-Meachanismus (Service) wird hier initialisiert
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();



            app.UseAuthentication(); //MUSS unbedingt vor Authorization stehen 
            app.UseAuthorization();



            //Aufruf einer Web-Methode
            app.MapControllers();

            app.Run();
        }
    }
}
