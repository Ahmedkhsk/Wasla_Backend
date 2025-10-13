namespace Wasla_Backend.Factories.Implementation
{
    public class UserFactory : IUserFactory
    {
        public ApplicationUser CreateUser(string role)
        {
            return role.ToLower() switch
            {
                "doctor" => new Doctor(),
                "driver" => new Driver(),
                "resident" => new Resident(),
                "restaurantOwner" => new Restaurant(),
                "gymOwner" => new Gym(),
                "technician" => new Technician(),
                _ => throw new NotFoundException($"Unsupported role: {role}")
            };
        }
    }
}
