namespace Shoes_Ecommerce.Helpers.Localization
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
            ["UserNameAlreadyExists"] = new()
            {
                ["en"] = "UserName is already taken",
                ["ar"] = "الاسم مستخدم بالفعل"
            },
            ["EmailAlreadyExists"] = new()
            {
                ["en"] = "Email is already taken",
                ["ar"] = "البريد الإلكتروني مستخدم بالفعل"
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
                ["en"] = "The password has been changed successfully.",
                ["ar"] = "تم تغيير كلمة المرور بنجاح."
            },


            ["ChangePasswordFailed"] = new()
            {
                ["en"] = "Failed to change the password.",
                ["ar"] = "فشل في تغيير كلمة المرور."
            },

            ["PassFailed"] = new()
            {
                ["en"] = "Password is incorrect.",
                ["ar"] = "كلمة المرور غير صحيحة."
            },
            ["UserNotFound"] = new()
            {
                ["en"] = "User not found.",
                ["ar"] = "لم يتم العثور على المستخدم."
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
                ["ar"] = "تم تسجيل دخول المستخدم بنجاح."
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
