var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly); 
    config.AddBehavior(typeof(BuildingBlocks.Behaviors.LoggingBehavior<,>));
    config.AddBehavior(typeof(BuildingBlocks.Behaviors.ValidationBehavior<,>));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
//builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseExceptionHandler();
app.Run();
