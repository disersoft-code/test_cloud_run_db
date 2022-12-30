using TestCloudRunDB.Data;
using TestCloudRunDB.Data.Repositories;
using TestCloudRunDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var redisConn = $"{builder.Configuration.GetValue<string>("Redis:IP")}:{builder.Configuration.GetValue<int>("Redis:Port")},abortConnect=false";

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConn;
});

var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("dsfcontrol"));
builder.Services.AddSingleton(mySQLConfiguration);

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
//builder.Services.AddSingleton<ITelegramBotService, TelegramBotService>();
//builder.Services.AddHostedService<TelegramBotHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
