using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hiwell.AddressBook.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection ConfigureCoreDependecies(this IServiceCollection services)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assembly);

            services.AddMediatR(assembly);

            return services;
        }

    }
}
