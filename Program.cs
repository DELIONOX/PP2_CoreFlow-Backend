using CoreFlow_Backend.Data;
using CoreFlow_Backend.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Cors de Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy( "PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//Controllers
builder.Services.AddControllers();

//Servicios
/*
builder.Services.AddSingleton<ClienteService>();
builder.Services.AddSingleton<PedidoService>();
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<ProveedorService>();
*/
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorService>();

//Conexion con la DB SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
     builder.Configuration.GetConnectionString("Conexion")
    )
);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app=builder.Build();

//Entorno para visualizar en Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PermitirAngular");
app.UseAuthorization();
app.MapControllers();
app.Run();