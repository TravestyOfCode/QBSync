using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QBSync.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
