using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Database>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/infections", async (DateTime time, Database db) =>
    await db.Infections.Where(infection =>
    Math.Abs(SqlServerDbFunctionsExtensions.DateDiffMinute(EF.Functions, infection.Timestamp, time)) < TimeSpan.FromHours(2).TotalMinutes)
    .ToListAsync()).WithName("GetInfections");

app.MapPost("/infection", async (ICollection<Infection> infection, Database db) =>
{
    await db.Infections.AddRangeAsync(infection);
    await db.SaveChangesAsync();
}).WithName("PostInfection");

app.Run();

class Infection
{
    public int Id { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

class Database : DbContext
{
    public Database(DbContextOptions options) : base(options) { }
    public DbSet<Infection> Infections { get; set; }
}
