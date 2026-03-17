namespace Wasla_Backend.Helpers.Extensions
{

    public static class EnumExtensions
    {
        public static string GetName(this Enum value, string lang = "en")
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return value.ToString();

            var attribute = field.GetCustomAttribute<DisplayAttribute>();

            if (attribute == null)
                return value.ToString();

            if (lang == "ar")
                return attribute.Description ?? attribute.Name ?? value.ToString();

            return attribute.Name ?? value.ToString();
        }
    }
}
