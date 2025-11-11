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
                ["en"] = "Doctor data completed successfully",
                ["ar"] = "تم استكمال بيانات الطبيب بنجاح"
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
            }

        };

        public static string GetLocalizedMessage(string key, string lan)
        {
            if (messages.ContainsKey(key) && messages[key].ContainsKey(lan))
                return messages[key][lan];
            return "An error occurred.";
        }
    }
}
