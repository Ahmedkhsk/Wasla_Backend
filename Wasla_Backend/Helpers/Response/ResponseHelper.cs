namespace Wasla_Backend.Helpers.Response
{
    public static class ResponseHelper
    {
        public static ApiResponse Success(LocalizationKey key, string lan, dynamic? data = null)
        {
            return new ApiResponse(true, LocalizationHelper.GetLocalizedMessage(key, lan), data);
        }

        public static ApiResponse Fail(LocalizationKey key, string lan, dynamic? data = null)
        {
            return new ApiResponse(false, LocalizationHelper.GetLocalizedMessage(key, lan), data);
        }

    }
}
