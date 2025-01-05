using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Middleware;
using NZWalks.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<NZWalksDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")) );

builder.Services.AddDbContext<AuthDbContext> (Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection")) );

builder.Services.AddScoped<IRegionsRepository, RegionsRepository>();
builder.Services.AddScoped<IWalksRepository, WalksRepository>();
builder.Services.AddScoped<ITokenRepository , TokenRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(Options => 
    {
        Options.Password.RequireDigit = false;
        Options.Password.RequireUppercase = false;
        Options.Password.RequireLowercase = false;
        Options.Password.RequireNonAlphanumeric = false;
        Options.Password.RequiredLength = 6;
        Options.Password.RequiredUniqueChars = 1;

    }
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(Options => 
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        }
        );
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions 
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
