using Wasla_Backend.Models.Driver;
using Wasla_Backend.Models.GymModel;
using Wasla_Backend.Models.technician;

namespace Wasla_Backend.Helpers.Resolvers
{
    public static class ServiceProviderTypeResolver
    {
        private static readonly Dictionary<Type, ServiceProviderType> Map = new()
    {
        { typeof(Doctor), ServiceProviderType.Doctor },
        { typeof(Restaurant), ServiceProviderType.Restaurant },
        { typeof(Driver), ServiceProviderType.Driver },
        { typeof(Gym), ServiceProviderType.Gym },
          {typeof(Technician),ServiceProviderType.Technician }


    };

        public static ServiceProviderType Resolve(ApplicationUser user)
        {
            var type = user.GetType();

            if (!Map.TryGetValue(type, out var serviceType))
                throw new Exception("InvalidServiceProviderType");

            return serviceType;
        }
        public static Models.ServiceProvider ResolveServiceProvider(ServiceProviderType serviceType, ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                return null!; 

            return serviceType switch
            {
                ServiceProviderType.Doctor => applicationUser as Doctor,
                ServiceProviderType.Driver => applicationUser as Driver,
                ServiceProviderType.Gym => applicationUser as Gym,
                ServiceProviderType.Restaurant => applicationUser as Restaurant,
                ServiceProviderType.Technician => applicationUser as Technician,
                _ => throw new ArgumentException($"Unknown ServiceProviderType: {serviceType}")
            };
        }

    }

}
