using MediacApi.Data;
using MediacApi.Data.Entities;
using MediacApi.Services;
using MediacBack.Services.IRepositories;
using MediacBack.Services.MockRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;
using MediacApi.Middleware;
using MediacApi.Services.IRepositories;
using MediacApi.Services.MockRepositories;
using MediacApi.PayPalClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/AppLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Services.AddDbContextPool<MediacDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MediacDbConnection"));
});

builder.Services.AddControllers();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<IBlogRepository, BlogMockRepository>();
builder.Services.AddScoped<IFileRespository, FileMockRepository>();
builder.Services.AddScoped<IPostRepository,PostMockRepositories>();
builder.Services.AddSingleton<ihttpAccessor, MockIhttpAccessor>();
builder.Services.AddScoped<ISubscribeRepo, SubscribeMockRepo>();
builder.Services.AddScoped<iUserRepository, UserMockRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton(x =>
new PayPalClientDto(
    builder.Configuration["PayPal:ClientID"],
    builder.Configuration["PayPal:Secret"],
    builder.Configuration["PayPal:Mode"]
    )
);

builder.Services.AddScoped<IFollowRepository,  FollowMockRepository>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type =ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "outh2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;

    opt.SignIn.RequireConfirmedEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<MediacDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateIssuer = true,
            ValidateAudience = false,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
builder.Services.AddMemoryCache(opt =>
{
    opt.SizeLimit = 1024;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images/PostImages")),
    RequestPath = "/PostImages"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images/BlogImages")),
    RequestPath = "/BlogImages"
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images/PostImages")),
    RequestPath = "/PostImages"
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images/BlogImages")),
    RequestPath = "/BlogImages"
});
app.UseHttpsRedirection();

app.UseAuthentication();


app.UseMiddleware<userInfoMiddleware>();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.UseCors(opt =>
{
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
app.MapControllers();

app.Run();
