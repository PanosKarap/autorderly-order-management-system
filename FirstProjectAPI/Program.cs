using FirstProjectAPI.Data;
using FirstProjectAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// --- 1. SERVICES (Ρυθμίσεις) ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Ρυθμίσεις για το Swagger (Swashbuckle) με υποστήριξη JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FirstProjectAPI", Version = "v1" });

    // 1. Ορίζουμε πώς θα δουλεύει το κουμπί "Authorize"
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Βάλε το Token σου εδώ γράφοντας τη λέξη 'Bearer', κενό, και μετά το Token.\n\nΠαράδειγμα: 'Bearer eyJhbGciOiJIUzI1Ni...'"
    });

    // 2. Λέμε στο Swagger να χρησιμοποιεί αυτό το σύστημα σε κάθε κλειδωμένο Endpoint
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Σύνδεση με τη Βάση Δεδομένων
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ρύθμιση του ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false; // Το κάνουμε false για ευκολία στο testing
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Ρύθμιση του JWT Authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Πολιτική CORS για τη React
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


var app = builder.Build();

// --- 2. PIPELINE (Η ροή της εφαρμογής) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ΣΗΜΑΝΤΙΚΟ: Το CORS πρέπει να είναι πριν το Authentication/Authorization
app.UseCors("AllowReactApp");

// ΣΗΜΑΝΤΙΚΟ: Το Authentication ΠΡΙΝ το Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// --- 3. DATA SEEDING (Δημιουργία Ρόλων & Πρώτου Χρήστη) ---
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // 1. Δημιουργία των Ρόλων ανεξάρτητα από τους χρήστες
    if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
    }
    if (!roleManager.RoleExistsAsync("StoreOwner").GetAwaiter().GetResult())
    {
        roleManager.CreateAsync(new IdentityRole("StoreOwner")).GetAwaiter().GetResult();
    }

    // 2. Εύρεση ή Δημιουργία του Admin Χρήστη
    var adminEmail = "admin@test.com";
    var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            StoreName = "My First Store",
            EmailConfirmed = true
        };
        userManager.CreateAsync(adminUser, "Admin123!").GetAwaiter().GetResult();
    }

    // 3. Σύνδεση του Χρήστη με τον Ρόλο "Admin"
    if (!userManager.IsInRoleAsync(adminUser, "Admin").GetAwaiter().GetResult())
    {
        userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
    }
}

app.Run();