namespace Wasla_Backend.Enums
{
    public enum LocalizationKey
    {
        #region Authentication & Authorization
        InvalidRequest,
        InvalidData,
        Unauthorized,
        InvalidToken,
        InvalidRefreshToken,
        TokenRefreshSuccess,
        RefreshTokenMissing,
        ExpiredRefreshToken,
        LoginSuccess,
        LoginFailed,
        UserNotLoggedIn,
        Usernotloggedin,
        UserLoggedOutSuccess,
        #endregion

        #region Registration & Verification
        UserNameAlreadyExists,
        EmailAlreadyExists,
        RegistrationSuccess,
        RegistrationFailed,
        VerificationSuccess,
        VerificationFailed,
        OTPSend,
        InvalidOrExpiredCode,
        UserNotVerified,
        UserNotApproved,
        EmailNotFound,
        EmailExists,
        verficationEmailSent,
        verficationEmailFailed,
        EmailVerificationFailed,
        EmailVerified,
        InvalidNationalId,
        CompleteDataSuccess,
        CompleteResidentRegisterSuccess,
        #endregion

        #region User & Profile
        UserNotFound,
        GetProfileSuccess,
        GetAllUsersSuccess,
        DeleteUserSuccess,
        UpdateProfileSuccess,
        UpdateDoctorProfileSuccess,
        ProfileEditSuccess,
        SuccessToGetUserDetails,
        FetchMembersSuccess,
        UserBlockedDueToViolations,
        FailedToChangeUserStatus,
        SuccessToChangeUserStatus,
        #endregion

        #region Password
        ChangePassSuccess,
        ChangePasswordFailed,
        ChangePassFailed,
        PassFailed,
        PassMismatch,
        IncorrectOldPass,
        IncorrectPassword,
        NewPasswordSameAsOld,
        Newpasswordthesameastheoldpassword,
        #endregion

        #region Doctor & Specialization
        DoctorNotFound,
        FetchDoctorProfileSuccess,
        FetchDoctorSpecializationsSuccess,
        FetchDoctorsBySpecialistSuccess,
        FetchAllDoctorsSuccess,
        FetchDoctorChartSuccess,
        FetchDoctorDataSuccess,
        FetchAllBookingOfDoctorsSuccess,
        SpecializationNotFound,
        #endregion

        #region Service
        ServiceAddedSuccessfully,
        ServiceNotFound,
        FetchServicesSuccess,
        ServiceUpdatedSuccessfully,
        ServiceDeletedSuccessfully,
        ServiceAlreadyBooked,
        ServiceBookedSuccessfully,
        ServiceDayNotFound,
        CannotUpdateServiceWithExistingBookings,
        CannotDeleteServiceWithExistingBookings,
        ServiceHasBookings,
        ServiceDeletedfromserviceprovider,
        ServiceIdRequired,
        servicehandlernotfound,
        InvalidServiceProviderType,
        #endregion

        #region Booking
        BookingSuccess,
        BookingRetrievedsuccess,
        BookingNotFound,
        BookingExpired,
        BookingCancelled,
        BookingCancelledSuccessfully,
        BookingConfirmedSuccessfully,
        BookingAddedSuccessfully,
        BookingsRetrievedSuccessfully,
        BookingUpdatedSuccessfully,
        BookingStatusUpdatedSuccessfully,
        BookingStatusIsAlreadyCompleted,
        InvalidBookingStatus,
        InvalidBookingUpdateDetails,
        UserHasAnotherBookingWithSameProviderOnThisDate,
        TimeSlotNotFound,
        CollectedCountBookingsSuccess,
        #endregion

        #region Gym & Package
        GymNotFound,
        Gymnotfound,
        AllGymsData,
        GymProfileData,
        PackageNotFound,
        PackageAddedSuccessfully,
        PackageUpdatedSuccessfully,
        PackageDeletedSuccessfully,
        PackagesRetrievedSuccessfully,
        PackageAlreadyBooked,
        #endregion

        #region Payment
        PaymentInitializedSuccessfully,
        PaymentInitializationFailed,
        PaymentProcessedSuccessfully,
        PaymentProcessingFailed,
        Invalidwebhooksignature,
        paymobApiFailed,
        PaymobApiFailed,
        InvalidPaymentMethod,
        PaymentMethodNotFound,
        AmountMustBeGreaterThanZero,
        #endregion

        #region Reviews
        GetReviewsSuccess,
        ReviewNotFound,
        ReviewDeletedSuccessfully,
        ReviewAddedSuccessfully,
        ReviewUpdatedSuccessfully,
        CannotAddMoreThan3Reviews,
        ReviewContainsToxicContent,
        ToxicityPredictionSuccess,
        #endregion

        #region Favourites
        FavouriteNotFound,
        FavouriteAddedSuccessfully,
        FavouriteRemovedSuccessfully,
        FavouritesRetrievedSuccessfully,
        #endregion

        #region Resident
        Residentnotfound,
        ResidentNotFound,
        ResidentIdRequired,
        GetResidentChartSuccess,
        SuccessToGetUserApproveResponses,
        #endregion

        #region Service Provider
        ServiceProviderNotFound,
        SuccessToGetTopServiceProviders,
        ServiceProviderIdRequired,
        FetchChartSuccess,
        #endregion

        #region Notifications
        NotificationNotFound,
        NotificationsFetched,
        NotificationMarkedAsSeen,
        AllNotificationsMarkedAsSeen,
        NotificationDeleted,
        NotificationAdded,
        TemplateNotFound,
        UserSubscriptionSuccess,
        UserUnsubscriptionSuccess,
        NotificationSentToTopicSuccess,
        NotificationSentToDeviceSuccess,
        #endregion

        #region Roles
        RoleNameRequired,
        RoleAddFailed,
        RoleAddedSuccessfully,
        RoleAlreadyExists,
        RoleIdRequired,
        RoleDeletionFailed,
        RoleDeletedSuccessfully,
        UserIdRequired,
        NoRolesFoundForUser,
        UserRolesRetrieved,
        NoRolesFound,
        RoleNotFound,
        AllRolesRetrieved,
        #endregion

        #region QR Code
        QrAlreadyUsed,
        QrCodeValid,
        QrCodeInvalid,
        InvalidQr,
        #endregion

        #region Dashboard & Analytics
        SuccessToGetUserDashboard,
        SuccessToGetAdminDashboard,
        FailedToGetDashboardData,
        SuccessToGetMostUsedServices,
        SuccessToGetConversionRates,
        SuccessToGetMostActiveUsers,
        #endregion

        #region Events
        SuccessToGetUserEvents,
        SuccessToCreateUserEvent,
        FailedToCreateUserEvent,
        NoUserEventsFound,
        #endregion

        #region Posts
        SuccessToCreatePost,
        FailedToCreatePost,
        SuccessToUpdatePost,
        FailedToUpdatePost,
        SuccessToDeletePost,
        SuccessToGetInformationProfile,
        FailedToDeletePost,
        SuccessToGetPost,
        SuccessToGetPosts,
        NoPostsFound,
        PostNotFound,
        UnauthorizedToModifyPost,
        #endregion

        #region Comments
        SuccessToCreateComment,
        FailedToCreateComment,
        SuccessToUpdateComment,
        FailedToUpdateComment,
        SuccessToDeleteComment,
        FailedToDeleteComment,
        SuccessToGetComments,
        NoCommentsFound,
        CommentNotFound,
        UnauthorizedToModifyComment,
        #endregion

        #region Reactions
        SuccessToCheckReaction,
        SuccessToAddReaction,
        FailedToAddReaction,
        SuccessToRemoveReaction,
        FailedToRemoveReaction,
        ReactionAlreadyExists,
        ReactionNotFound,
        SuccessToToggleReaction,
        #endregion

        #region Files
        FileIsRequired,
        FileSizeExceeded,
        InvalidFileType,
        InvalidFileContentType,
        #endregion

        #region Contacts
        SuccessToAddContact,
        SuccessToGetContacts,
        #endregion

        #region System / General
        ServerError,
        TooManyRequests,
        TimeZoneNotConfigured,
        BookingStatusUpdaterIterationFailed,
        NoUnitFound,
        #endregion

        #region Technician
        TechnicianNotFound,
        TechnicianCompleteRegisterSuccessfully,
        TechnicianProfileRetrievedSuccessfully,
        TechnicianProfileUpdatedSuccessfully,
        DocumentsAreRequired,
        TechnicianSpecialtiesRetrievedSuccessfully,
        TechniciansRetrievedSuccessfully,
        #endregion

        #region Driver
        DriverNotFound,
        VehicleNumberAlreadyExists,
        CarImagesAreRequired,
        DriverFilesAreRequired,
        DriverCompleteRegisterSuccess,
        GetDriverProfileSuccess ,
        ChangeDriverStatusSuccess,
        TrackingDriverSuccess,
        GetDriverLocationSuccess,
        DriverLocationNotFound,
        GetTopNearestDriverSuccess,
        VehicleTypeNotSupported,
        EstimateRideSuccessfully,
        RequestRideSuccessfully,
        ResidentHasActiveRide,
        RideNotFound,
        GetRideByIdSuccessfully,
        CannotCancelRide,
        SomeOneHadAcceptIt,
        InvalidRideStatus,
        AcceptRideSuccessfully,
        CompleteRideSuccessfully,
        CancelRideSuccessfully,
        StartRideSuccessfully,
        RideNotAcceptedYet,
        GetUserRidesSuccessfully,
        GetDriverRidesSuccessfully,
        RideCompleted,
        GetDriverChartSuccessfully,
        UpdateDriverProfileSuccess,
    #endregion

        #region ChatAndUserKeys

        SuccessToGetUsers,
        SuccessToMarkAsRead,
        NoUsersFound,
        FailedToGetUsers,
        MessageNotFoundOrNoPermission,
        ChatNotFoundOrNoPermission,
        SuccessToGetChats,
        NoChatsFound,
        FailedToGetChats,
        ChatNotFound,
        SuccessToGetUserProfile,
        SuccessToGetChat,
        SuccessToAddMessage,
        FailedToAddMessage,

        SuccessToUpdateMessage,
        FailedToUpdateMessage,

        SuccessToDeleteMessage,
        FailedToDeleteMessage,
        MessageNotFound,

        SuccessToDeleteChat,
        FailedToDeleteChat,

        SuccessToUpdateBio,
        FailedToUpdateBio,

        #endregion
    }
}