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
},new NotificationTemplate
{
    Type = NotificationType.gymPaymentSuccess,
    TitleAr = "تم الدفع بنجاح ✅",
    BodyAr = "تمت عملية الدفع بنجاح وتم تأكيد حجزك في {GymName}. اضغط لعرض رمز الدخول (QR Code).",
    TitleEn = "Payment Successful ✅",
    BodyEn = "Your payment was completed and your booking at {GymName} is confirmed. Tap to view your QR code."
},new NotificationTemplate
{
    Type = NotificationType.gymPaymentFailed,
    TitleAr = "فشلت عملية الدفع ❌",
    BodyAr = "لم تكتمل عملية الدفع لحجزك في {GymName}. اضغط لإعادة المحاولة وتأكيد الحجز.",
    TitleEn = "Payment Failed ❌",
    BodyEn = "Your payment for {GymName} did not go through. Tap to try the payment again and confirm your booking."
},
      new NotificationTemplate
{
    Type = NotificationType.newRideRequest,

    TitleAr = "طلب رحلة جديد 🚗",
    BodyAr = "طلب رحلة قريب منك. المسافة {Distance} كم والسعر {Price} جنيه",

    TitleEn = "New Ride Request 🚗",
    BodyEn = "New ride near you. Distance {Distance} km and price {Price} EGP"
},
      new NotificationTemplate
{
    Type = NotificationType.rideAccepted,

    TitleAr = "تم قبول الرحلة 🚗",
    BodyAr = "السائق {DriverName} قبل طلب رحلتك وهو في الطريق إليك",

    TitleEn = "Ride Accepted 🚗",
    BodyEn = "Driver {DriverName} accepted your ride and is on the way"
},



    };
    }
}
