using Microsoft.Extensions.DependencyInjection.Extensions;
using BMS.Application.Interfaces;
using BMS.Application.Services;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppService
    {
        public static IServiceCollection AddAppService(this IServiceCollection services)
        {
            services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Scoped<IBookService, BookService>(),
               
            });

            return services;
        }
    }
}
