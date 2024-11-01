using Microsoft.Extensions.DependencyInjection;
 

namespace BMS.IOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection service)        {
             
            service.AddAppService().AddPersistenceService();
        }
    }
}
