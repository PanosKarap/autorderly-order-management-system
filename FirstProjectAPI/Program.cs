using Microsoft.EntityFrameworkCore;
using FirstProjectAPI.Data; // Σιγουρέψου ότι έχεις αυτό για να βλέπει το AppDbContext

var builder = WebApplication.CreateBuilder(args);

// --- 1. Services (Ρυθμίσεις) ---

builder.Services.AddControllers();

// Ρυθμίσεις για το Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Σύνδεση με τη Βάση Δεδομένων
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 1. Ορισμός της πολιτικής
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowReactApp");

// --- 2. Pipeline (Η ροή της εφαρμογής) ---

if (app.Environment.IsDevelopment())
{
    // Ενεργοποίηση του Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Η ΓΡΑΜΜΗ ΤΕΡΜΑΤΙΣΜΟΥ (Τίποτα μετά από αυτό!)
app.Run();