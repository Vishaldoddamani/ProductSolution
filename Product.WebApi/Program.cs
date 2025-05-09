using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Application.Services;
using Product.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


//Middleware code to configure various services and intercept and code accordingly.
// Add services to the container.

builder.Services.AddControllers().AddOData(options => 
        options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100));
    ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Configures the EF core to use in memory database using Microsoft.EntityFrameworkCore.InMemory package and uses other ef core packaged for persisting the data.
builder.Services.AddDbContextPool<ProductDbContext>(
    options =>
         options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

//Register Swagger for endpoint detail documentation and testing.
builder.Services.AddSwaggerGen();


// Resolves dependency when IProductRepository   .
// A new instance od scoped service is created once per request within the scope.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//There is seperation of concern by repository pattern and clean architecture are
// Separation of Concerns
//Unit Testing and Testability 
//  Ease of Maintenance 
// Caching and Performance Optimization 
// Handling Multiple Data Sources etc
builder.Services.AddScoped<ProductService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:57265")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        dbContext.Database.Migrate();
        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.Run();

public partial class Program { }