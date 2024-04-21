using FinalProject.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
    x.AllowAnyHeader() // Allows any headers in the request
    .AllowAnyMethod() // Allows any HTTP method (GET, POST, etc.)
    .AllowCredentials() // Allows credentials to be included in the request (such as cookies, authorization headers)
    .SetIsOriginAllowed(_ => true) // Allow any origin
);
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
app.MapHub<ChatHub>("/chat");
 app.MapHub<SignalHub>("/signal");
app.UseAuthorization();

app.MapControllers();
       
app.Run();
