using stela_api;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var startUp = new Startup(builder.Configuration);
startUp.ConfigureServices(builder.Services);

var app = builder.Build();
startUp.Configure(app, builder.Environment);
