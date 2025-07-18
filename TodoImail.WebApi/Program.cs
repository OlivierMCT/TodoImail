using Microsoft.EntityFrameworkCore;
using TodoImail.Services.DbContexts;
using TodoImail.Services.Repositories;
using TodoImail.Services.Services;
using TodoImail.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Singleton => 1 seule instance pour toute l'application
// Transient => 1 instance par demande d'injection
// Scoped => 1 instance pour un scope = le traitement d'une requete HTTP

builder.Services.AddScoped<ITodoImailService, TodoImailServiceDefaultImplementation>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped(sp => {
//    DbContextOptionsBuilder builder = new();
//    builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TodoWebApiBase;Integrated Security=True");

//    return new TodoImailDbContext(builder.Options);
//});
builder.Services.AddDbContext<TodoImailDbContext>(
    optionsBuilders => optionsBuilders
        .EnableSensitiveDataLogging()
        .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TodoWebApiBase;Integrated Security=True")
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(config => {
    config.AddPolicy("ATP-Formation", p => p.WithOrigins("https://www.atp-formation.com").AllowAnyMethod().AllowAnyHeader());
    config.AddPolicy("EveryoneFullControl", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    //config.AddPolicy();
    //config.AddPolicy();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TodoImailDbContext>();
    context.Database.EnsureCreated();
    if(!context.Categories.Any()) {
        context.Categories.AddRange(Data.GetCategories());
        context.SaveChanges();
    }

    if (!context.Todos.Any()) {
        context.Todos.AddRange(Data.GetTodos(context.Categories.ToList()));
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseCors("EveryoneFullControl");
app.MapControllers();

app.Run();
