using DBAccess;
using DBAccess.Repositories;
using GadgetsAPI.Tg;
using GadgetsAPI.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<AppExceptionFilter>();
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IGadgetRepository, GadgetRepository>();
builder.Services.AddSingleton<IProducerRepository, ProducerRepository>();
builder.Services.AddSingleton<DBConfig>(services =>
{
    var configuration = services.GetRequiredService<IConfiguration>();
    return new DBConfig
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection")
    };
});

//don't want to publish my token
builder.Services.AddTelegram("token");

var app = builder.Build();

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true)
                  .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
