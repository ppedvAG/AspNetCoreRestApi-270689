
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using WebAPIKurs.Configurations;
using WebAPIKurs.Formatters;
using WebAPIKurs.Services;
using WebAPIKurs.Services.Extentions;

namespace WebAPIKurs
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //Ab hier, ist die appsettings.json eingelesen + Environment Variablen + Secrets + Parameter etc.  (Alle Konfigurationsquellen sind geladen)
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext") ?? throw new InvalidOperationException("Connection string 'MovieDbContext' not found.");

            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));


            #region IOC Container Initialsiierung
            // Add services to the container.

            //builder.Services ->IServiceCollection (IOC-Container und hilft uns verschiede Dienste in die Controller-Klassen oder Methoden verfügbar zu machen)


            //Mit AddController sagen wir, dass wir eine WebAPI verwenden
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //In Relation kann es zu Loops kommen. 
                    options.JsonSerializerOptions.ReferenceHandler =
                    ReferenceHandler.IgnoreCycles;
                })
                .AddXmlSerializerFormatters()
                .AddMvcOptions(options =>
                {
                    options.InputFormatters.Add(new CsvInputFormatter());
                    options.OutputFormatters.Add(new CsvOutputFormatter());
                });
                

            //builder.Services.AddControllersWithViews(); //MVC  (UI-Technologie)
            //builder.Services.AddRazorPages(); //RazorPage (UI-Technologie)
            //builder.Services.AddMvc(); //MVC + Razor (beides kombiniert und funtkioniert perfekt zusammen) 


            builder.Services.AddSingleton<IDateTimeService, DateTimeService>();

            //Bis .NET 7 (Best Practise, wenn wir ein Interface mit mehreren Implementierungen verwenden wollen)

            builder.Services.AddSingleton<ISingletonGuidService, GuidService>();
            //builder.Services.AddScoped<IScopedGuidService, GuidService>();
            builder.Services.AddGuidServce(); //In dieser Methode wird der Service im IOC registriert. :-) 
            builder.Services.AddTransient<ITransientGuidService, GuidService>();


            //Ab .NET 8:
            builder.Services.AddKeyedSingleton<IGuidService, GuidService>("Singleton");
            builder.Services.AddKeyedScoped<IGuidService, GuidService>("Scoped");
            builder.Services.AddKeyedTransient<IGuidService, GuidService>("Transient");

            #region Configurationen Einlesen
            //1.) Hinzufügen einer weiteren Konfigurationsquelle
            builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("GameSettings.json", optional: true, true);
            });

            //2.) 
            builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));
            #endregion



            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options=>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.EnableAnnotations();
                options.IncludeXmlComments(xmlPath);
                options.OperationFilter<CsvSwaggerOperationFilter>();
            });

            //Bei Build
            var app = builder.Build();
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
