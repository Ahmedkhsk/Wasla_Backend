namespace Wasla_Backend.Helpers.NotificationHelper
{
    public static class NotificationTemplates
    {
        public static IReadOnlyList<NotificationTemplate> Templates = new List<NotificationTemplate>
    {
        new NotificationTemplate
        {
            Type = NotificationType.EventCancellation,
            TitleAr = "تقييم جديد",
            BodyAr = "{UserName} أضاف تقييمًا جديدًا",

            TitleEn = "New Review",
            BodyEn = "{UserName} left a new review"
        },

        new NotificationTemplate
        {
            Type = NotificationType.EventUpdate,
            TitleAr = "اكتمل الحجز",
            BodyAr = "{DoctorName}حجزك مع  اكتمل",

            TitleEn = "Booking Completed",
            BodyEn = "Your booking with {DoctorName} is completed"
        }
    };
    }
}
