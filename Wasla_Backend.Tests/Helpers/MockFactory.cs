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
using Microsoft.AspNetCore.SignalR;
using Wasla_Backend.Hubs.RestaurantHubs;
using Wasla_Backend.Services.Interfaces.General;
using Wasla_Backend.Strategies.Payment;
using Wasla_Backend.Hubs.DriverHubs;
using Wasla_Backend.Repositories.Interfaces.driver;
using Wasla_Backend.Repositories.Interfaces.Driver;
using Wasla_Backend.Repositories.Interfaces.General;
using Wasla_Backend.Services.Interfaces.Driver;

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
        public Mock<IResidentRepository> ResidentRepo { get; } = new();
        public Mock<IMenuItemCategoryRepository> MenuItemCategoryRepo { get; } = new();
        public Mock<IRestaurantRepository> RestaurantRepository { get; } = new();
        public Mock<IMenuItemRepository> MenuItemRepository { get; } = new();
        public Mock<IMenuItemCategoryRepository> MenuItemCategoryRepository { get; } = new();
        public Mock<IMapper> Mapper { get; } = new();
        public Mock<IFileUrlBuilderService> FileUrlBuilderService { get; } = new();
        public Mock<ICartRepository> CartRepo { get; } = new();
        public Mock<ICartItemRepository> CartItemRepo { get; } = new();
        public Mock<IMenuItemRepository> MenuItemRepo { get; } = new();
        public Mock<IPaymentStrategyFactory> PaymentStrategyFactory { get; } = new();
        public Mock<IPaymentStrategy> PaymentStrategy { get; } = new();
        public Mock<IPaymentService> PaymentService { get; } = new();
        public Mock<IHubContext<OrderHub>> HubContext { get; } = new();
        public Mock<IHubClients> HubClients { get; } = new();
        public Mock<IClientProxy> ClientProxy { get; } = new();
        public Mock<IRideRepository> RideRepo { get; } = new();
        public Mock<IDriverRepository> DriverRepo { get; } = new();
        public Mock<IDriverService> DriverService { get; } = new();
        public Mock<IEntityLoader> EntityLoader { get; } = new();
        public Mock<IHubContext<RideHub>> RideHubContext { get; } = new();
        public Mock<IHubClients> RideHubClients { get; } = new();
        public Mock<ISingleClientProxy> SingleClient { get; } = new();

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
            config.AddProfile<ReservationProfile>();
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
        public static AddReservationDto BuildAddReservationDto()
        {
            return new AddReservationDto
            {
                restaurantId = "rest-1",
                userId = "user-1",
                numberOfPersons = 2,
                reservationDate = DateOnly.FromDateTime(DateTime.Now),
                reservationTime = TimeOnly.FromDateTime(DateTime.Now)
            };
        }

        public static Resident BuildResident(string id = "user-1")
        {
            return new Resident
            {
                Id = id,
                FullName = "Test Resident"
            };
        }

        public static Reservations BuildReservation()
        {
            return new Reservations
            {
                id = 1,
                userId = "user-1",
                restaurantId = "rest-1",
                status = Status.Pending
            };
        }

        public static AddMenuItemDto BuildAddMenuItemDto()
        {
            return new AddMenuItemDto
            {
                restaurantId = "r1",
                categoryId = 1,
                imageUrl = null
            };
        }

        public static UpdateMenuItemDto BuildUpdateMenuItemDto()
        {
            return new UpdateMenuItemDto
            {
                id = 1,
                imageUrl = null
            };
        }

        public static ChangeStatusItemMenuDto BuildChangeStatusDto()
        {
            return new ChangeStatusItemMenuDto
            {
                restaurantId = "r1",
                menuItemId = 1
            };
        }
    }
}