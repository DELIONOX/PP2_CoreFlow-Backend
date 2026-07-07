using CoreFlow_Backend.Data;
using CoreFlow_Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// CORS de Angular (Adaptado para Render)
// =====================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://tu-app-angular.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =====================================
// Controllers & Servicios
// =====================================
builder.Services.AddControllers();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<PedidoService>();

// =====================================
// Conexión con SQL Server
// =====================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion"))
);

// =====================================
// Swagger
// =====================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================================
// Pipeline de la Aplicación (Middleware)
// =====================================
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Comentado: Render maneja la terminación SSL de forma externa

app.UseCors("PermitirAngular");
app.UseAuthorization();
app.MapControllers();
app.Run();