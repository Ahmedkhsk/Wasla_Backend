namespace Wasla_Backend.Services.Implementation.General
{
    public class ServiceDispatcher
    {
        private readonly IEnumerable<IServiceHandler> _serviceHandlers;
        public ServiceDispatcher(IEnumerable<IServiceHandler> serviceHandlers)
        {
            _serviceHandlers = serviceHandlers;
        }
  
    }
}
