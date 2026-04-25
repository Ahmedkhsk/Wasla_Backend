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
      new NotificationTemplate
{
    Type = NotificationType.rideCancelled,

    TitleAr = "تم إلغاء الرحلة ❌",
    BodyAr = "{UserName} قام بإلغاء الرحلة",

    TitleEn = "Ride Cancelled ❌",
    BodyEn = "{UserName} cancelled the ride"
},
      new NotificationTemplate
{
    Type = NotificationType.rideCompleted,

    TitleAr = "انتهت الرحلة ✅",
    BodyAr = "تمت الرحلة بنجاح مع السائق {DriverName}. لا تنسَ تقييم السائق ⭐",

    TitleEn = "Ride Completed ✅",
    BodyEn = "Your ride with {DriverName} is completed. Please rate the driver ⭐"
},
      new NotificationTemplate
{
    Type = NotificationType.technicianNewBookingRequest,

    TitleAr = "طلب حجز جديد 🔧",
    BodyAr = "{UserName} أرسل لك طلب حجز جديد",

    TitleEn = "New Booking Request 🔧",
    BodyEn = "{UserName} sent you a new booking request"
},
      new NotificationTemplate
{
    Type = NotificationType.doctorCompleteInfoScreen,

    TitleAr = "تم إكمال بياناتك بنجاح 🎉",
    BodyAr = "شكرًا لك! تم تسجيل بياناتك بنجاح ويمكنك الآن بدء استخدام التطبيق واستقبال الحجوزات",

    TitleEn = "Profile Completed 🎉",
    BodyEn = "Thank you! Your information has been successfully submitted. You can now start using the app and receive bookings"
},
      new NotificationTemplate
{
    Type = NotificationType.doctorBookingScreen,

    TitleAr = "حجز جديد 🩺",
    BodyAr = "{UserName} قام بحجز موعد معك بتاريخ {Date}",

    TitleEn = "New Booking 🩺",
    BodyEn = "{UserName} booked an appointment with you on {Date}"
},
  new NotificationTemplate
{
    Type = NotificationType.doctorEditBookingScreen,

    TitleAr = "تم تعديل الحجز ✏️",
    BodyAr = "قام الدكتور بتعديل تفاصيل حجزك",

    TitleEn = "Booking Updated ✏️",
    BodyEn = "Doctor has updated your booking"
},
  new NotificationTemplate
{
    Type = NotificationType.doctorCancelBookingScreen,

    TitleAr = "تم إلغاء الحجز ❌",
    BodyAr = "قام الدكتور بإلغاء حجزك",

    TitleEn = "Booking Cancelled ❌",
    BodyEn = "Doctor has cancelled your booking"
}
  ,
  new NotificationTemplate
{
    Type = NotificationType.residentCancelDoctorBooking,

    TitleAr = "تم إلغاء الحجز ❌",
    BodyAr = "قام الساكن بإلغاء حجزك",

    TitleEn = "Booking Cancelled ❌",
    BodyEn = "Resident has cancelled your booking"
},
  new NotificationTemplate
{
    Type = NotificationType.messageReceived,

    TitleAr = "رسالة جديدة 💬",
    BodyAr = "لديك رسالة جديدة من {SenderName}",

    TitleEn = "New Message 💬",
    BodyEn = "You have a new message from {SenderName}"
},
  new NotificationTemplate
{
    Type = NotificationType.driverCompleteInfoScreen,

    TitleAr = "تم إكمال التسجيل 🎉",
    BodyAr = "شكرًا لك! تم تسجيل بياناتك بنجاح ويمكنك الآن البدء في استقبال الرحلات",

    TitleEn = "Registration Completed 🎉",
    BodyEn = "Thank you! Your registration is complete. You can now start receiving rides"
},
new NotificationTemplate
{
    Type = NotificationType.gymCompleteInfoScreen,

    TitleAr = "تم إكمال البيانات 🎉",
    BodyAr = "شكرًا لك! تم تسجيل بيانات الجيم بنجاح ويمكنك الآن إدارة حسابك",

    TitleEn = "Profile Completed 🎉",
    BodyEn = "Thank you! Your gym profile has been successfully created"
},
new NotificationTemplate
{
    Type = NotificationType.gymPackageBooked,

    TitleAr = "تم حجز باقة جديدة 🎉",
    BodyAr = "تم حجز باقة {PackageName} بواسطة {UserName}",

    TitleEn = "New Package Booking 🎉",
    BodyEn = "{UserName} booked {PackageName} package"
},
new NotificationTemplate
{
    Type = NotificationType.gymPackageExpired,

    TitleAr = "انتهت الباقة ⏳",
    BodyAr = "انتهت صلاحية باقتك، يمكنك تجديدها الآن",

    TitleEn = "Package Expired ⏳",
    BodyEn = "Your package has expired. Please renew it to continue"
},
new NotificationTemplate
{
    Type = NotificationType.gymBookingCancelled,

    TitleAr = "تم إلغاء الحجز ❌",
    BodyAr = "قام {UserName} بإلغاء حجز باقة {PackageName}",

    TitleEn = "Booking Cancelled ❌",
    BodyEn = "{UserName} cancelled {PackageName} booking"
},
new NotificationTemplate
{
    Type = NotificationType.residentCompleteInfoScreen,

    TitleAr = "تم إكمال بياناتك 🎉",
    BodyAr = "تم تسجيل بياناتك بنجاح ويمكنك الآن استخدام جميع خدمات التطبيق",

    TitleEn = "Profile Completed 🎉",
    BodyEn = "Your profile is complete. You can now use all app features"
},
new NotificationTemplate
{
    Type = NotificationType.technicianCompleteInfoScreen,

    TitleAr = "تم إكمال بياناتك 🎉",
    BodyAr = "تم تسجيل بياناتك بنجاح ويمكنك الآن استقبال طلبات الخدمات",

    TitleEn = "Profile Completed 🎉",
    BodyEn = "Your profile is complete. You can now receive service requests"
},
new NotificationTemplate
{
    Type = NotificationType.technicianAcceptBooking,

    TitleAr = "تم قبول الحجز ✅",
    BodyAr = "قام الفني بقبول طلب الخدمة الخاص بك",

    TitleEn = "Booking Accepted ✅",
    BodyEn = "The technician has accepted your service request"
},
new NotificationTemplate
{
    Type = NotificationType.technicianRejectBooking,

    TitleAr = "تم رفض الحجز ❌",
    BodyAr = "قام الفني برفض طلب الخدمة الخاص بك",

    TitleEn = "Booking Rejected ❌",
    BodyEn = "The technician has rejected your service request"
},
new NotificationTemplate
{
    Type = NotificationType.userTechnicianBookingCancelled,

    TitleAr = "تم إلغاء الحجز ❌",
    BodyAr = "تم إلغاء طلب الخدمة الخاص بك",

    TitleEn = "Booking Cancelled ❌",
    BodyEn = "Your service request has been cancelled"
},
new NotificationTemplate
{
    Type = NotificationType.technicianCancelBooking,

    TitleAr = "تم إلغاء الحجز ❌",
    BodyAr = "قام العميل بإلغاء طلب الخدمة",

    TitleEn = "Booking Cancelled ❌",
    BodyEn = "The customer has cancelled the service request"
},
new NotificationTemplate
{
    Type = NotificationType.postCommented,

    TitleAr = "تعليق جديد 💬",
    BodyAr = "{UserName} علّق على منشورك",

    TitleEn = "New Comment 💬",
    BodyEn = "{UserName} commented on your post"
},
new NotificationTemplate
{
    Type = NotificationType.postReacted,

    TitleAr = "تفاعل جديد ❤️",
    BodyAr = "{UserName} تفاعل على منشورك",

    TitleEn = "New Reaction ❤️",
    BodyEn = "{UserName} reacted to your post"
},



    };
    }
}
