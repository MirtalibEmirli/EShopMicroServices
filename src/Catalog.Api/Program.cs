 
var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();//TODO: bax niye loghtweight
//add services


var app = builder.Build();
//Configure Http request pipeline

app.MapCarter();

app.Run();
