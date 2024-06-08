using System.Text;
using stela_api.src.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeDetective;
using webApiTemplate.src.App.IService;
using webApiTemplate.src.App.Service;
using webApiTemplate.src.Domain.Entities.Config;
using Newtonsoft.Json;
using stela_api.src.App.IService;
using stela_api.src.App.Service;
using stela_api.src.Domain.Entities.Config;
using Swashbuckle.AspNetCore.Filters;
using MimeDetective.Definitions.Licensing;

namespace stela_api
{

    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            var jwtSettingsString = Environment.GetEnvironmentVariable(nameof(JwtSettings))
                                    ?? throw new Exception("JwtSettings is not found");
            var emailServiceSettinsString = Environment.GetEnvironmentVariable(nameof(EmailServiceSettings), EnvironmentVariableTarget.User)
                                            ?? throw new Exception("EmailServiceSettings is not found");

            var phoneServiceSettingsString = Environment.GetEnvironmentVariable(nameof(PhoneServiceSettings), EnvironmentVariableTarget.User)
                                       ?? throw new Exception("PhoneServiceSettings is not found");

            var jwtSettings = JsonConvert.DeserializeObject<JwtSettings>(jwtSettingsString)
                              ?? throw new Exception("jwt settings is not correct format");
            var emailServiceSettings = JsonConvert.DeserializeObject<EmailServiceSettings>(emailServiceSettinsString)
                                       ?? throw new Exception("EmailServiceSettings is not correct format");

            var phoneServiceSettings = JsonConvert.DeserializeObject<PhoneServiceSettings>(phoneServiceSettingsString)
                ?? throw new Exception("PhoneServiceSettings is not correct format");

            var fileInspector = new ContentInspectorBuilder()
            {
                Definitions = new MimeDetective.Definitions.CondensedBuilder()
                {
                    UsageType = UsageType.PersonalNonCommercial
                }.Build()
            }.Build();

            services.AddControllers(config => config.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>())
                    .AddNewtonsoftJson()
                    .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

            services.AddCors(setup => setup.AddDefaultPolicy(options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                }));

            services.AddEndpointsApiExplorer();
            services.AddDbContext<AppDbContext>();

            services
                .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                });

            services.AddAuthorization();
            services.AddLogging(builder => builder.AddConsole());


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "stela_api",
                    Description = "Api",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer auth scheme",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();

                options.EnableAnnotations();
            }).AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton(fileInspector);
            services.AddSingleton(jwtSettings);
            services.AddSingleton(emailServiceSettings);
            services.AddSingleton(phoneServiceSettings);

            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IPhoneService, PhoneService>();
            services.AddSingleton<IFileUploaderService, LocalFileUploaderService>();

            services.AddScoped<IPlotPriceCalculationService, PlotPriceCalculationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICreateMemorialService, CreateMemorialService>();

            services.Scan(scan => scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(scan => scan.FromCallingAssembly()
                    .AddClasses(classes =>
                        classes.Where(type =>
                            type.Name.EndsWith("Manager")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpLogging();
            app.UseRequestLocalization();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}