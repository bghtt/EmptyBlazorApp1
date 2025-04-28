using EmptyBlazorApp1;
using Npgsql;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:PostgreSQL")
    ?? builder.Configuration.GetValue<string>("ConnectionStrings__PostgreSQL")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQL");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("⚠️ PostgreSQL connection string not found, but continuing...");
    
}
else
{
    Console.WriteLine($"✅ Using PostgreSQL: {connectionString}");
}
builder.Services.AddScoped<AuthService>(_ =>
    new AuthService(connectionString ?? "Host=db;Port=5432;Database=database;Username=postgres;Password=rootroot"));


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddScoped<AuthService>(_ => new AuthService(connectionString));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();