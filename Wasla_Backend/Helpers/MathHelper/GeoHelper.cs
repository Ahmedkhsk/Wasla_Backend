namespace Wasla_Backend.Helpers.MathHelper
{
    public static class GeoHelper
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) *
                Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }
        public static double CalculateDuration(double distanceKm)
        {
            double averageSpeed;

            if (distanceKm < 2)
                averageSpeed = 20; 
            else if (distanceKm < 10)
                averageSpeed = 30; 
            else
                averageSpeed = 50; 

            double hours = distanceKm / averageSpeed;

            return hours * 60; 
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
