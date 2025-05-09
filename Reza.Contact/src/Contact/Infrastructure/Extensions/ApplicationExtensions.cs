using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Contact.Infrastructure.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {

        builder.Services.AddDbContext<ContactUserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SvcDbContext")));



        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    }
}
