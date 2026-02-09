namespace Wasla_Backend.Helpers.Localization
{
    public class LocalizationHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> messages = new()
        {
            ["InvalidRequest"] = new()
            {
                ["en"] = "Invalid Request Data",
                ["ar"] = "البيانات المطلوبة غير صالحة"
            },
            ["InvalidData"] = new()
            {
                ["en"] = "Invalid data provided.",
                ["ar"] = "تم تقديم بيانات غير صالحة."
            },
            ["UserNameAlreadyExists"] = new()
            {
                ["en"] = "Username is already taken.",
                ["ar"] = "اسم المستخدم مستخدم بالفعل."
            },
            ["EmailAlreadyExists"] = new()
            {
                ["en"] = "Email is already taken.",
                ["ar"] = "البريد الإلكتروني مستخدم بالفعل."
            },
            ["RegistrationSuccess"] = new()
            {
                ["en"] = "User registered successfully.",
                ["ar"] = "تم تسجيل المستخدم بنجاح."
            },
            ["RegistrationFailed"] = new()
            {
                ["en"] = "User registration failed.",
                ["ar"] = "فشل تسجيل المستخدم."
            },
            ["VerificationSuccess"] = new()
            {
                ["en"] = "User verification successful.",
                ["ar"] = "تم التحقق من المستخدم بنجاح."
            },
            ["VerificationFailed"] = new()
            {
                ["en"] = "Verification code is wrong.",
                ["ar"] = "رمز التحقق خاطئ."
            },
            ["OTPSend"] = new()
            {
                ["en"] = "The OTP code has been sent.",
                ["ar"] = "تم إرسال رمز التحقق."
            },
            ["ProfileEditSuccess"] = new()
            {
                ["en"] = "Profile updated successfully.",
                ["ar"] = "تم تحديث الملف الشخصي بنجاح."
            },
            ["ChangePassSuccess"] = new()
            {
                ["en"] = "Password changed successfully.",
                ["ar"] = "تم تغيير كلمة المرور بنجاح."
            },
            ["ChangePasswordFailed"] = new()
            {
                ["en"] = "Failed to change the password.",
                ["ar"] = "فشل في تغيير كلمة المرور."
            },
            ["ChangePassFailed"] = new()
            {
                ["en"] = "Failed to reset the password.",
                ["ar"] = "فشل في إعادة تعيين كلمة المرور."
            },
            ["PassFailed"] = new()
            {
                ["en"] = "Password is incorrect.",
                ["ar"] = "كلمة المرور غير صحيحة."
            },

            ["PassMismatch"] = new()
            {
                ["en"] = "New password and confirm password do not match.",
                ["ar"] = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين."
            },
            ["IncorrectOldPass"] = new()
            {
                ["en"] = "Old password is incorrect.",
                ["ar"] = "كلمة المرور القديمة غير صحيحة."
            },
            ["LoginSuccess"] = new()
            {
                ["en"] = "User logged in successfully.",
                ["ar"] = "تم تسجيل الدخول بنجاح."
            },
            ["LoginFailed"] = new()
            {
                ["en"] = "Email or password invalid.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            ["RoleNameRequired"] = new()
            {
                ["en"] = "Role name is required.",
                ["ar"] = "اسم الدور مطلوب."
            },
            ["RoleAddFailed"] = new()
            {
                ["en"] = "Failed to add the role.",
                ["ar"] = "فشل في إضافة الدور."
            },
            ["RoleAddedSuccessfully"] = new()
            {
                ["en"] = "Role added successfully.",
                ["ar"] = "تمت إضافة الدور بنجاح."
            },
            ["UserIdRequired"] = new()
            {
                ["en"] = "User ID is required.",
                ["ar"] = "معرف المستخدم مطلوب."
            },
            ["NoRolesFoundForUser"] = new()
            {
                ["en"] = "No roles found for this user.",
                ["ar"] = "لم يتم العثور على أدوار لهذا المستخدم."
            },
            ["UserRolesRetrieved"] = new()
            {
                ["en"] = "User roles retrieved successfully.",
                ["ar"] = "تم جلب أدوار المستخدم بنجاح."
            },
            ["NoRolesFound"] = new()
            {
                ["en"] = "No roles found.",
                ["ar"] = "لم يتم العثور على أدوار."
            },
            ["RoleNotFound"] = new()
            {
                ["en"] = "role no found.",
                ["ar"] = "لم يتم العثور على أدوار."
            },
            ["InvalidOrExpiredCode"] = new()
            {
                ["en"] = "Invalid or expired verification code.",
                ["ar"] = "رمز التحقق غير صالح أو منتهي."
            },
            ["UserNotVerified"] = new()
            {
                ["en"] = "User email is not verified.",
                ["ar"] = "لم يتم التحقق من بريد المستخدم."
            },
            ["UserNotApproved"] = new()
            {
                ["en"] = "User is not approved yet.",
                ["ar"] = "المستخدم لم يتم الموافقة عليه بعد."
            },
            ["IncorrectPassword"] = new()
            {
                ["en"] = "The email or password is incorrect.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            ["EmailNotFound"] = new()
            {
                ["en"] = "The email or password is incorrect.",
                ["ar"] = "البريد الإلكتروني أو كلمة المرور غير صحيحة."
            },
            ["EmailExists"] = new()
            {
                ["en"] = "Email already exists.",
                ["ar"] = "البريد الإلكتروني موجود بالفعل."
            },
            ["Unauthorized"] = new()
            {
                ["en"] = "Unauthorized access.",
                ["ar"] = "دخول غير مصرح به."
            },
            ["InvalidRefreshToken"] = new()
            {
                ["en"] = "Invalid or expired refresh token.",
                ["ar"] = "رمز التحديث غير صالح أو منتهي."
            },
            ["AllRolesRetrieved"] = new()
            {
                ["en"] = "All roles retrieved successfully.",
                ["ar"] = "تم جلب جميع الأدوار بنجاح."
            },
            ["verficationEmailSent"] = new()
            {
                ["en"] = "Verification code sent successfully.",
                ["ar"] = "تم إرسال رمز التحقق بنجاح."
            },
            ["verficationEmailFailed"] = new()
            {
                ["en"] = "Failed to send verification code.",
                ["ar"] = "فشل في إرسال رمز التحقق."
            },
            ["EmailVerificationFailed"] = new()
            {
                ["en"] = "Email verification failed.",
                ["ar"] = "فشل التحقق من البريد الإلكتروني."
            },
            ["EmailVerified"] = new()
            {
                ["en"] = "Email verified successfully.",
                ["ar"] = "تم التحقق من البريد الإلكتروني بنجاح."
            },
            ["UserNotFound"] = new()
            {
                ["en"] = "User not found.",
                ["ar"] = "لم يتم العثور على المستخدم."
            },
            ["InvalidToken"] = new()
            {
                ["en"] = "Invalid refresh token.",
                ["ar"] = "رمز التحديث غير صالح."
            },
            ["TokenRefreshSuccess"] = new()
            {
                ["en"] = "Token refreshed successfully.",
                ["ar"] = "تم تحديث الرمز بنجاح."
            },
            ["ServerError"] = new()
            {
                ["en"] = "An unexpected error occurred. Please try again later.",
                ["ar"] = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا."
            },
            ["CompleteDataSuccess"] = new()
            {
                ["en"] = "data completed successfully",
                ["ar"] = "تم استكمال البيانات بنجاح"
            },
            ["FetchDoctorSpecializationsSuccess"] = new()
            {
                ["en"] = "Doctor specializations fetched successfully",
                ["ar"] = "تم جلب تخصصات الأطباء بنجاح"
            },
            ["InvalidNationalId"] = new()
            {
                ["en"] = "The national ID provided is invalid.",
                ["ar"] = "رقم الهوية الوطنية المقدم غير صالح."
            },
            ["NoUnitFound"] = new()
            {
                ["en"] = "You don't have a unit here.",
                ["ar"] = "ليس لديك وحدة هنا."
            },
            ["CompleteResidentRegisterSuccess"] = new()
            {
                ["en"] = "Resident registration completed successfully",
                ["ar"] = "تم إكمال تسجيل المقيم بنجاح"
            },
            ["GetProfileSuccess"] = new()
            {
                ["en"] = "Profile fetched successfully",
                ["ar"] = "تم جلب الملف الشخصي بنجاح"
            },
            ["GetAllUsersSuccess"] = new()
            {
                ["en"] = "All users fetched successfully",
                ["ar"] = "تم جلب جميع المستخدمين بنجاح"
            },
            ["DeleteUserSuccess"] = new()
            {
                ["en"] = "User deleted successfully",
                ["ar"] = "تم حذف المستخدم بنجاح"
            },
            ["FetchDoctorProfileSuccess"] = new()
            {
                ["en"] = "Doctor profile fetched successfully.",
                ["ar"] = "تم جلب ملف الطبيب بنجاح."
            },
            ["FetchDoctorProfileSuccess"] = new()
            {
                ["en"] = "Doctor profile fetched successfully.",
                ["ar"] = "تم جلب ملف الطبيب بنجاح."
            },
            ["ServiceAddedSuccessfully"] = new()
            {
                ["en"] = "Service added successfully.",
                ["ar"] = "تمت إضافة الخدمة بنجاح."
            },
            ["ServiceNotFound"] = new()
            {
                ["en"] = "Service not found.",
                ["ar"] = "الخدمة غير موجودة."
            },
            ["FetchServicesSuccess"] = new()
            {
                ["en"] = "Services fetched successfully.",
                ["ar"] = "تم جلب الخدمات بنجاح."
            },
            ["ServiceUpdatedSuccessfully"] = new()
            {
                ["en"] = "Service updated successfully.",
                ["ar"] = "تم تحديث الخدمة بنجاح."
            },
            ["ServiceDeletedSuccessfully"] = new()
            {
                ["en"] = "Service deleted successfully.",
                ["ar"] = "تم حذف الخدمة بنجاح."
            },
            ["FetchAllDoctorsSuccess"] = new()
            {
                ["en"] = "All doctors fetched successfully.",
                ["ar"] = "تم جلب جميع الأطباء بنجاح."
            },
            ["FetchDoctorsBySpecialistSuccess"] = new()
            {
                ["en"] = "Doctors fetched by specialist successfully.",
                ["ar"] = "تم جلب الأطباء حسب التخصص بنجاح."
            },
            ["GetReviewsSuccess"] = new()
            {
                ["en"] = "Reviews fetched successfully.",
                ["ar"] = "تم جلب التقييمات بنجاح."
            },
            ["ServiceNotFound"] = new()
            {
                ["en"] = "Service not found.",
                ["ar"] = "الخدمة غير موجودة."
            },
            ["ServiceProviderNotFound"] = new()
            {
                ["en"] = "Service provider not found.",
                ["ar"] = "مزود الخدمة غير موجود."
            },
            ["ServiceAlreadyBooked"] = new()
            {
                ["en"] = "Service is already booked.",
                ["ar"] = "الخدمة محجوزة بالفعل."
            },
            ["BookingSuccess"] = new()
            {
                ["en"] = "Service booked successfully.",
                ["ar"] = "تم حجز الخدمة بنجاح."
            },
            ["ServiceBookedSuccessfully"] = new()
            {
                ["en"] = "Service booked successfully.",
                ["ar"] = "تم حجز الخدمة بنجاح."
            },
            ["BookingRetrievedsuccess"] = new()
            {
                ["en"] = "Booking details retrieved successfully.",
                ["ar"] = "تم جلب تفاصيل الحجز بنجاح."
            },
            ["TimeSlotNotFound"] = new()
            {
                ["en"] = "The selected time slot was not found.",
                ["ar"] = "لم يتم العثور على الوقت المحدد."
            },
            ["BookingNotFound"] = new()
            {
                ["en"] = "Booking not found.",
                ["ar"] = "لم يتم العثور على الحجز."
            },
            ["ServiceDayNotFound"] = new()
            {
                ["en"] = "Service day not found.",
                ["ar"] = "لم يتم العثور على يوم الخدمة."
            },
            ["BookingConfirmedSuccessfully"] = new()
            {
                ["en"] = "Booking confirmed successfully.",
                ["ar"] = "تم تأكيد الحجز بنجاح."
            },
            ["DoctorNotFound"] = new()
            {
                ["en"] = "Doctor not found.",
                ["ar"] = "لم يتم العثور على الطبيب."
            },
            ["RoleIdRequired"] = new()
            {
                ["en"] = "Role ID is required.",
                ["ar"] = "معرف الدور مطلوب."
            },
            ["RoleDeletionFailed"] = new()
            {
                ["en"] = "Failed to delete the role.",
                ["ar"] = "فشل في حذف الدور."
            },
            ["RoleDeletedSuccessfully"] = new()
            {
                ["en"] = "Role deleted successfully.",
                ["ar"] = "تم حذف الدور بنجاح."
            },
            ["FetchDoctorChartSuccess"] = new()
            {
                ["en"] = "Doctor chart fetched successfully.",
                ["ar"] = "تم جلب مخطط الطبيب بنجاح."
            },
            ["InvalidBookingStatus"] = new()
            {
                ["en"] = "Invalid booking status.",
                ["ar"] = "حالة الحجز غير صالحة."
            },
            ["FetchAllBookingOfDoctorsSuccess"] = new()
            {
                ["en"] = "All doctor bookings fetched successfully.",
                ["ar"] = "تم جلب جميع حجوزات الطبيب بنجاح."
            },
            ["GetResidentChartSuccess"] = new()
            {
                ["en"] = "Resident chart fetched successfully.",
                ["ar"] = "تم جلب مخطط المقيم بنجاح."
            },
            ["BookingStatusUpdatedSuccessfully"] = new()
            {
                ["en"] = "Booking status updated successfully.",
                ["ar"] = "تم تحديث حالة الحجز بنجاح."
            },
            ["BookingStatusIsAlreadyCompleted"] = new()
            {
                ["en"] = "The booking is already completed and cannot be updated.",
                ["ar"] = "هذا الحجز مكتمل بالفعل ولا يمكن تغيير حالته."
            },
            ["CannotUpdateServiceWithExistingBookings"] = new()
            {
                ["en"] = "This service cannot be updated because it has existing bookings.",
                ["ar"] = "لا يمكن تعديل هذه الخدمة لوجود حجوزات مرتبطة بها."
            },
            ["UpdateDoctorProfileSuccess"] = new()
            {
                ["en"] = "Doctor profile updated successfully.",
                ["ar"] = "تم تحديث ملف الطبيب بنجاح."
            },
            ["BookingUpdatedSuccessfully"] = new()
            {
                ["en"] = "Booking updated successfully.",
                ["ar"] = "تم تحديث الحجز بنجاح."
            },
            ["InvalidBookingUpdateDetails"] = new()
            {
                ["en"] = "Invalid booking update details.",
                ["ar"] = "بيانات تحديث الحجز غير صحيحة."
            },
            ["CannotDeleteServiceWithExistingBookings"] = new()
            {
                ["en"] = "This service cannot be deleted as it has active bookings.",
                ["ar"] = "لا يمكن حذف هذه الخدمة لوجود حجوزات نشطة."
            },
            ["ReviewNotFound"] = new()
            {
                ["en"] = "ReviewNotFound.",
                ["ar"] = "التقييم غير موجود"
            },
            ["ReviewDeletedSuccessfully"] = new()
            {
                ["en"] = "Review Deleted Successfully",
                ["ar"] = "تم حذف التقييم بنجاح"
            },
            ["ReviewAddedSuccessfully"] = new()
            {
                ["en"] = "Review Added Successfully",
                ["ar"] = "تم اضافة التقييم بنجاح"
            },
            ["ReviewUpdatedSuccessfully"] = new()
            {
                ["en"] = "Review updated Successfully",
                ["ar"] = "تم تعديل التقييم بنجاح"
            }
            ,
            ["CannotAddMoreThan3Reviews"] = new()
            {
                ["en"] = "You cannot add more than three reviews for the same service provider.",
                ["ar"] = "لا يمكن إضافة أكثر من 3 تقييمات لنفس مزود الخدمة."
            },
            ["InvalidServiceProviderType"] = new()
            {
                ["en"] = "Invalid Service Provider Type.",
                ["ar"] = "نوع مزود الخدمة غير صالح."
            },
            ["FavouriteNotFound"] = new()
            {
                ["en"] = "Favourite not found.",
                ["ar"] = "المفضل غير موجود."
            },
            ["FavouriteAddedSuccessfully"] = new()
            {
                ["en"] = "Favourite added successfully.",
                ["ar"] = "تمت إضافة المفضل بنجاح."
            },
            ["FavouriteRemovedSuccessfully"] = new()
            {
                ["en"] = "Favourite removed successfully.",
                ["ar"] = "تمت إزالة المفضل بنجاح."


            },
            ["FavouritesRetrievedSuccessfully"] = new()
            {
                ["en"] = "Favourites retrieved successfully.",
                ["ar"] = "تم جلب المفضلات بنجاح."
            },
            ["TimeZoneNotConfigured"] = new()
            {
                ["en"] = "Default time zone is not configured.",
                ["ar"] = "لم يتم إعداد المنطقة الزمنية الافتراضية."
            },
            ["BookingStatusUpdaterIterationFailed"] = new()
            {
                ["en"] = "An error occurred while processing bookings.",
                ["ar"] = "حدث خطأ أثناء معالجة الحجوزات."
            },
            ["ServiceHasBookings"] = new()
            {
                ["en"] = "This service cannot be deleted or update because it has existing bookings.",
                ["ar"] = "لا يمكن حذف او تعديل هذه الخدمة لوجود حجوزات مرتبطة بها."
            },
            ["TooManyRequests"]=new()
            {
                ["en"] = "Too many requests. Please try again later.",
                ["ar"] = "طلبات كثيرة جدا. يرجى المحاولة لاحقًا."
            }
            ,
            ["FileIsRequired"] = new()
            {
                ["en"] = "File is required.",
                ["ar"] = "الملف مطلوب."
            }
            ,
            ["FileSizeExceeded"] = new()
            {
                ["en"] = "File size exceeded the maximum limit of 5 MB.",
                ["ar"] = "تجاوز حجم الملف الحد الأقصى المسموح به وهو 5 ميجابايت."
            },
            ["InvalidFileType"] = new()
            {
                ["en"] = "Invalid file type. Allowed types are: .jpg, .jpeg, .png, .docx, .pdf.",
                ["ar"] = "نوع الملف غير صالح. الأنواع المسموح بها هي: .jpg، .jpeg، .png، .docx، .pdf."
            },
            ["InvalidFileContentType"] = new()
            {
                ["en"] = "Invalid file content type.",
                ["ar"] = "نوع محتوى الملف غير صالح."
            },
            ["FetchDoctorDataSuccess"]=new()
            {
                ["en"] = "Doctor data fetched successfully.",
                ["ar"] = "تم جلب بيانات الطبيب بنجاح."
            },
            ["RefreshTokenMissing"]=new()
            {
                ["en"] = "Refresh token is missing.",
                ["ar"] = "رمز التحديث مفقود."
            }

            ,
            ["Usernotloggedin"]=new()
            {
                ["en"] = "User is not logged in.",
                ["ar"] = "المستخدم غير مسجل الدخول."
            },
            ["UserLoggedOutSuccess"]=new()
            {
                ["en"] = "User logged out successfully.",
                ["ar"] = "تم تسجيل خروج المستخدم بنجاح."
            },
            ["UserHasAnotherBookingWithSameProviderOnThisDate"]= new()
            {
                ["en"] = "User has another booking with the same provider on this date.",
                ["ar"] = "للمستخدم حجز آخر مع نفس مقدم الخدمة في هذا التاريخ."
            },
            ["Newpasswordthesameastheoldpassword"]=new()
            {
                ["en"] = "The new password cannot be the same as the old password.",
                ["ar"] = "لا يمكن أن تكون كلمة المرور الجديدة هي نفسها كلمة المرور القديمة."
            },
            ["ServiceDeletedfromserviceprovider"] = new()
            {
                ["en"] = "The service has been deleted from the service provider.",
                ["ar"] = "تم حذف الخدمة من مزود الخدمة."
            },
            ["ToxicityPredictionSuccess"]=new()
            {
                ["en"] = "Toxicity prediction completed successfully.",
                ["ar"] = "تم إكمال التنبؤ بالسلبية بنجاح."
            },
            ["ReviewContainsToxicContent"]=new()
            {
                ["en"] = "The review contains toxic content.",
                ["ar"] = "التقييم يحتوي على محتوى غير مناسب."
            },
            ["UserBlockedDueToViolations"]= new()
            {
                ["en"] = "User is blocked due to multiple violations.",
                ["ar"] = "المستخدم محظور بسبب انتهاكات متعددة."
            },
            ["CollectedCountBookingsSuccess"] = new()
            {
                ["en"] = "Bookings count retrieved successfully.",
                ["ar"] = "تم جلب عدد الحجوزات بنجاح."
            },
            ["FailedToChangeUserStatus"] = new()
            {
                ["en"] = "Failed to change user status.",
                ["ar"] = "فشل تغيير حالة المستخدم."
            },
            ["SuccessToChangeUserStatus"] = new()
            {
                ["en"] = "User status changed successfully.",
                ["ar"] = "تم تغيير حالة المستخدم بنجاح."
            },
            ["SuccessToAddContact"] = new()
            {
                ["en"] = "Contact message sent successfully.",
                ["ar"] = "تم إرسال رسالة التواصل بنجاح."
            },
            ["SuccessToGetContacts"] = new()
            {
                ["en"] = "Contacts retrieved successfully.",
                ["ar"] = "تم جلب رسائل التواصل بنجاح."
            },
            ["SuccessToGetUserApproveResponses"] = new()
            {
                ["en"] = "User approval responses retrieved successfully.",
                ["ar"] = "تم جلب بيانات اعتماد المستخدمين بنجاح."
            },
            ["SuccessToGetUserDetails"] = new()
            {
                ["en"] = "User details retrieved successfully.",
                ["ar"] = "تم جلب بيانات المستخدم بنجاح."
            },
            ["GymNotFound"] = new()
            {
                ["en"] = "The requested gym was not found.",
                ["ar"] = "الجيم المطلوب غير موجود."
            },
        };


        public static string GetLocalizedMessage(string key, string lan)
        {
            if (messages.ContainsKey(key) && messages[key].ContainsKey(lan))
                return messages[key][lan];
            return "An error occurred.";
        }
    }
}
