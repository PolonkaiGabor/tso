global using beadando_szoftech.Models;
global using beadando_szoftech.Services;


var builder = WebApplication.CreateBuilder(args);

// Beállítások konfigurálása
builder.Services.Configure<ProjectDatabaseSettings>(
    builder.Configuration.GetSection("ProjectDatabase"));

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<HouseService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
