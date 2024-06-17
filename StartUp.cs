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
using stela_api.src.Domain.Models;
using stela_api.src.Domain.Enums;
using webApiTemplate.src.App.Provider;

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
            services.AddScoped<ICreatePortfolioMemorialService, CreatePortfolioMemorialService>();

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

            InitDatabase(app.Services);
            app.Run();
        }

        private void InitDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var account = context.Accounts.FirstOrDefault(e => e.Email == "admin@admin.ru");
            if (account != null) return;

            var adminAccount = new Account
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.ru",
                PasswordHash = Hmac512Provider.Compute("htop"),
                RoleName = UserRole.Admin.ToString(),
                IsEmailVerified = true,
            };

            context.Accounts.Add(adminAccount);

            var materials = new List<MemorialMaterial>()
            {
                new() { Name = "Шанси Блэк", ColorName = "Черный", Image = "44_Shanxi_Black.png" },
                new() { Name = "Змеевик", ColorName = "Зеленый", Image = "uralskiy-zmeevik.jpg"},
                new() { Name = "Габбро-Диабаз", ColorName = "Черный", Hex = "484848"},
                new() { Name = "Диабаз", ColorName = "Серый", Image = "диабаз.jpg" },
                new() { Name = "Коелга", ColorName = "Белый",  Image = "mramor-koelga.jpg"},
                new() { Name = "Абсолют Блэк", ColorName = "Черный", Image = "absolute_black.jpg" },
                new() { Name = "Олив Грин", ColorName = "Зеленый", Hex = "549079" },
                new() { Name = "Паданг Дарк", ColorName = "Серый", Image = "Padang_Dark.jpg" },
                new() { Name = "Роял Уайт", ColorName = "Белый", Image = "royale_white.jpg"},
                new() { Name = "Чайна Марбал Уайт", ColorName = "Белый", Image = "чайна_вайт.jpg"},
                new() { Name = "Империал Рэд", ColorName = "Красный", Image = "imperial_Red.jpg"},
                new() { Name = "Куру Грей", ColorName = "Серый", Image = "curu_gray.jpg"}
            };
            context.Materials.AddRange(materials);

            var additionalServices = new List<AdditionalService>()
            {
                new() { Name = "Мемориальные комплексы", Price = 10000, Image = "изготовление.png"},
                new() { Name = "Изготовление скульптур", Price = 10000, Image = "изг.png"},
                new() { Name = "Эксклюзивные памятники", Price = 10000,  Image = "эксклюзив.png"}
            };
            context.AdditionalServices.AddRange(additionalServices);

            var memorials = new List<Memorial>()
            {
                new()
                {
                    Name = "Проект 1",
                    Price = 40000,
                    Image = "image1.png",
                    Description = "",
                    StelaHeight = 1,
                    StelaLength = 1,
                    StelaWidth = 1,
                    Materials = new List<MemorialMaterials>()
                    {
                        new() { Material = materials[0]}
                    }
                },

                new()
                {
                    Name = "Проект 2",
                    Price = 40000,
                    Image = "image2.png",
                    Description = "",
                    StelaHeight = 1,
                    StelaLength = 1,
                    StelaWidth = 1,
                    Materials = new List<MemorialMaterials>()
                    {
                        new() { Material = materials[3]},
                    }
                },

                new()
                {
                    Name = "Проект 3",
                    Price = 40000,
                    Image = "image3.png",
                    Description = "",
                    StelaHeight = 1,
                    StelaLength = 1,
                    StelaWidth = 1,
                    Materials = new List<MemorialMaterials>()
                    {
                        new() { Material = materials[5]},
                        new() { Material = materials[9]},
                    }
                },

                new()
                {
                    Name = "Проект 4",
                    Price = 40000,
                    Image = "image4.png",
                    Description = "",
                    StelaHeight = 1,
                    StelaLength = 1,
                    StelaWidth = 1,
                    Materials = new List<MemorialMaterials>()
                    {
                        new() { Material = materials[10]},
                        new() { Material = materials[11]},
                    }
                }
            };

            context.Memorials.AddRange(memorials);

            var portfolioMemorials = new List<PortfolioMemorial>()
            {
                new() { CemeteryName = "Православное кладбище", Name = "МОНУМЕНТАЛЬНЫЙ МЕМОРИАЛЬНЫЙ КОМПЛЕКС", Description = "", Images="port1_1.png;port1_2.png;port1_3.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[0]}
                }},

                new() { CemeteryName = "Православное кладбище",Description = "", Name = "ЭКСКЛЮЗИВНЫЙ ПАМЯТНИК ИЗ ЗМЕЕВИКА", Images = "port2_1.png;port2_2.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[1]},
                    new() { Material = materials[2]}
                }},

                new() { CemeteryName = "Православное кладбище",Description = "", Name = "МЕМОРИАЛЬНЫЙ КОМПЛЕКС", Images = "port3_1.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[1]},
                    new() { Material = materials[3]},
                    new() { Material = materials[4]}
                }},

                new() { CemeteryName = "Православное кладбище",Description = "", Name = "МЕМОРИАЛЬНЫЙ КОМПЛЕКС", Images = "4_1.png;4_2.png;4_3.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[5]},
                    new() { Material = materials[6]},
                }},

                new() { CemeteryName = "Православное кладбище",Description = "", Name = "АВТОРСКАЯ НАДГРОБНАЯ КОМПОЗИЦИЯ", Images = "5.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[7]},
                }},

                new() { CemeteryName = "Православное кладбище",Description = "", Name = "ЭЛИТНЫЙ МЕМОРИАЛЬНЫЙ КОМПЛЕКС", Images = "6.png", Materials = new List<PortfolioMemorialMaterials>()
                {
                    new() { Material = materials[0]},
                    new() { Material = materials[8]},
                }}
            };

            context.PortfolioMemorials.AddRange(portfolioMemorials);

            context.SaveChanges();
        }
    }
}