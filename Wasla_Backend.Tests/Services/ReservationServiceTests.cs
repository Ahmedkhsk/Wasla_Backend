using NUnit.Framework;
using Moq;
using FluentAssertions;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Helpers.Localization;
using Wasla_Backend.Enums;
using Wasla_Backend.Models;
using Wasla_Backend.DTOs.PaginationDTOS;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Microsoft.AspNetCore.Http;
using Hangfire;
using Hangfire.MemoryStorage;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private MockFactory _mocks;
        private ReservationService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();
            _sut = new ReservationService(
                _mocks.ReservationRepo.Object,
                _mocks.RestaurantRepo.Object,
                _mocks.ResidentRepo.Object,
                _mocks.FileUrlBuilder.Object,
                _mocks.CreateRealMapper(),
                _mocks.FileService.Object,
                _mocks.DateTimeHelper.Object,
               _mocks.UserAuthorizationService.Object

            );
            GlobalConfiguration.Configuration.UseStorage(new MemoryStorage());
        }

        #region AddReservation

        [Test]
        public async Task AddReservation_ValidDto_AddsAndSaves()
        {
            // Arrange
            var dto = TestDataBuilder.BuildAddReservationDto();
            var restaurant = TestDataBuilder.BuildRestaurant(id: dto.restaurantId);
            var resident = TestDataBuilder.BuildResident(id: dto.userId);

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.restaurantId)).ReturnsAsync(restaurant);
            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(dto.userId)).ReturnsAsync(resident);
            _mocks.ReservationRepo.Setup(r => r.AddAsync(It.IsAny<Reservations>())).Returns(Task.CompletedTask);
            _mocks.ReservationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");
            _mocks.DateTimeHelper.Setup(d => d.CalculateDelay(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>())).Returns(TimeSpan.FromMinutes(10));

            // Act
            await _sut.AddReservatio(dto);

            // Assert
            _mocks.ReservationRepo.Verify(r => r.AddAsync(It.Is<Reservations>(res =>
                res.userId == dto.userId &&
                res.restaurantId == dto.restaurantId &&
                res.numberOfPersons == dto.numberOfPersons &&
                res.status == Status.Pending
            )), Times.Once);
            _mocks.ReservationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddReservation_RestaurantNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = TestDataBuilder.BuildAddReservationDto();
            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.restaurantId)).ReturnsAsync((Restaurant?)null);

            // Act
            var act = async () => await _sut.AddReservatio(dto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task AddReservation_ResidentNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var dto = TestDataBuilder.BuildAddReservationDto();
            var restaurant = TestDataBuilder.BuildRestaurant(id: dto.restaurantId);

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.restaurantId)).ReturnsAsync(restaurant);
            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(dto.userId)).ReturnsAsync((Resident?)null);

            // Act
            var act = async () => await _sut.AddReservatio(dto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region ChangeStatus

        [Test]
        public async Task ChangeStatus_ToPending_UpdatesStatusAndSaves()
        {
            // Arrange
            var reservation = TestDataBuilder.BuildReservation();
            reservation.restaurants = TestDataBuilder.BuildRestaurant();
            reservation.user = TestDataBuilder.BuildResident();

            _mocks.ReservationRepo.Setup(r => r.GetWithResidentAndRestaurant(reservation.id)).ReturnsAsync(reservation);
            _mocks.ReservationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            // Act
            await _sut.ChangeStatus(reservation.id, Status.Canceled);

            // Assert
            reservation.status.Should().Be(Status.Canceled);
            _mocks.ReservationRepo.Verify(r => r.Update(reservation), Times.Once);
            _mocks.ReservationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ChangeStatus_ToAccepted_GeneratesQrAndSaves()
        {
            // Arrange
            var reservation = TestDataBuilder.BuildReservation();
            reservation.restaurants = TestDataBuilder.BuildRestaurant();
            reservation.user = TestDataBuilder.BuildResident();
            var qrPath = "qr/reservation_1.png";

            _mocks.ReservationRepo.Setup(r => r.GetWithResidentAndRestaurant(reservation.id)).ReturnsAsync(reservation);
            _mocks.FileUrlBuilder.Setup(f => f.GetPath(MediaType.qrCode)).Returns("path/qr");
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");
            _mocks.FileService.Setup(f => f.AddFileAsync(It.IsAny<IFormFile>(), "path/qr")).ReturnsAsync(qrPath);
            _mocks.ReservationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _sut.ChangeStatus(reservation.id, Status.Accepted);

            // Assert
            reservation.status.Should().Be(Status.Accepted);
            reservation.QRCode.Should().Be(qrPath);
            _mocks.FileService.Verify(f => f.AddFileAsync(It.IsAny<IFormFile>(), "path/qr"), Times.Once);
        }

        [Test]
        public async Task ChangeStatus_ReservationNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _mocks.ReservationRepo.Setup(r => r.GetWithResidentAndRestaurant(It.IsAny<int>())).ReturnsAsync((Reservations?)null);

            // Act
            var act = async () => await _sut.ChangeStatus(99, Status.Accepted);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region GetRestaurantReservations

        [Test]
        public async Task GetRestaurantReservations_ReturnsPagedMappedResult()
        {
            // Arrange
            var dto = new GetGeneralWithPaginationDto<string> { id = "rest-001", PageNumber = 1, PageSize = 10 };
            var resident = TestDataBuilder.BuildResident();
            var pagedData = new PagedResult<Reservations>
            {
                Data = new List<Reservations>
                {
                    new() { id = 1, user = resident, userId = resident.Id, status = Status.Pending }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1,
            };

            _mocks.ReservationRepo.Setup(r => r.GetRestaurantReservations(dto)).ReturnsAsync(pagedData);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.userImage)).Returns("url/photo.jpg");

            // Act
            var result = await _sut.GetRestaurantReservations(dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            result.Data.Should().HaveCount(1);
            result.Data.First().profile.Should().Be("url/photo.jpg");
        }

        [Test]
        public async Task GetRestaurantReservations_EmptyList_ReturnsEmpty()
        {
            // Arrange
            var dto = new GetGeneralWithPaginationDto<string> { id = "rest-001", PageNumber = 1, PageSize = 10 };
            var pagedData = new PagedResult<Reservations>
            {
                Data = new List<Reservations>(),
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            _mocks.ReservationRepo.Setup(r => r.GetRestaurantReservations(dto)).ReturnsAsync(pagedData);

            // Act
            var result = await _sut.GetRestaurantReservations(dto);

            // Assert
            result.Data.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        #endregion

        #region GetResidentReservations

        [Test]
        public async Task GetResidentReservations_ReturnsPagedMappedResult()
        {
            // Arrange
            var dto = new GetGeneralWithPaginationDto<string> { id = "res-001", PageNumber = 1, PageSize = 10 };
            var restaurant = TestDataBuilder.BuildRestaurant();
            var pagedData = new PagedResult<Reservations>
            {
                Data = new List<Reservations>
                {
                    new()
                    {
                        id          = 1,
                        restaurants = restaurant,
                        restaurantId = restaurant.Id,
                        QRCode      = "qr.png",
                        status      = Status.Accepted
                    }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1,
            };

            _mocks.ReservationRepo.Setup(r => r.GetResidentReservations(dto)).ReturnsAsync(pagedData);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.userImage)).Returns("url/photo.jpg");
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.qrCode)).Returns("url/qr.png");

            // Act
            var result = await _sut.GetResidentReservations(dto);

            // Assert
            result.Data.Should().HaveCount(1);
            result.Data.First().restaurantProfile.Should().Be("url/photo.jpg");
            result.Data.First().QRCode.Should().Be("url/qr.png");
        }

        #endregion
    }
}