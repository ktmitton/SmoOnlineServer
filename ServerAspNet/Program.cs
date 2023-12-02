using SuperMarioOdysseyOnline.Server.Events;
using SuperMarioOdysseyOnline.Server.Players;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;
using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Scenarios;
using SuperMarioOdysseyOnline.Server.Tcp;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDefaultUpdateStrategy();
builder.Services.AddPlayers();
builder.Services.AddPacketHandler();
builder.Services.AddEventStream();
builder.Services.AddControllers();
builder.Services.AddTcpConnections(builder.Configuration);
builder.Services.AddSingleton<IScenarioManager, ScenarioManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.Use((context, next) =>
// {
//     context.Request.EnableBuffering();

//     return next(context);
// });

app.MapControllers();

//app.MapSocket<LegacyConnectionHandler>(IPAddress.Loopback, 5321);

app.Run();
