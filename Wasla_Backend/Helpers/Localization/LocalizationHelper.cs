namespace Wasla_Backend.Helpers.Localization
{
    public class LocalizationHelper
    {
        private static readonly Dictionary<LocalizationKey, Dictionary<string, string>> messages = new()
        {
            #region Auth & User
            [LocalizationKey.UserNotLoggedIn] = new()
            {
                ["en"] = "User is not logged in.",
                ["ar"] = "المستخدم غير مسجل الدخول."
            },
            [LocalizationKey.InvalidRequest] = new()
            {
                ["en"] = "Invalid Request Data",
                ["ar"] = "البيانات المطلوبة غير صالحة"
            },
            [LocalizationKey.InvalidData] = new()
            {
                ["en"] = "Invalid data provided.",
                ["ar"] = "تم تقديم بيانات غير صالحة."
            },
            [LocalizationKey.UserNameAlreadyExists] = new()
            {
                ["en"] = "Username is already taken.",
                ["ar"] = "اسم المستخدم مستخدم بالفعل."
            },
            [LocalizationKey.EmailAlreadyExists] = new()
            {
                ["en"] = "Email is already taken.",
                ["ar"] = "البريد الإلكتروني مستخدم بالفعل."
            },
            [LocalizationKey.RegistrationSuccess] = new()
            {
                ["en"] = "User registered successfully.",
                ["ar"] = "تم تسجيل المستخدم بنجاح."
            },
            [LocalizationKey.RegistrationFailed] = new()
            {
                ["en"] = "User registration failed.",
                ["ar"] = "فشل تسجيل المستخدم."
            },
            [LocalizationKey.VerificationSuccess] = new()
            {
                ["en"] = "User verification successful.",
                ["ar"] = "تم التحقق من المستخدم بنجاح."
            },
            [LocalizationKey.VerificationFailed] = new()
            {
                ["en"] = "Verification code is wrong.",
                ["ar"] = "رمز التحقق خاطئ."
            },
            [LocalizationKey.OTPSend] = new()
            {
                ["en"] = "The OTP code has been sent.",
                ["ar"] = "تم إرسال رمز التحقق."
            },
            [LocalizationKey.LoginSuccess] = new()
            {
                ["en"] = "User logged in successfully.",
                ["ar"] = "تم تسجيل الدخول بنجاح."
            },
            [LocalizationKey.LoginFailed] = new()
            {
                ["en"] = "Email or password invalid.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            [LocalizationKey.UserNotFound] = new()
            {
                ["en"] = "User not found.",
                ["ar"] = "لم يتم العثور على المستخدم."
            },
            [LocalizationKey.UserNotVerified] = new()
            {
                ["en"] = "User email is not verified.",
                ["ar"] = "لم يتم التحقق من بريد المستخدم."
            },
            [LocalizationKey.UserNotApproved] = new()
            {
                ["en"] = "User is not approved yet.",
                ["ar"] = "المستخدم لم يتم الموافقة عليه بعد."
            },
            [LocalizationKey.Unauthorized] = new()
            {
                ["en"] = "Unauthorized access.",
                ["ar"] = "دخول غير مصرح به."
            },
            [LocalizationKey.Usernotloggedin] = new()
            {
                ["en"] = "User is not logged in.",
                ["ar"] = "المستخدم غير مسجل الدخول."
            },
            [LocalizationKey.UserLoggedOutSuccess] = new()
            {
                ["en"] = "User logged out successfully.",
                ["ar"] = "تم تسجيل خروج المستخدم بنجاح."
            },
            [LocalizationKey.GetAllUsersSuccess] = new()
            {
                ["en"] = "All users fetched successfully",
                ["ar"] = "تم جلب جميع المستخدمين بنجاح"
            },
            [LocalizationKey.DeleteUserSuccess] = new()
            {
                ["en"] = "User deleted successfully",
                ["ar"] = "تم حذف المستخدم بنجاح"
            },
            [LocalizationKey.UserBlockedDueToViolations] = new()
            {
                ["en"] = "User is blocked due to multiple violations.",
                ["ar"] = "المستخدم محظور بسبب انتهاكات متعددة."
            },
            [LocalizationKey.FailedToChangeUserStatus] = new()
            {
                ["en"] = "Failed to change user status.",
                ["ar"] = "فشل تغيير حالة المستخدم."
            },
            [LocalizationKey.SuccessToChangeUserStatus] = new()
            {
                ["en"] = "User status changed successfully.",
                ["ar"] = "تم تغيير حالة المستخدم بنجاح."
            },
            [LocalizationKey.SuccessToGetUserDetails] = new()
            {
                ["en"] = "User details retrieved successfully.",
                ["ar"] = "تم جلب بيانات المستخدم بنجاح."
            },
            [LocalizationKey.SuccessToGetUserApproveResponses] = new()
            {
                ["en"] = "User approval responses retrieved successfully.",
                ["ar"] = "تم جلب بيانات اعتماد المستخدمين بنجاح."
            },
            [LocalizationKey.SuccessToCreateUserEvent] = new()
            {
                ["en"] = "User event created successfully.",
                ["ar"] = "تم إنشاء حدث المستخدم بنجاح."
            },
            #endregion

            #region Password

            [LocalizationKey.NewPasswordSameAsOld] = new()
            {
                ["en"] = "The new password cannot be the same as the old password.",
                ["ar"] = "لا يمكن أن تكون كلمة المرور الجديدة هي نفسها كلمة المرور القديمة."
            },
            [LocalizationKey.ProfileEditSuccess] = new()
            {
                ["en"] = "Profile updated successfully.",
                ["ar"] = "تم تحديث الملف الشخصي بنجاح."
            },
            [LocalizationKey.ChangePassSuccess] = new()
            {
                ["en"] = "Password changed successfully.",
                ["ar"] = "تم تغيير كلمة المرور بنجاح."
            },
            [LocalizationKey.ChangePasswordFailed] = new()
            {
                ["en"] = "Failed to change the password.",
                ["ar"] = "فشل في تغيير كلمة المرور."
            },
            [LocalizationKey.ChangePassFailed] = new()
            {
                ["en"] = "Failed to reset the password.",
                ["ar"] = "فشل في إعادة تعيين كلمة المرور."
            },
            [LocalizationKey.PassFailed] = new()
            {
                ["en"] = "Password is incorrect.",
                ["ar"] = "كلمة المرور غير صحيحة."
            },
            [LocalizationKey.PassMismatch] = new()
            {
                ["en"] = "New password and confirm password do not match.",
                ["ar"] = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين."
            },
            [LocalizationKey.IncorrectOldPass] = new()
            {
                ["en"] = "Old password is incorrect.",
                ["ar"] = "كلمة المرور القديمة غير صحيحة."
            },
            [LocalizationKey.IncorrectPassword] = new()
            {
                ["en"] = "The email or password is incorrect.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            [LocalizationKey.Newpasswordthesameastheoldpassword] = new()
            {
                ["en"] = "The new password cannot be the same as the old password.",
                ["ar"] = "لا يمكن أن تكون كلمة المرور الجديدة هي نفسها كلمة المرور القديمة."
            },
            #endregion

            #region Email & Verification
            [LocalizationKey.EmailNotFound] = new()
            {
                ["en"] = "The email or password is incorrect.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            [LocalizationKey.EmailExists] = new()
            {
                ["en"] = "Email already exists.",
                ["ar"] = "البريد الإلكتروني موجود بالفعل."
            },
            [LocalizationKey.verficationEmailSent] = new()
            {
                ["en"] = "Verification code sent successfully.",
                ["ar"] = "تم إرسال رمز التحقق بنجاح."
            },
            [LocalizationKey.verficationEmailFailed] = new()
            {
                ["en"] = "Failed to send verification code.",
                ["ar"] = "فشل في إرسال رمز التحقق."
            },
            [LocalizationKey.EmailVerificationFailed] = new()
            {
                ["en"] = "Email verification failed.",
                ["ar"] = "فشل التحقق من البريد الإلكتروني."
            },
            [LocalizationKey.EmailVerified] = new()
            {
                ["en"] = "Email verified successfully.",
                ["ar"] = "تم التحقق من البريد الإلكتروني بنجاح."
            },
            [LocalizationKey.InvalidOrExpiredCode] = new()
            {
                ["en"] = "Invalid or expired verification code.",
                ["ar"] = "رمز التحقق غير صالح أو منتهي."
            },
            #endregion

            #region Token
            [LocalizationKey.ExpiredRefreshToken] = new()
            {
                ["en"] = "Refresh token has expired.",
                ["ar"] = "انتهت صلاحية رمز التحديث."
            },
            [LocalizationKey.InvalidRefreshToken] = new()
            {
                ["en"] = "Invalid or expired refresh token.",
                ["ar"] = "رمز التحديث غير صالح أو منتهي."
            },
            [LocalizationKey.InvalidToken] = new()
            {
                ["en"] = "Invalid refresh token.",
                ["ar"] = "رمز التحديث غير صالح."
            },
            [LocalizationKey.TokenRefreshSuccess] = new()
            {
                ["en"] = "Token refreshed successfully.",
                ["ar"] = "تم تحديث الرمز بنجاح."
            },
            [LocalizationKey.RefreshTokenMissing] = new()
            {
                ["en"] = "Refresh token is missing.",
                ["ar"] = "رمز التحديث مفقود."
            },
            #endregion

            #region Roles
            [LocalizationKey.RoleNameRequired] = new()
            {
                ["en"] = "Role name is required.",
                ["ar"] = "اسم الدور مطلوب."
            },
            [LocalizationKey.RoleAddFailed] = new()
            {
                ["en"] = "Failed to add the role.",
                ["ar"] = "فشل في إضافة الدور."
            },
            [LocalizationKey.RoleAddedSuccessfully] = new()
            {
                ["en"] = "Role added successfully.",
                ["ar"] = "تمت إضافة الدور بنجاح."
            },
            [LocalizationKey.UserIdRequired] = new()
            {
                ["en"] = "User ID is required.",
                ["ar"] = "معرف المستخدم مطلوب."
            },
            [LocalizationKey.NoRolesFoundForUser] = new()
            {
                ["en"] = "No roles found for this user.",
                ["ar"] = "لم يتم العثور على أدوار لهذا المستخدم."
            },
            [LocalizationKey.UserRolesRetrieved] = new()
            {
                ["en"] = "User roles retrieved successfully.",
                ["ar"] = "تم جلب أدوار المستخدم بنجاح."
            },
            [LocalizationKey.NoRolesFound] = new()
            {
                ["en"] = "No roles found.",
                ["ar"] = "لم يتم العثور على أدوار."
            },
            [LocalizationKey.RoleNotFound] = new()
            {
                ["en"] = "Role not found.",
                ["ar"] = "لم يتم العثور على أدوار."
            },
            [LocalizationKey.AllRolesRetrieved] = new()
            {
                ["en"] = "All roles retrieved successfully.",
                ["ar"] = "تم جلب جميع الأدوار بنجاح."
            },
            [LocalizationKey.RoleIdRequired] = new()
            {
                ["en"] = "Role ID is required.",
                ["ar"] = "معرف الدور مطلوب."
            },
            [LocalizationKey.RoleDeletionFailed] = new()
            {
                ["en"] = "Failed to delete the role.",
                ["ar"] = "فشل في حذف الدور."
            },
            [LocalizationKey.RoleDeletedSuccessfully] = new()
            {
                ["en"] = "Role deleted successfully.",
                ["ar"] = "تم حذف الدور بنجاح."
            },
            [LocalizationKey.RoleAlreadyExists] = new()
            {
                ["en"] = "Role already exists.",
                ["ar"] = "الدور موجود بالفعل."
            },
            #endregion

            #region Profile
            [LocalizationKey.GetProfileSuccess] = new()
            {
                ["en"] = "Profile fetched successfully",
                ["ar"] = "تم جلب الملف الشخصي بنجاح"
            },
            [LocalizationKey.UpdateProfileSuccess] = new()
            {
                ["en"] = "Profile updated successfully.",
                ["ar"] = "تم تحديث الملف الشخصي بنجاح."
            },
            [LocalizationKey.CompleteDataSuccess] = new()
            {
                ["en"] = "Data completed successfully",
                ["ar"] = "تم استكمال البيانات بنجاح"
            },
            [LocalizationKey.FetchMembersSuccess] = new()
            {
                ["en"] = "Members fetched successfully.",
                ["ar"] = "تم جلب الأعضاء بنجاح."
            },
            #endregion

            #region Doctor
            [LocalizationKey.SpecializationNotFound] = new()
            {
                ["en"] = "Specialization not found.",
                ["ar"] = "التخصص غير موجود."
            },
            [LocalizationKey.FetchDoctorSpecializationsSuccess] = new()
            {
                ["en"] = "Doctor specializations fetched successfully",
                ["ar"] = "تم جلب تخصصات الأطباء بنجاح"
            },
            [LocalizationKey.FetchDoctorProfileSuccess] = new()
            {
                ["en"] = "Doctor profile fetched successfully.",
                ["ar"] = "تم جلب ملف الطبيب بنجاح."
            },
            [LocalizationKey.FetchAllDoctorsSuccess] = new()
            {
                ["en"] = "All doctors fetched successfully.",
                ["ar"] = "تم جلب جميع الأطباء بنجاح."
            },
            [LocalizationKey.FetchDoctorsBySpecialistSuccess] = new()
            {
                ["en"] = "Doctors fetched by specialist successfully.",
                ["ar"] = "تم جلب الأطباء حسب التخصص بنجاح."
            },
            [LocalizationKey.DoctorNotFound] = new()
            {
                ["en"] = "Doctor not found.",
                ["ar"] = "لم يتم العثور على الطبيب."
            },
            [LocalizationKey.FetchDoctorChartSuccess] = new()
            {
                ["en"] = "Doctor chart fetched successfully.",
                ["ar"] = "تم جلب مخطط الطبيب بنجاح."
            },
            [LocalizationKey.UpdateDoctorProfileSuccess] = new()
            {
                ["en"] = "Doctor profile updated successfully.",
                ["ar"] = "تم تحديث ملف الطبيب بنجاح."
            },
            [LocalizationKey.FetchDoctorDataSuccess] = new()
            {
                ["en"] = "Doctor data fetched successfully.",
                ["ar"] = "تم جلب بيانات الطبيب بنجاح."
            },
            [LocalizationKey.FetchAllBookingOfDoctorsSuccess] = new()
            {
                ["en"] = "All doctor bookings fetched successfully.",
                ["ar"] = "تم جلب جميع حجوزات الطبيب بنجاح."
            },
            #endregion

            #region Service
            [LocalizationKey.ServiceProviderIdRequired] = new()
            {
                ["en"] = "Service provider ID is required.",
                ["ar"] = "معرف مزود الخدمة مطلوب."
            },
            [LocalizationKey.ServiceIdRequired] = new()
            {
                ["en"] = "Service ID is required.",
                ["ar"] = "معرف الخدمة مطلوب."
            },
            [LocalizationKey.ServiceAddedSuccessfully] = new()
            {
                ["en"] = "Service added successfully.",
                ["ar"] = "تمت إضافة الخدمة بنجاح."
            },
            [LocalizationKey.ServiceNotFound] = new()
            {
                ["en"] = "Service not found.",
                ["ar"] = "الخدمة غير موجودة."
            },
            [LocalizationKey.FetchServicesSuccess] = new()
            {
                ["en"] = "Services fetched successfully.",
                ["ar"] = "تم جلب الخدمات بنجاح."
            },
            [LocalizationKey.ServiceUpdatedSuccessfully] = new()
            {
                ["en"] = "Service updated successfully.",
                ["ar"] = "تم تحديث الخدمة بنجاح."
            },
            [LocalizationKey.ServiceDeletedSuccessfully] = new()
            {
                ["en"] = "Service deleted successfully.",
                ["ar"] = "تم حذف الخدمة بنجاح."
            },
            [LocalizationKey.ServiceAlreadyBooked] = new()
            {
                ["en"] = "Service is already booked.",
                ["ar"] = "الخدمة محجوزة بالفعل."
            },
            [LocalizationKey.ServiceProviderNotFound] = new()
            {
                ["en"] = "Service provider not found.",
                ["ar"] = "مزود الخدمة غير موجود."
            },
            [LocalizationKey.ServiceDayNotFound] = new()
            {
                ["en"] = "Service day not found.",
                ["ar"] = "لم يتم العثور على يوم الخدمة."
            },
            [LocalizationKey.CannotUpdateServiceWithExistingBookings] = new()
            {
                ["en"] = "This service cannot be updated because it has existing bookings.",
                ["ar"] = "لا يمكن تعديل هذه الخدمة لوجود حجوزات مرتبطة بها."
            },
            [LocalizationKey.CannotDeleteServiceWithExistingBookings] = new()
            {
                ["en"] = "This service cannot be deleted as it has active bookings.",
                ["ar"] = "لا يمكن حذف هذه الخدمة لوجود حجوزات نشطة."
            },
            [LocalizationKey.ServiceHasBookings] = new()
            {
                ["en"] = "This service cannot be deleted or updated because it has existing bookings.",
                ["ar"] = "لا يمكن حذف او تعديل هذه الخدمة لوجود حجوزات مرتبطة بها."
            },
            [LocalizationKey.ServiceDeletedfromserviceprovider] = new()
            {
                ["en"] = "The service has been deleted from the service provider.",
                ["ar"] = "تم حذف الخدمة من مزود الخدمة."
            },
            [LocalizationKey.servicehandlernotfound] = new()
            {
                ["en"] = "Service handler not found.",
                ["ar"] = "لم يتم العثور على معالج الخدمة."
            },
            [LocalizationKey.InvalidServiceProviderType] = new()
            {
                ["en"] = "Invalid Service Provider Type.",
                ["ar"] = "نوع مزود الخدمة غير صالح."
            },
            #endregion

            #region Booking
            [LocalizationKey.BookingSuccess] = new()
            {
                ["en"] = "Service booked successfully.",
                ["ar"] = "تم حجز الخدمة بنجاح."
            },
            [LocalizationKey.ServiceBookedSuccessfully] = new()
            {
                ["en"] = "Service booked successfully.",
                ["ar"] = "تم حجز الخدمة بنجاح."
            },
            [LocalizationKey.BookingRetrievedsuccess] = new()
            {
                ["en"] = "Booking details retrieved successfully.",
                ["ar"] = "تم جلب تفاصيل الحجز بنجاح."
            },
            [LocalizationKey.TimeSlotNotFound] = new()
            {
                ["en"] = "The selected time slot was not found.",
                ["ar"] = "لم يتم العثور على الوقت المحدد."
            },
            [LocalizationKey.BookingNotFound] = new()
            {
                ["en"] = "Booking not found.",
                ["ar"] = "لم يتم العثور على الحجز."
            },
            [LocalizationKey.BookingExpired] = new()
            {
                ["en"] = "Booking has expired.",
                ["ar"] = "انتهت صلاحية الحجز."
            },
            [LocalizationKey.BookingCancelled] = new()
            {
                ["en"] = "Booking has been cancelled.",
                ["ar"] = "تم إلغاء الحجز."
            },
            [LocalizationKey.BookingConfirmedSuccessfully] = new()
            {
                ["en"] = "Booking confirmed successfully.",
                ["ar"] = "تم تأكيد الحجز بنجاح."
            },
            [LocalizationKey.InvalidBookingStatus] = new()
            {
                ["en"] = "Invalid booking status.",
                ["ar"] = "حالة الحجز غير صالحة."
            },
            [LocalizationKey.BookingStatusUpdatedSuccessfully] = new()
            {
                ["en"] = "Booking status updated successfully.",
                ["ar"] = "تم تحديث حالة الحجز بنجاح."
            },
            [LocalizationKey.BookingStatusIsAlreadyCompleted] = new()
            {
                ["en"] = "The booking is already completed and cannot be updated.",
                ["ar"] = "هذا الحجز مكتمل بالفعل ولا يمكن تغيير حالته."
            },
            [LocalizationKey.BookingUpdatedSuccessfully] = new()
            {
                ["en"] = "Booking updated successfully.",
                ["ar"] = "تم تحديث الحجز بنجاح."
            },
            [LocalizationKey.InvalidBookingUpdateDetails] = new()
            {
                ["en"] = "Invalid booking update details.",
                ["ar"] = "بيانات تحديث الحجز غير صحيحة."
            },
            [LocalizationKey.BookingStatusUpdaterIterationFailed] = new()
            {
                ["en"] = "An error occurred while processing bookings.",
                ["ar"] = "حدث خطأ أثناء معالجة الحجوزات."
            },
            [LocalizationKey.BookingAddedSuccessfully] = new()
            {
                ["en"] = "Booking added successfully.",
                ["ar"] = "تمت إضافة الحجز بنجاح."
            },
            [LocalizationKey.BookingCancelledSuccessfully] = new()
            {
                ["en"] = "Booking cancelled successfully.",
                ["ar"] = "تم إلغاء الحجز بنجاح."
            },
            [LocalizationKey.BookingsRetrievedSuccessfully] = new()
            {
                ["en"] = "Bookings retrieved successfully.",
                ["ar"] = "تم جلب الحجوزات بنجاح."
            },
            [LocalizationKey.UserHasAnotherBookingWithSameProviderOnThisDate] = new()
            {
                ["en"] = "User has another booking with the same provider on this date.",
                ["ar"] = "للمستخدم حجز آخر مع نفس مقدم الخدمة في هذا التاريخ."
            },
            [LocalizationKey.CollectedCountBookingsSuccess] = new()
            {
                ["en"] = "Bookings count retrieved successfully.",
                ["ar"] = "تم جلب عدد الحجوزات بنجاح."
            },
            #endregion

            #region QR Code
            [LocalizationKey.QrAlreadyUsed] = new()
            {
                ["en"] = "QR code has already been used.",
                ["ar"] = "تم استخدام رمز الاستجابة السريعة من قبل."
            },
            [LocalizationKey.QrCodeValid] = new()
            {
                ["en"] = "QR code is valid.",
                ["ar"] = "رمز QR صالح."
            },
            [LocalizationKey.QrCodeInvalid] = new()
            {
                ["en"] = "QR code is invalid.",
                ["ar"] = "رمز QR غير صالح."
            },
            [LocalizationKey.InvalidQr] = new()
            {
                ["en"] = "Invalid QR code.",
                ["ar"] = "رمز QR غير صالح."
            },
            #endregion

            #region Reviews
            [LocalizationKey.GetReviewsSuccess] = new()
            {
                ["en"] = "Reviews fetched successfully.",
                ["ar"] = "تم جلب التقييمات بنجاح."
            },
            [LocalizationKey.ReviewNotFound] = new()
            {
                ["en"] = "Review not found.",
                ["ar"] = "التقييم غير موجود"
            },
            [LocalizationKey.ReviewDeletedSuccessfully] = new()
            {
                ["en"] = "Review deleted successfully.",
                ["ar"] = "تم حذف التقييم بنجاح"
            },
            [LocalizationKey.ReviewAddedSuccessfully] = new()
            {
                ["en"] = "Review added successfully.",
                ["ar"] = "تم اضافة التقييم بنجاح"
            },
            [LocalizationKey.ReviewUpdatedSuccessfully] = new()
            {
                ["en"] = "Review updated successfully.",
                ["ar"] = "تم تعديل التقييم بنجاح"
            },
            [LocalizationKey.CannotAddMoreThan3Reviews] = new()
            {
                ["en"] = "You cannot add more than three reviews for the same service provider.",
                ["ar"] = "لا يمكن إضافة أكثر من 3 تقييمات لنفس مزود الخدمة."
            },
            [LocalizationKey.ReviewContainsToxicContent] = new()
            {
                ["en"] = "The review contains toxic content.",
                ["ar"] = "التقييم يحتوي على محتوى غير مناسب."
            },
            [LocalizationKey.ToxicityPredictionSuccess] = new()
            {
                ["en"] = "Toxicity prediction completed successfully.",
                ["ar"] = "تم إكمال التنبؤ بالسلبية بنجاح."
            },
            #endregion

            #region Favourites
            [LocalizationKey.FavouriteNotFound] = new()
            {
                ["en"] = "Favourite not found.",
                ["ar"] = "المفضل غير موجود."
            },
            [LocalizationKey.FavouriteAddedSuccessfully] = new()
            {
                ["en"] = "Favourite added successfully.",
                ["ar"] = "تمت إضافة المفضل بنجاح."
            },
            [LocalizationKey.FavouriteRemovedSuccessfully] = new()
            {
                ["en"] = "Favourite removed successfully.",
                ["ar"] = "تمت إزالة المفضل بنجاح."
            },
            [LocalizationKey.FavouritesRetrievedSuccessfully] = new()
            {
                ["en"] = "Favourites retrieved successfully.",
                ["ar"] = "تم جلب المفضلات بنجاح."
            },
            #endregion

            #region Gym
            [LocalizationKey.GymNotFound] = new()
            {
                ["en"] = "The requested gym was not found.",
                ["ar"] = "الجيم المطلوب غير موجود."
            },
            [LocalizationKey.AllGymsData] = new()
            {
                ["en"] = "All gyms data fetched successfully.",
                ["ar"] = "تم جلب بيانات جميع الجيمات بنجاح."
            },
            [LocalizationKey.GymProfileData] = new()
            {
                ["en"] = "Gym profile data fetched successfully.",
                ["ar"] = "تم جلب بيانات ملف الجيم بنجاح."
            },
            [LocalizationKey.Gymnotfound] = new()
            {
                ["en"] = "The requested gym was not found.",
                ["ar"] = "الجيم المطلوب غير موجود."
            },
            #endregion

            #region Package
            [LocalizationKey.PackageNotFound] = new()
            {
                ["en"] = "The requested package was not found.",
                ["ar"] = "الباقة المطلوبة غير موجودة."
            },
            [LocalizationKey.PackageAddedSuccessfully] = new()
            {
                ["en"] = "Package added successfully.",
                ["ar"] = "تمت إضافة الباقة بنجاح."
            },
            [LocalizationKey.PackageUpdatedSuccessfully] = new()
            {
                ["en"] = "Package updated successfully.",
                ["ar"] = "تم تحديث الباقة بنجاح."
            },
            [LocalizationKey.PackageDeletedSuccessfully] = new()
            {
                ["en"] = "Package deleted successfully.",
                ["ar"] = "تم حذف الباقة بنجاح."
            },
            [LocalizationKey.PackagesRetrievedSuccessfully] = new()
            {
                ["en"] = "Packages retrieved successfully.",
                ["ar"] = "تم جلب الباقات بنجاح."
            },
            [LocalizationKey.PackageAlreadyBooked] = new()
            {
                ["en"] = "This package is already booked.",
                ["ar"] = "هذه الباقة محجوزة بالفعل."
            },
            #endregion

            #region Payment
            [LocalizationKey.AmountMustBeGreaterThanZero] = new()
            {
                ["en"] = "Amount must be greater than zero.",
                ["ar"] = "يجب أن يكون المبلغ أكبر من صفر."
            },
            [LocalizationKey.PaymobApiFailed] = new()
            {
                ["en"] = "Failed to communicate with Paymob API.",
                ["ar"] = "فشل في التواصل مع واجهة برمجة تطبيقات Paymob."
            },
            [LocalizationKey.PaymentInitializedSuccessfully] = new()
            {
                ["en"] = "Payment initialized successfully.",
                ["ar"] = "تم تهيئة الدفع بنجاح."
            },
            [LocalizationKey.PaymentInitializationFailed] = new()
            {
                ["en"] = "Failed to initialize payment.",
                ["ar"] = "فشل في تهيئة الدفع."
            },
            [LocalizationKey.Invalidwebhooksignature] = new()
            {
                ["en"] = "Invalid webhook signature.",
                ["ar"] = "توقيع الويب هوك غير صالح."
            },
            [LocalizationKey.PaymentProcessedSuccessfully] = new()
            {
                ["en"] = "Payment processed successfully.",
                ["ar"] = "تمت معالجة الدفع بنجاح."
            },
            [LocalizationKey.PaymentProcessingFailed] = new()
            {
                ["en"] = "Failed to process payment.",
                ["ar"] = "فشل في معالجة الدفع."
            },
            [LocalizationKey.paymobApiFailed] = new()
            {
                ["en"] = "Failed to communicate with Paymob API.",
                ["ar"] = "فشل في التواصل مع واجهة برمجة تطبيقات Paymob."
            },
            [LocalizationKey.InvalidPaymentMethod] = new()
            {
                ["en"] = "Invalid payment method.",
                ["ar"] = "طريقة الدفع غير صالحة."
            },
            [LocalizationKey.PaymentMethodNotFound] = new()
            {
                ["en"] = "Payment method not found.",
                ["ar"] = "طريقة الدفع غير موجودة."
            },
            #endregion

            #region Resident
            [LocalizationKey.ResidentNotFound] = new()
            {
                ["en"] = "Resident not found.",
                ["ar"] = "المقيم غير موجود."
            },
            [LocalizationKey.ResidentIdRequired] = new()
            {
                ["en"] = "Resident ID is required.",
                ["ar"] = "معرف المقيم مطلوب."
            },
            [LocalizationKey.InvalidNationalId] = new()
            {
                ["en"] = "The national ID provided is invalid.",
                ["ar"] = "رقم الهوية الوطنية المقدم غير صالح."
            },
            [LocalizationKey.NoUnitFound] = new()
            {
                ["en"] = "You don't have a unit here.",
                ["ar"] = "ليس لديك وحدة هنا."
            },
            [LocalizationKey.CompleteResidentRegisterSuccess] = new()
            {
                ["en"] = "Resident registration completed successfully",
                ["ar"] = "تم إكمال تسجيل المقيم بنجاح"
            },
            [LocalizationKey.GetResidentChartSuccess] = new()
            {
                ["en"] = "Resident chart fetched successfully.",
                ["ar"] = "تم جلب مخطط المقيم بنجاح."
            },
            [LocalizationKey.Residentnotfound] = new()
            {
                ["en"] = "The requested resident was not found.",
                ["ar"] = "المقيم المطلوب غير موجود."
            },
            #endregion

            #region Files
            [LocalizationKey.FileIsRequired] = new()
            {
                ["en"] = "File is required.",
                ["ar"] = "الملف مطلوب."
            },
            [LocalizationKey.FileSizeExceeded] = new()
            {
                ["en"] = "File size exceeded the maximum limit of 5 MB.",
                ["ar"] = "تجاوز حجم الملف الحد الأقصى المسموح به وهو 5 ميجابايت."
            },
            [LocalizationKey.InvalidFileType] = new()
            {
                ["en"] = "Invalid file type. Allowed types are: .jpg, .jpeg, .png, .docx, .pdf.",
                ["ar"] = "نوع الملف غير صالح. الأنواع المسموح بها هي: .jpg، .jpeg، .png، .docx، .pdf."
            },
            [LocalizationKey.InvalidFileContentType] = new()
            {
                ["en"] = "Invalid file content type.",
                ["ar"] = "نوع محتوى الملف غير صالح."
            },
            #endregion

            #region Contact & Charts
            [LocalizationKey.SuccessToAddContact] = new()
            {
                ["en"] = "Contact message sent successfully.",
                ["ar"] = "تم إرسال رسالة التواصل بنجاح."
            },
            [LocalizationKey.SuccessToGetContacts] = new()
            {
                ["en"] = "Contacts retrieved successfully.",
                ["ar"] = "تم جلب رسائل التواصل بنجاح."
            },
            [LocalizationKey.FetchChartSuccess] = new()
            {
                ["en"] = "Chart fetched successfully.",
                ["ar"] = "تم جلب مخطط بنجاح."
            },
            #endregion

            #region General
            [LocalizationKey.ServerError] = new()
            {
                ["en"] = "An unexpected error occurred. Please try again later.",
                ["ar"] = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا."
            },
            [LocalizationKey.TooManyRequests] = new()
            {
                ["en"] = "Too many requests. Please try again later.",
                ["ar"] = "طلبات كثيرة جدا. يرجى المحاولة لاحقًا."
            },
            [LocalizationKey.TimeZoneNotConfigured] = new()
            {
                ["en"] = "Default time zone is not configured.",
                ["ar"] = "لم يتم إعداد المنطقة الزمنية الافتراضية."
            },
            #endregion

            #region FireBase
            [LocalizationKey.UserSubscriptionSuccess] = new()
            {
                ["en"] = "User subscribed to notifications successfully.",
                ["ar"] = "تم اشتراك المستخدم في الإشعارات بنجاح."
            },
            [LocalizationKey.UserUnsubscriptionSuccess] = new()
            {
                ["en"] = "User unsubscribed from notifications successfully.",
                ["ar"] = "تم إلغاء اشتراك المستخدم في الإشعارات بنجاح."
            },
            [LocalizationKey.NotificationSentToDeviceSuccess] = new()
            {
                ["en"] = "Notification sent to device successfully.",
                ["ar"] = "تم إرسال الإشعار إلى الجهاز بنجاح."
            },
            [LocalizationKey.NotificationSentToTopicSuccess] = new()
            {
                ["en"] = "Notification sent to topic successfully.",
                ["ar"] = "تم إرسال الإشعار إلى الموضوع بنجاح."
            },
            #endregion

            #region Notifications
            [LocalizationKey.NotificationNotFound] = new()
            {
                ["en"] = "Notification not found.",
                ["ar"] = "الإشعار غير موجود."
            },
            [LocalizationKey.NotificationMarkedAsSeen] = new()
            {
                ["en"] = "Notification marked as seen successfully.",
                ["ar"] = "تم وضع علامة تم رؤيته على الإشعار بنجاح."
            },
            [LocalizationKey.NotificationsFetched] = new()
            {
                ["en"] = "Notifications fetched successfully.",
                ["ar"] = "تم جلب الإشعارات بنجاح."
            },
            [LocalizationKey.AllNotificationsMarkedAsSeen] = new()
            {
                ["en"] = "All notifications marked as seen successfully.",
                ["ar"] = "تم وضع علامة تم رؤيته على جميع الإشعارات بنجاح."
            },
            [LocalizationKey.NotificationDeleted] = new()
            {
                ["en"] = "Notification deleted successfully.",
                ["ar"] = "تم حذف الإشعار بنجاح."
            },
            [LocalizationKey.NotificationAdded] = new()
            {
                ["en"] = "Notification added successfully.",
                ["ar"] = "تم إضافة الإشعار بنجاح."
            },
            #endregion

            #region UserEvents

            [LocalizationKey.SuccessToCreateUserEvent] = new()
            {
                ["en"] = "User event created successfully.",
                ["ar"] = "تم تسجيل نشاط المستخدم بنجاح."
            },

            [LocalizationKey.FailedToCreateUserEvent] = new()
            {
                ["en"] = "Failed to create user event.",
                ["ar"] = "فشل في تسجيل نشاط المستخدم."
            },

            [LocalizationKey.SuccessToGetUserEvents] = new()
            {
                ["en"] = "User events retrieved successfully.",
                ["ar"] = "تم جلب أنشطة المستخدم بنجاح."
            },

            [LocalizationKey.NoUserEventsFound] = new()
            {
                ["en"] = "No user events found.",
                ["ar"] = "لا توجد أنشطة مسجلة للمستخدم."
            },

            [LocalizationKey.SuccessToGetTopServiceProviders] = new()
            {
                ["en"] = "Top service providers retrieved successfully.",
                ["ar"] = "تم جلب أكثر مقدمي الخدمات تفاعلاً بنجاح."
            },

            [LocalizationKey.SuccessToGetUserDashboard] = new()
            {
                ["en"] = "User dashboard data retrieved successfully.",
                ["ar"] = "تم جلب بيانات لوحة المستخدم بنجاح."
            },

            [LocalizationKey.SuccessToGetAdminDashboard] = new()
            {
                ["en"] = "Admin dashboard data retrieved successfully.",
                ["ar"] = "تم جلب بيانات لوحة التحكم بنجاح."
            },

            [LocalizationKey.FailedToGetDashboardData] = new()
            {
                ["en"] = "Failed to retrieve dashboard data.",
                ["ar"] = "فشل في جلب بيانات لوحة التحكم."
            },

            [LocalizationKey.SuccessToGetMostUsedServices] = new()
            {
                ["en"] = "Most used services retrieved successfully.",
                ["ar"] = "تم جلب أكثر الخدمات استخدامًا بنجاح."
            },

            [LocalizationKey.SuccessToGetConversionRates] = new()
            {
                ["en"] = "Service conversion rates retrieved successfully.",
                ["ar"] = "تم جلب نسب التحويل للخدمات بنجاح."
            },

            [LocalizationKey.SuccessToGetMostActiveUsers] = new()
            {
                ["en"] = "Most active users retrieved successfully.",
                ["ar"] = "تم جلب أكثر المستخدمين نشاطًا بنجاح."
            },
            #endregion

            #region Social
            [LocalizationKey.PostNotFound] = new()
            {
                ["en"] = "Post not found.",
                ["ar"] = "المنشور غير موجود."
            },
            [LocalizationKey.SuccessToMarkAsRead] = new()
            {
                ["en"] = "Marked as read successfully.",
                ["ar"] = "تم تحديد الرسائل كمقروءة بنجاح."
            },
            [LocalizationKey.SuccessToCheckReaction] = new()
            {
                ["en"] = "Reaction checked successfully.",
                ["ar"] = "تم التحقق من التفاعل بنجاح."
            },
            [LocalizationKey.SuccessToCreatePost] = new()
            {
                ["en"] = "Post created successfully.",
                ["ar"] = "تم إنشاء المنشور بنجاح."
            },
            [LocalizationKey.SuccessToUpdatePost] = new()
            {
                ["en"] = "Post updated successfully.",
                ["ar"] = "تم تحديث المنشور بنجاح."
            },

            [LocalizationKey.FailedToCreatePost] = new()
            {
                ["en"] = "Failed to create post.",
                ["ar"] = "فشل في إنشاء المنشور."
            },

            [LocalizationKey.SuccessToGetPosts] = new()
            {
                ["en"] = "Posts retrieved successfully.",
                ["ar"] = "تم جلب المنشورات بنجاح."
            },

            [LocalizationKey.NoPostsFound] = new()
            {
                ["en"] = "No posts found.",
                ["ar"] = "لا توجد منشورات."
            },

            [LocalizationKey.SuccessToDeletePost] = new()
            {
                ["en"] = "Post deleted successfully.",
                ["ar"] = "تم حذف المنشور بنجاح."
            },

            [LocalizationKey.FailedToDeletePost] = new()
            {
                ["en"] = "Failed to delete post.",
                ["ar"] = "فشل في حذف المنشور."
            },


            [LocalizationKey.SuccessToCreateComment] = new()
            {
                ["en"] = "Comment added successfully.",
                ["ar"] = "تمت إضافة التعليق بنجاح."
            },

            [LocalizationKey.FailedToCreateComment] = new()
            {
                ["en"] = "Failed to add comment.",
                ["ar"] = "فشل في إضافة التعليق."
            },

            [LocalizationKey.SuccessToGetComments] = new()
            {
                ["en"] = "Comments retrieved successfully.",
                ["ar"] = "تم جلب التعليقات بنجاح."
            },

            [LocalizationKey.NoCommentsFound] = new()
            {
                ["en"] = "No comments found.",
                ["ar"] = "لا توجد تعليقات."
            },

            [LocalizationKey.SuccessToDeleteComment] = new()
            {
                ["en"] = "Comment deleted successfully.",
                ["ar"] = "تم حذف التعليق بنجاح."
            },

            [LocalizationKey.FailedToDeleteComment] = new()
            {
                ["en"] = "Failed to delete comment.",
                ["ar"] = "فشل في حذف التعليق."
            },
            [LocalizationKey.SuccessToUpdateComment] = new()
            {
                ["en"] = "Comment updated successfully.",
                ["ar"] = "تم تعديل التعليق بنجاح."
            },
            [LocalizationKey.SuccessToToggleReaction] = new()
            {
                ["en"] = "Reaction toggled successfully.",
                ["ar"] = "تم تحديث التفاعل بنجاح."
            },
            [LocalizationKey.SuccessToGetInformationProfile] = new()
            {
                ["en"] = "Profile information retrieved successfully.",
                ["ar"] = "تم جلب معلومات الملف الشخصي بنجاح."
            },
            #endregion

            #region Driver

            [LocalizationKey.DriverNotFound] = new()
            {
                ["en"] = "Driver not found.",
                ["ar"] = "السائق غير موجود."
            },
            [LocalizationKey.VehicleNumberAlreadyExists] = new()
            {
                ["en"] = "Vehicle number already exists.",
                ["ar"] = "رقم السيارة موجود بالفعل."
            },
            
            [LocalizationKey.CarImagesAreRequired] = new()
            {
                ["en"] = "Car images are required.",
                ["ar"] = "صور السيارة مطلوبة."
            },
            [LocalizationKey.DriverFilesAreRequired] = new()
            {
                ["en"] = "Driver files are required.",
                ["ar"] = "ملفات السائق مطلوبة."
            },
            [LocalizationKey.DriverCompleteRegisterSuccess] = new()
            {
                ["en"] = "Driver registration completed successfully",
                ["ar"] = "تم إكمال تسجيل السائق بنجاح"
            },
            [LocalizationKey.GetDriverProfileSuccess] = new()
            {
                ["en"] = "Driver profile fetched successfully.",
                ["ar"] = "تم جلب ملف السائق بنجاح."
            },
            [LocalizationKey.ChangeDriverStatusSuccess]=new()
            {
                ["en"] = "Driver status changed successfully.",
                ["ar"] = "تم تغيير حالة السائق بنجاح."
            },
            [LocalizationKey.TrackingDriverSuccess]=new()
            {
                ["en"] = "Driver location tracked successfully.",
                ["ar"] = "تم تتبع موقع السائق بنجاح."
            },
            [LocalizationKey.GetDriverLocationSuccess]=new()
            {
                ["en"] = "Driver location retrieved successfully.",
                ["ar"] = "تم جلب موقع السائق بنجاح."
            },
            [LocalizationKey.DriverLocationNotFound]=new()
            {
                ["en"] = "Driver location not found.",
                ["ar"] = "موقع السائق غير موجود."
            },
            [LocalizationKey.GetTopNearestDriverSuccess]=new()
            {
                ["en"] = "Top nearest drivers retrieved successfully.",
                ["ar"] = "تم جلب أقرب السائقين بنجاح."
            },
            [LocalizationKey.VehicleTypeNotSupported]=new()
            {
                ["en"] = "Vehicle type not supported.",
                ["ar"] = "نوع السيارة غير مدعوم."
            },
            [LocalizationKey.EstimateRideSuccessfully]=new()
            {
                ["en"] = "Ride estimated successfully.",
                ["ar"] = "تم تقدير الرحلة بنجاح."
            },
            [LocalizationKey.RequestRideSuccessfully]=new()
            {
                ["en"] = "Ride requested successfully.",
                ["ar"] = "تم طلب الرحلة بنجاح."
            },
            [LocalizationKey.ResidentHasActiveRide]=new()
            {
                ["en"] = "Resident already has an active ride.",
                ["ar"] = "المقيم لديه رحلة نشطة بالفعل."
            },
            [LocalizationKey.RideNotFound]=new()
            {
                ["en"] = "Ride not found.",
                ["ar"] = "الرحلة غير موجودة."
            },
            [LocalizationKey.GetRideByIdSuccessfully]=new()
            {
                ["en"] = "Ride details retrieved successfully.",
                ["ar"] = "تم جلب تفاصيل الرحلة بنجاح."
            },
            [LocalizationKey.CannotCancelRide]=new()
            {
                ["en"] = "Cannot cancel the ride at this stage.",
                ["ar"] = "لا يمكن إلغاء الرحلة في هذه المرحلة."
            },
            [LocalizationKey.SomeOneHadAcceptIt]=new()
            {
                ["en"] = "Someone has already accepted the ride request.",
                ["ar"] = "لقد قام شخص ما بقبول طلب الرحلة بالفعل."
            },
            [LocalizationKey.InvalidRideStatus]=new()
            {
                ["en"] = "Invalid ride status.",
                ["ar"] = "حالة الرحلة غير صالحة."
            },
            [LocalizationKey.AcceptRideSuccessfully]=new()
            {
                ["en"] = "Ride accepted successfully.",
                ["ar"] = "تم قبول الرحلة بنجاح."
            },
            [LocalizationKey.CompleteRideSuccessfully]=new()
            {
                ["en"] = "Ride completed successfully.",
                ["ar"] = "تم إكمال الرحلة بنجاح."
            },
            [LocalizationKey.CancelRideSuccessfully]=new()
            {
                ["en"] = "Ride cancelled successfully.",
                ["ar"] = "تم إلغاء الرحلة بنجاح."
            },
            [LocalizationKey.StartRideSuccessfully]=new()
            {
                ["en"] = "Ride started successfully.",
                ["ar"] = "تم بدء الرحلة بنجاح."
            },
            [LocalizationKey.RideNotAcceptedYet]=new()
            {
                ["en"] = "Ride has not been accepted yet.",
                ["ar"] = "لم يتم قبول الرحلة بعد."
            },
            [LocalizationKey.GetUserRidesSuccessfully]=new()
            {
                ["en"] = "User rides retrieved successfully.",
                ["ar"] = "تم جلب رحلات المستخدم بنجاح."
            },
            [LocalizationKey.GetDriverRidesSuccessfully] = new()
            {
                ["en"] = "Driver rides retrieved successfully.",
                ["ar"] = "تم جلب رحلات السائق بنجاح."
            },
            [LocalizationKey.RideCompleted]=new()
            {
                ["en"] = "Ride has already been completed.",
                ["ar"] = "تم إكمال الرحلة بالفعل."
            },
            [LocalizationKey.GetDriverChartSuccessfully]=new()
            {
                ["en"] = "Driver chart fetched successfully.",
                ["ar"] = "تم جلب مخطط السائق بنجاح."
            },
            [LocalizationKey.UpdateDriverProfileSuccess]=new()
            {
                ["en"] = "Driver profile updated successfully.",
                ["ar"] = "تم تحديث ملف السائق بنجاح."
            },

            #endregion

            #region ChatAndUser

            [LocalizationKey.SuccessToGetUserProfile] = new()
            {
                ["en"] = "User profile retrieved successfully.",
                ["ar"] = "تم جلب ملف المستخدم بنجاح."
            },

            [LocalizationKey.SuccessToMarkAsRead] = new()
            {
                ["en"] = "Marked as read successfully.",
                ["ar"] = "تم تحديد الرسائل كمقروءة بنجاح."
            },

            [LocalizationKey.SuccessToGetChat] = new()
            {
                ["en"] = "Chat retrieved successfully.",
                ["ar"] = "تم جلب المحادثة بنجاح."
            },

            [LocalizationKey.SuccessToGetUsers] = new()
            {
                ["en"] = "Users retrieved successfully.",
                ["ar"] = "تم جلب المستخدمين بنجاح."
            },

            [LocalizationKey.MessageNotFoundOrNoPermission] = new()
            {
                ["en"] = "Message not found or you don't have permission to delete it.",
                ["ar"] = "الرسالة غير موجودة أو ليس لديك صلاحية لحذفها."
            },

            [LocalizationKey.ChatNotFoundOrNoPermission] = new()
            {
                ["en"] = "Chat not found or you don't have permission to access it.",
                ["ar"] = "المحادثة غير موجودة أو ليس لديك صلاحية للوصول إليها."
            },

            [LocalizationKey.NoUsersFound] = new()
            {
                ["en"] = "No users found.",
                ["ar"] = "لا يوجد مستخدمون."
            },

            [LocalizationKey.FailedToGetUsers] = new()
            {
                ["en"] = "Failed to retrieve users.",
                ["ar"] = "فشل في جلب المستخدمين."
            },

            [LocalizationKey.SuccessToGetChats] = new()
            {
                ["en"] = "Chats retrieved successfully.",
                ["ar"] = "تم جلب المحادثات بنجاح."
            },

            [LocalizationKey.NoChatsFound] = new()
            {
                ["en"] = "No chats found.",
                ["ar"] = "لا توجد محادثات."
            },

            [LocalizationKey.FailedToGetChats] = new()
            {
                ["en"] = "Failed to retrieve chats.",
                ["ar"] = "فشل في جلب المحادثات."
            },

            [LocalizationKey.SuccessToAddMessage] = new()
            {
                ["en"] = "Message sent successfully.",
                ["ar"] = "تم إرسال الرسالة بنجاح."
            },

            [LocalizationKey.FailedToAddMessage] = new()
            {
                ["en"] = "Failed to send message.",
                ["ar"] = "فشل في إرسال الرسالة."
            },

            [LocalizationKey.SuccessToUpdateMessage] = new()
            {
                ["en"] = "Message updated successfully.",
                ["ar"] = "تم تحديث الرسالة بنجاح."
            },

            [LocalizationKey.FailedToUpdateMessage] = new()
            {
                ["en"] = "Failed to update message.",
                ["ar"] = "فشل في تحديث الرسالة."
            },

            [LocalizationKey.SuccessToDeleteMessage] = new()
            {
                ["en"] = "Message deleted successfully.",
                ["ar"] = "تم حذف الرسالة بنجاح."
            },

            [LocalizationKey.FailedToDeleteMessage] = new()
            {
                ["en"] = "Failed to delete message.",
                ["ar"] = "فشل في حذف الرسالة."
            },

            [LocalizationKey.MessageNotFound] = new()
            {
                ["en"] = "Message not found.",
                ["ar"] = "الرسالة غير موجودة."
            },

            [LocalizationKey.SuccessToDeleteChat] = new()
            {
                ["en"] = "Chat deleted successfully.",
                ["ar"] = "تم حذف المحادثة بنجاح."
            },

            [LocalizationKey.FailedToDeleteChat] = new()
            {
                ["en"] = "Failed to delete chat.",
                ["ar"] = "فشل في حذف المحادثة."
            },

            [LocalizationKey.ChatNotFound] = new()
            {
                ["en"] = "Chat not found.",
                ["ar"] = "المحادثة غير موجودة."
            },

            [LocalizationKey.SuccessToUpdateBio] = new()
            {
                ["en"] = "Bio updated successfully.",
                ["ar"] = "تم تحديث النبذة الشخصية بنجاح."
            },

            [LocalizationKey.FailedToUpdateBio] = new()
            {
                ["en"] = "Failed to update bio.",
                ["ar"] = "فشل في تحديث النبذة الشخصية."
            },

            #endregion

            #region Technician

            [LocalizationKey.TechnicianNotFound] = new()
            {
                ["en"] = "Technician not found.",
                ["ar"] = "الفني غير موجود."
            },
            [LocalizationKey.TechnicianCompleteRegisterSuccessfully] = new()
            {
                ["en"] = "Technician registration completed successfully",
                ["ar"] = "تم إكمال تسجيل الفني بنجاح"
            },
            [LocalizationKey.TechnicianProfileRetrievedSuccessfully] = new()
            {
                ["en"] = "Technician profile fetched successfully.",
                ["ar"] = "تم جلب ملف الفني بنجاح."

            },
            [LocalizationKey.TechnicianProfileUpdatedSuccessfully] = new()
            {
                ["en"] = "Technician profile updated successfully.",
                ["ar"] = "تم تحديث ملف الفني بنجاح."
            },
            [LocalizationKey.DocumentsAreRequired] = new()
            {
                ["en"] = "Technician documents are required.",
                ["ar"] = "وثائق الفني مطلوبة."
            },
            [LocalizationKey.TechnicianSpecialtiesRetrievedSuccessfully]=new()
            {
                ["en"] = "Technician specialties retrieved successfully.",
                ["ar"] = "تم جلب تخصصات الفني بنجاح."
            },
            [LocalizationKey.TechniciansRetrievedSuccessfully]=new()
            {
                ["en"] = "Technicians retrieved successfully.",
                ["ar"] = "تم جلب الفنيين بنجاح."
            },
            #endregion
        };

        public static string GetLocalizedMessage(LocalizationKey key, string lan)
        {
            if (messages.ContainsKey(key) && messages[key].ContainsKey(lan))
                return messages[key][lan];
            return "An error occurred.";
        }
    }
}