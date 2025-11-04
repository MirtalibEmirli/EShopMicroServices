var builder = WebApplication.CreateBuilder(args);

//add services


var app = builder.Build(); 
//Configure Http request pipeline

 
app.Run();
