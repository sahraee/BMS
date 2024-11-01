using Microsoft.Extensions.DependencyInjection.Extensions;
using BMS.Domain.Interfaces;
using BMS.Persistence.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
  
    public static class PersistenceService
    {

        public static IServiceCollection AddPersistenceService(this IServiceCollection services)
        {
            services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Scoped<IBookRepository, BookRepository>()              
             
            });

            return services;
        }
    }
}
