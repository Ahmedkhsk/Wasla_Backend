namespace Wasla_Backend.Helpers.NotificationHelper
{
    public static class NotificationTemplates
    {
        public static IReadOnlyList<NotificationTemplate> Templates = new List<NotificationTemplate>
    {
       new NotificationTemplate
{
          Type = NotificationType.reviewScreen,

           TitleAr = "تقييم جديد ⭐",
           BodyAr = "{UserName} أضاف تقييمًا جديدًا ({Rating} ⭐)",

           TitleEn = "New Review ⭐",
           BodyEn = "{UserName} left a new review ({Rating} ⭐)"
},
       new NotificationTemplate
{
    Type = NotificationType.allFavouritesScreen,
    TitleAr = "تمت إضافتك للمفضلة ❤️",
    BodyAr = "{UserName} أضافك إلى المفضلة",
    TitleEn = "Added to Favorites ❤️",
    BodyEn = "{UserName} added you to favorites"
},



    };
    }
}
