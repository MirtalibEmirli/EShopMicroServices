 
var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

//add services


var app = builder.Build();
//Configure Http request pipeline

app.MapCarter();

app.Run();
