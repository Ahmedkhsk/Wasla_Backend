namespace Wasla_Backend.Helpers.NotificationHelper
{
    public static class NotificationTemplateEngine
    {
        public static (string title, string body) Generate(
            NotificationType type,
            string language,
            Dictionary<string, string>? metadata = null)
        {
            var template = NotificationTemplates.Templates
                .FirstOrDefault(t => t.Type == type);

            if (template == null)
                throw new BadRequestException(LocalizationKey.TemplateNotFound);

            string title = language == "ar" ? template.TitleAr : template.TitleEn;
            string body = language == "ar" ? template.BodyAr : template.BodyEn;

            if (metadata != null)
            {
                foreach (var item in metadata)
                {
                    var value = item.Value ?? string.Empty; 

                    if (title.Contains($"{{{item.Key}}}"))
                        title = title.Replace($"{{{item.Key}}}", value);

                    if (body.Contains($"{{{item.Key}}}"))
                        body = body.Replace($"{{{item.Key}}}", value);
                }
            }

            return (title, body);
        }
    }
}
