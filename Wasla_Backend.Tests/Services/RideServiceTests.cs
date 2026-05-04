using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation.Driver;
using Wasla_Backend.Models;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Enums;
using Wasla_Backend.DTOs.DriverDTOS;
using Wasla_Backend.Hubs;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Wasla_Backend.Models.Driver;
using Hangfire.MemoryStorage;
using Hangfire;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class RideServiceTests
    {
        private MockFactory _mocks;
        private RideService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();
            GlobalConfiguration.Configuration
       .UseMemoryStorage();

            JobStorage.Current = new MemoryStorage();

            // SignalR Hub Setup
            _mocks.RideHubClients
                .Setup(c => c.User(It.IsAny<string>()))
                .Returns(_mocks.SingleClient.Object);
            _mocks.RideHubContext
                .Setup(h => h.Clients)
                .Returns(_mocks.RideHubClients.Object);
            _mocks.SingleClient
                .Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            _sut = new RideService(
                _mocks.RideRepo.Object,
                _mocks.ResidentRepo.Object,
                _mocks.CreateRealMapper(),
                _mocks.DateTimeHelper.Object,
                _mocks.DriverService.Object,
                _mocks.DriverRepo.Object,
                _mocks.RideHubContext.Object,
                _mocks.FileUrlBuilder.Object,
                _mocks.EntityLoader.Object
            );
        }

        // ================================================================
        //  AcceptRide
        // ================================================================
        #region AcceptRide

        [Test]
        public async Task AcceptRide_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.AcceptRide(1, "driver-001", "en"));
        }

        [Test]
        public async Task AcceptRide_DriverNotFound_ThrowsNotFoundException()
        {
            var ride = BuildRide(status: RideStatus.Pending);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Driver?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.AcceptRide(ride.Id, "driver-001", "en"));
        }

        [Test]
        public async Task AcceptRide_DriverOnTrip_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Pending);
            var driver = BuildDriver(status: DriverStatus.OnTrip);

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.AcceptRide(ride.Id, driver.Id, "en"));
        }

        [Test]
        public async Task AcceptRide_RideNotPending_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Accepted); // مش Pending
            var driver = BuildDriver(status: DriverStatus.Online);

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.AcceptRide(ride.Id, driver.Id, "en"));
        }

        [Test]
        public async Task AcceptRide_RaceCondition_ThrowsBadRequestException()
        {
            // affectedRows == 0 يعني حد تاني سبقه
            var ride = BuildRide(status: RideStatus.Pending);
            var driver = BuildDriver(status: DriverStatus.Online);

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(driver.Id)).ReturnsAsync(driver);
            _mocks.RideRepo.Setup(r => r.UpdateRideStatusAsync(ride.Id, RideStatus.Accepted, driver.Id)).ReturnsAsync(0);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.AcceptRide(ride.Id, driver.Id, "en"));
        }

        [Test]
        public async Task AcceptRide_Valid_SetsDriverOnTripAndNotifies()
        {
            var ride = BuildRide(status: RideStatus.Pending, residentId: "res-001");
            var driver = BuildDriver(status: DriverStatus.Online);

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(driver.Id)).ReturnsAsync(driver);
            _mocks.RideRepo.Setup(r => r.UpdateRideStatusAsync(ride.Id, RideStatus.Accepted, driver.Id)).ReturnsAsync(1);
            _mocks.DriverRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            var result = await _sut.AcceptRide(ride.Id, driver.Id, "en");

            Assert.That(result, Is.EqualTo(ride.Id));
            Assert.That(driver.DriverStatus, Is.EqualTo(DriverStatus.OnTrip));
            _mocks.DriverRepo.Verify(r => r.Update(driver), Times.Once);
            _mocks.DriverRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mocks.RideHubClients.Verify(c => c.User(ride.ResidentId), Times.Once);
        }

        #endregion

        // ================================================================
        //  CancelRide
        // ================================================================
        #region CancelRide

        [Test]
        public async Task CancelRide_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.CancelRide(1, true, "en"));
        }

        [Test]
        public async Task CancelRide_AlreadyCancelled_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Cancelled);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.CancelRide(ride.Id, true, "en"));
        }

        [Test]
        public async Task CancelRide_WithDriver_SetsDriverStatusOnline()
        {
            var driver = BuildDriver(status: DriverStatus.OnTrip);
            var ride = BuildRide(status: RideStatus.Accepted, driverId: driver.Id);
            ride.Driver = driver;
            ride.Resident = BuildResident();

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            await _sut.CancelRide(ride.Id, true, "en");

            Assert.That(ride.Status, Is.EqualTo(RideStatus.Cancelled));
            Assert.That(driver.DriverStatus, Is.EqualTo(DriverStatus.Online));
        }

        [Test]
        public async Task CancelRide_NoDriver_CancelsRideAndReturnsId()
        {
            // ride مش معاه driver بعد
            var ride = BuildRide(status: RideStatus.Pending, driverId: null);
            ride.Resident = BuildResident();

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.DriverRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            var result = await _sut.CancelRide(ride.Id, true, "en");

            Assert.That(result, Is.EqualTo(ride.Id));
            Assert.That(ride.Status, Is.EqualTo(RideStatus.Cancelled));
        }

        [Test]
        public async Task CancelRide_DriverNull_LoadsDriverReference()
        {
            var driver = BuildDriver();
            var ride = BuildRide(status: RideStatus.Accepted, driverId: driver.Id);
            ride.Driver = null;   // مش loaded
            ride.Resident = BuildResident();

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.EntityLoader.Setup(e => e.LoadReferenceAsync(ride, r => r.Driver))
                .Callback(() => ride.Driver = driver)
                .Returns(Task.CompletedTask);
            _mocks.EntityLoader.Setup(e => e.LoadReferenceAsync(ride, r => r.Resident)).Returns(Task.CompletedTask);
            _mocks.DriverRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            await _sut.CancelRide(ride.Id, true, "en");

            _mocks.EntityLoader.Verify(e => e.LoadReferenceAsync(ride, r => r.Driver), Times.Once);
        }

        #endregion

        // ================================================================
        //  CompleteRide
        // ================================================================
        #region CompleteRide

        [Test]
        public async Task CompleteRide_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.CompleteRide(1, "en"));
        }

        [Test]
        public async Task CompleteRide_RideNotInProgress_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Accepted); // مش InProgress
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.CompleteRide(ride.Id, "en"));
        }

        [Test]
        public async Task CompleteRide_Valid_SetsStatusAndUpdatesDriver()
        {
            var driver = BuildDriver(status: DriverStatus.OnTrip, tripsCount: 5);
            var ride = BuildRide(status: RideStatus.InProgress);

            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.EntityLoader.Setup(e => e.LoadReferenceAsync(ride, r => r.Driver))
                .Callback(() => ride.Driver = driver)
                .Returns(Task.CompletedTask);
            _mocks.RideRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

            var result = await _sut.CompleteRide(ride.Id, "en");

            Assert.That(result, Is.EqualTo(ride.Id));
            Assert.That(ride.Status, Is.EqualTo(RideStatus.Completed));
            Assert.That(driver.DriverStatus, Is.EqualTo(DriverStatus.Online));
            Assert.That(driver.TripsCount, Is.EqualTo(6));
            _mocks.DriverRepo.Verify(r => r.Update(driver), Times.Once);
        }

        #endregion

        // ================================================================
        //  StartRide
        // ================================================================
        #region StartRide

        [Test]
        public async Task StartRide_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.StartRide(1));
        }

        [Test]
        public async Task StartRide_RideNotAccepted_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Pending);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.StartRide(ride.Id));
        }

        [Test]
        public async Task StartRide_Valid_SetsStatusInProgress()
        {
            var ride = BuildRide(status: RideStatus.Accepted);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.RideRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _sut.StartRide(ride.Id);

            Assert.That(result, Is.EqualTo(ride.Id));
            Assert.That(ride.Status, Is.EqualTo(RideStatus.InProgress));
            _mocks.RideRepo.Verify(r => r.Update(ride), Times.Once);
            _mocks.RideRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        // ================================================================
        //  RequestRide
        // ================================================================
        #region RequestRide

        [Test]
        public async Task RequestRide_ResidentNotFound_ThrowsNotFoundException()
        {
            var dto = BuildRequestRideDto();
            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(dto.PassengerId)).ReturnsAsync((Resident?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.RequestRide(dto, "en"));
        }

        [Test]
        public async Task RequestRide_HasActiveRide_ThrowsBadRequestException()
        {
            var dto = BuildRequestRideDto();
            var resident = BuildResident(id: dto.PassengerId);

            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(dto.PassengerId)).ReturnsAsync(resident);
            _mocks.RideRepo.Setup(r => r.IsHasActiveRide(dto.PassengerId)).ReturnsAsync(true);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.RequestRide(dto, "en"));
        }

        [Test]
        public async Task RequestRide_Valid_CreatesRideAndReturnsId()
        {
            var dto = BuildRequestRideDto();
            var resident = BuildResident(id: dto.PassengerId);

            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(dto.PassengerId)).ReturnsAsync(resident);
            _mocks.RideRepo.Setup(r => r.IsHasActiveRide(dto.PassengerId)).ReturnsAsync(false);
            _mocks.DateTimeHelper.Setup(d => d.Now).Returns(DateTime.UtcNow);
            _mocks.RideRepo.Setup(r => r.AddAsync(It.IsAny<ride>())).Returns(Task.CompletedTask);
            _mocks.RideRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");
            _mocks.DriverService.Setup(d => d.GetTopNearestDriver(
                dto.PickupLatitude, dto.PickupLongitude, dto.VehicleType
            )).ReturnsAsync(new List<string> { "driver-001" });

            await _sut.RequestRide(dto, "en");

            _mocks.RideRepo.Verify(r => r.AddAsync(It.Is<ride>(ride =>
                ride.ResidentId == dto.PassengerId &&
                ride.Status == RideStatus.Pending
            )), Times.Once);
            _mocks.RideRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        // ================================================================
        //  CheckRideAcceptance
        // ================================================================
        #region CheckRideAcceptance

        [Test]
        public async Task CheckRideAcceptance_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.CheckRideAcceptance(1));
        }

        [Test]
        public async Task CheckRideAcceptance_NoDriver_CancelsRide()
        {
            var ride = BuildRide(status: RideStatus.Pending, driverId: null);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);
            _mocks.RideRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _sut.CheckRideAcceptance(ride.Id);

            Assert.That(ride.Status, Is.EqualTo(RideStatus.Cancelled));
            _mocks.RideRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CheckRideAcceptance_HasDriver_DoesNotCancel()
        {
            var ride = BuildRide(status: RideStatus.Accepted, driverId: "driver-001");
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            await _sut.CheckRideAcceptance(ride.Id);

            Assert.That(ride.Status, Is.Not.EqualTo(RideStatus.Cancelled));
            _mocks.RideRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        // ================================================================
        //  EstimateRide
        // ================================================================
        #region EstimateRide

        [Test]
        public void EstimateRide_CarType_ReturnsCorrectEstimate()
        {
            var dto = new CalculateRideDto
            {
                PickupLatitude = 30.0444,
                PickupLongitude = 31.2357,
                DropoffLatitude = 30.0626,
                DropoffLongitude = 31.2497,
                VehicleType = VehicleType.Car
            };

            var result = _sut.EstimateRide(dto);

            Assert.That(result.EstimatedPrice, Is.GreaterThan(0));
            Assert.That(result.Distance, Is.GreaterThan(0));
        }

        [Test]
        public void EstimateRide_ScooterType_CheaperThanCar()
        {
            var dto = new CalculateRideDto
            {
                PickupLatitude = 30.0444,
                PickupLongitude = 31.2357,
                DropoffLatitude = 30.0626,
                DropoffLongitude = 31.2497,
                VehicleType = VehicleType.Scooter
            };

            var carDto = new CalculateRideDto
            {
                PickupLatitude = 30.0444,
                PickupLongitude = 31.2357,
                DropoffLatitude = 30.0626,
                DropoffLongitude = 31.2497,
                VehicleType = VehicleType.Car
            };

            var scooterResult = _sut.EstimateRide(dto);
            var carResult = _sut.EstimateRide(carDto);

            Assert.That(scooterResult.EstimatedPrice, Is.LessThan(carResult.EstimatedPrice));
        }

        [Test]
        public void EstimateRide_InvalidVehicleType_ThrowsBadRequestException()
        {
            var dto = new CalculateRideDto
            {
                PickupLatitude = 30.0,
                PickupLongitude = 31.0,
                DropoffLatitude = 30.1,
                DropoffLongitude = 31.1,
                VehicleType = (VehicleType)99
            };

            Assert.Throws<BadRequestException>(() => _sut.EstimateRide(dto));
        }

        #endregion

        // ================================================================
        //  GetDriverRides / GetUserRides / GetDriverChart
        // ================================================================
        #region GetData

        [Test]
        public async Task GetDriverRides_DriverNotFound_ThrowsNotFoundException()
        {
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Driver?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.GetDriverRides("driver-001"));
        }

        [Test]
        public async Task GetDriverRides_Valid_ReturnsRides()
        {
            var driver = BuildDriver();
            var rides = new List<DriverRideDto> { new() { RideId = 1 } };

            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(driver.Id)).ReturnsAsync(driver);
            _mocks.RideRepo.Setup(r => r.GetDriverRides(driver.Id)).ReturnsAsync(rides);

            var result = await _sut.GetDriverRides(driver.Id);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserRides_ResidentNotFound_ThrowsNotFoundException()
        {
            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Resident?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.GetUserRides("res-001"));
        }

        [Test]
        public async Task GetUserRides_Valid_ReturnsRides()
        {
            var resident = BuildResident();
            var rides = new List<UserRideDto> { new() { RideId = 1 } };

            _mocks.ResidentRepo.Setup(r => r.GetByIdAsync(resident.Id)).ReturnsAsync(resident);
            _mocks.RideRepo.Setup(r => r.GetUserRides(resident.Id)).ReturnsAsync(rides);

            var result = await _sut.GetUserRides(resident.Id);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetDriverChart_DriverNotFound_ThrowsNotFoundException()
        {
            _mocks.DriverRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Driver?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.GetDriverChart("driver-001"));
        }

        #endregion

        // ================================================================
        //  GetrideDetailsForResident
        // ================================================================
        #region GetrideDetailsForResident

        [Test]
        public async Task GetrideDetailsForResident_RideNotFound_ThrowsNotFoundException()
        {
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ride?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.GetrideDetailsForResident(1));
        }

        [Test]
        public async Task GetrideDetailsForResident_NoDriver_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Pending, driverId: null);
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.GetrideDetailsForResident(ride.Id));
        }

        [Test]
        public async Task GetrideDetailsForResident_CompletedRide_ThrowsBadRequestException()
        {
            var ride = BuildRide(status: RideStatus.Completed, driverId: "driver-001");
            _mocks.RideRepo.Setup(r => r.GetByIdAsync(ride.Id)).ReturnsAsync(ride);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.GetrideDetailsForResident(ride.Id));
        }

        #endregion

        // ================================================================
        //  Private Helpers
        // ================================================================
        private static ride BuildRide(
            RideStatus status = RideStatus.Pending,
            string? driverId = null,
            string residentId = "res-001",
            int id = 1) => new()
            {
                Id = id,
                Status = status,
                DriverId = driverId,
                ResidentId = residentId,
                Distance = 5.0,
            };

        private static Driver BuildDriver(
            string id = "driver-001",
            DriverStatus status = DriverStatus.Online,
            int tripsCount = 0) => new()
            {
                Id = id,
                FullName = "Test Driver",
                ProfilePhoto = "driver.jpg",
                DriverStatus = status,
                TripsCount = tripsCount,
            };

        private static Resident BuildResident(string id = "res-001") => new()
        {
            Id = id,
            FullName = "Test Resident",
            ProfilePhoto = "resident.jpg",
        };

        private static RequestRideDto BuildRequestRideDto() => new()
        {
            PassengerId = "res-001",
            PickupLatitude = 30.0444,
            PickupLongitude = 31.2357,
            DropoffLatitude = 30.0626,
            DropoffLongitude = 31.2497,
            VehicleType = VehicleType.Car,
            PickUpPlace = "Cairo",
            DropOffPlace = "Giza",
        };

    }
}