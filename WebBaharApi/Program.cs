using Bahar.Application.InterfaceContext;
using Bahar.Application.InterfaceRepository;
using Bahar.Persistence.Context;
using Bahar.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using WebBaharApi.Helper;

var builder = WebApplication.CreateBuilder(args);

// 1. افزودن DbContext با اتصال به SQL Server
builder.Services.AddDbContext<BaharDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 2. ثبت ریپازیتوری‌ها با Scoped lifetime
builder.Services.AddScoped<IDatabaseContext>(provider => provider.GetService<BaharDbContext>());

// ثبت AutoMapper فقط یک بار
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// 3. افزودن کنترلرها
builder.Services.AddControllers();

// 4. افزودن Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. ثبت SeedData اگر لازم داری
builder.Services.AddTransient<SeedData>();

var app = builder.Build();

// 6. اجرای SeedData اگر آرگومان "seeddata" داده شده باشد (می‌تونی داخل شرط محیط توسعه هم قرار بدی)
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedData>();
        service.SeedDatabase();
    }
}

// 7. فعال‌سازی Swagger و ریدایرکت روت فقط در محیط توسعه
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger/index.html");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
