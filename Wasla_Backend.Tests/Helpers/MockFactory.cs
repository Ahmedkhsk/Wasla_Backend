using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Wasla_Backend.Models;
using Wasla_Backend.Repositories.Interfaces;
using Wasla_Backend.Services.Interfaces;
using Wasla_Backend.Services.Interfaces.Files;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.Helpers.Cashing;
using Wasla_Backend.Helpers;
using Wasla_Backend.Helpers.Time;
using Wasla_Backend.Helpers.EmailSender;
using Wasla_Backend.Factories.Interfaces;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.DTOs.PaginationDTOS;
using Wasla_Backend.DTOs.Authentication;
using Wasla_Backend.DTOs;
using Wasla_Backend.Helpers.Localization;
using Wasla_Backend.Mappings;
using Wasla_Backend.Enums;

namespace Wasla_Backend.Tests.Helpers
{
    // ============================================================
    //  MockFactory — كل الـ Mocks في مكان واحد
    // ============================================================
    public class MockFactory
    {
        public Mock<IRestaurantRepository> RestaurantRepo { get; } = new();
        public Mock<IUserRepository> UserRepo { get; } = new();
        public Mock<IFileService> FileService { get; } = new();
        public Mock<IFileUrlBuilderService> FileUrlBuilder { get; } = new();
        public Mock<IGenericRepository<RestaurantCategory>> RestaurantCategoryRepo { get; } = new();
        public Mock<IReservationRepository> ReservationRepo { get; } = new();
        public Mock<IOrderRepository> OrderRepo { get; } = new();
        public Mock<IRoleRepository> RoleRepo { get; } = new();
        public Mock<IRefreshTokenRepository> RefreshTokenRepo { get; } = new();
        public Mock<ICacheManager> CacheManager { get; } = new();
        public Mock<ITokenHelper> TokenHelper { get; } = new();
        public Mock<IDateTimeHelper> DateTimeHelper { get; } = new();
        public Mock<IEmailSenderHelper> EmailSender { get; } = new();
        public Mock<IHttpContextAccessor> HttpContextAccessor { get; } = new();
        public Mock<IUserFactory> UserFactory { get; } = new();
        public Mock<UserManager<ApplicationUser>> UserManager { get; } = MockUserManager();

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );
        }

        public IMapper CreateRealMapper()
        {
            var config = new MapperConfigurationExpression();
            config.AddProfile<RestaurantProfile>();
            var mapperConfig = new MapperConfiguration(config);
            return mapperConfig.CreateMapper();
        }
    }

    public static class TestDataBuilder
    {
        public static Restaurant BuildRestaurant(string? id = null, string? email = null) => new()
        {
            Id = id ?? "rest-001",
            Email = email ?? "rest@test.com",
            BusinessName = "Test Restaurant",
            FullName = "Owner Name",
            Description = "Test Desc",
            Phone = "01000000000",
            ProfilePhoto = "photo.jpg",
            images = new List<string> { "img1.jpg", "img2.jpg" },
            IsCompleteRegistration = false,
            restaurantCategoryId = 1,
        };

        // User — برضو child class
        public static Restaurant BuildUser(string? id = null, string? email = null) => new()
        {
            Id = id ?? "user-001",
            Email = email ?? "user@test.com",
            UserName = "testuser",
            IsVerified = true,
            IsDeleted = false,
            Status = UserStatus.Active,
            IsCompleteRegistration = true,
        };

        public static RestaurantCategory BuildCategory(int id = 1) => new()
        {
            name = new MultilingualText { English = "Italian", Arabic = "إيطالي" },
        };

        public static CompleteRegisterRestaurantDto BuildCompleteProfileDto(int categoryId = 1) => new()
        {
            email = "rest@test.com",
            name = "Test Restaurant",
            ownerName = "Owner Name",
            description = "Test Desc",
            phoneNumber = "01000000000",
            restaurantCategoryId = categoryId,
            profile = null,
            gallery = null,
        };

        public static UpdateRestaurantDto BuildUpdateRestaurantDto(string id = "rest-001", int categoryId = 1) => new()
        {
            id = id,
            name = "Updated Name",
            ownerName = "Updated Owner",
            description = "Updated Desc",
            phoneNumber = "01111111111",
            restaurantCategoryId = categoryId,
            profile = null,
            files = new FilesUpdateDto { existingFiles = new List<string>(), newFiles = null },
        };

        public static LoginDto BuildLoginDto(string email = "user@test.com", string password = "Pass@123") => new()
        {
            Email = email,
            Password = password,
        };

        public static RegisterDto BuildRegisterDto(string email = "new@test.com", string roleId = "role-001") => new()
        {
            Email = email,
            Password = "Pass@123",
            roleId = roleId,
        };

        public static RefreshToken BuildRefreshToken(string userId = "user-001", bool expired = false) => new()
        {
            Token = "valid-refresh-token",
            UserId = userId,
            ExpiresAt = expired ? DateTime.UtcNow.AddDays(-1) : DateTime.UtcNow.AddDays(7),
        };

        public static PagedResult<Restaurant> BuildPagedRestaurants(int count = 2) => new()
        {
            Data = Enumerable.Range(1, count).Select(i => BuildRestaurant($"rest-00{i}", $"rest{i}@test.com")).ToList(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = count,
        };
    }
}