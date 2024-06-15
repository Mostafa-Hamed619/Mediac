using MediacApi.Data;
using MediacBack.Services.IRepositories;
using MediacBack.Services.MockRepositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
builder.Services.AddScoped<IBlogRepository, BlogMockRepository>();
builder.Services.AddScoped<IFileRespository, FileMockRepository>();
builder.Services.AddScoped<IPostRepository,PostMockRepositories>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
