using NUnit.Framework;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.DTOs.PaginationDTOS;
using Wasla_Backend.Enums;
using Wasla_Backend.Exceptions;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Wasla_Backend.DTOs.ChartDTOS;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class RestaurantServiceTests
    {
        private MockFactory _mocks;
        private RestaurantService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            _sut = new RestaurantService(
                _mocks.RestaurantRepo.Object,
                _mocks.UserRepo.Object,
                _mocks.FileService.Object,
                _mocks.CreateRealMapper(),
                _mocks.FileUrlBuilder.Object,
                _mocks.RestaurantCategoryRepo.Object,
                _mocks.ReservationRepo.Object,
                _mocks.OrderRepo.Object
            );
        }

       
        #region CompleteProfile

        [Test]
        public async Task CompleteProfile_ValidDto_UpdatesRestaurantAndSaves()
        {
            var dto = TestDataBuilder.BuildCompleteProfileDto();
            var restaurant = TestDataBuilder.BuildRestaurant(email: dto.email);
            var category = TestDataBuilder.BuildCategory(dto.restaurantCategoryId);

            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);

            await _sut.CompleteProfile(dto);

            restaurant.IsCompleteRegistration.Should().BeTrue();
            _mocks.RestaurantRepo.Verify(r => r.Update(restaurant), Times.Once);
            _mocks.RestaurantRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CompleteProfile_WithProfilePhoto_UploadsFile()
        {
            var dto = TestDataBuilder.BuildCompleteProfileDto();
            dto.profile = new FakeFormFile("photo.jpg");
            var restaurant = TestDataBuilder.BuildRestaurant(email: dto.email);
            var category = TestDataBuilder.BuildCategory();
            var uploadedPath = "uploaded/photo.jpg";

            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileUrlBuilder.Setup(f => f.GetPath(MediaType.userImage)).Returns("path/images");
            _mocks.FileService.Setup(f => f.AddFileAsync(dto.profile, "path/images")).ReturnsAsync(uploadedPath);

            await _sut.CompleteProfile(dto);

            restaurant.ProfilePhoto.Should().Be(uploadedPath);
            _mocks.FileService.Verify(f => f.AddFileAsync(dto.profile, "path/images"), Times.Once);
        }

        [Test]
        public async Task CompleteProfile_WithGallery_UploadsFiles()
        {
            var dto = TestDataBuilder.BuildCompleteProfileDto();
            dto.gallery = new List<IFormFile> { new FakeFormFile("img1.jpg") };
            var restaurant = TestDataBuilder.BuildRestaurant(email: dto.email);
            var category = TestDataBuilder.BuildCategory();
            var uploadedPaths = new List<string> { "uploaded/img1.jpg" };

            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileUrlBuilder.Setup(f => f.GetPath(MediaType.restaurantImage)).Returns("path/gallery");
            _mocks.FileService.Setup(f => f.AddFilesAsync(dto.gallery, "path/gallery")).ReturnsAsync(uploadedPaths);

             
            await _sut.CompleteProfile(dto);

             
            restaurant.images.Should().BeEquivalentTo(uploadedPaths);
        }

        [Test]
        public async Task CompleteProfile_NoPhoto_NoGallery_DoesNotCallFileService()
        {
            // 
            var dto = TestDataBuilder.BuildCompleteProfileDto();
            var restaurant = TestDataBuilder.BuildRestaurant(email: dto.email);
            var category = TestDataBuilder.BuildCategory();

            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);

             
            await _sut.CompleteProfile(dto);

            _mocks.FileService.Verify(f => f.AddFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
            _mocks.FileService.Verify(f => f.AddFilesAsync(It.IsAny<IEnumerable<IFormFile>>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task CompleteProfile_RestaurantNotFound_ThrowsNotFoundException()
        {
              
            var dto = TestDataBuilder.BuildCompleteProfileDto();
            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync((Restaurant?)null);

             
            var act = async () => await _sut.CompleteProfile(dto);

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task CompleteProfile_CategoryNotFound_ThrowsNotFoundException()
        {
                       var dto = TestDataBuilder.BuildCompleteProfileDto();
            var restaurant = TestDataBuilder.BuildRestaurant(email: dto.email);

            _mocks.RestaurantRepo.Setup(r => r.GetByEmailAsync(dto.email)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync((RestaurantCategory?)null);

             
            var act = async () => await _sut.CompleteProfile(dto);

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion



        #region UpdateRestaurant

        [Test]
        public async Task UpdateRestaurant_ValidDto_UpdatesAndSaves()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = TestDataBuilder.BuildUpdateRestaurantDto(restaurant.Id);
            var category = TestDataBuilder.BuildCategory(dto.restaurantCategoryId);

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileService.Setup(f => f.ExtractFileNames(dto.files.existingFiles)).Returns(new List<string>());

             
            await _sut.UpdateRestaurant(dto);

             
            _mocks.RestaurantRepo.Verify(r => r.Update(restaurant), Times.Once);
            _mocks.RestaurantRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateRestaurant_WithNewProfilePhoto_ReplacesFile()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = TestDataBuilder.BuildUpdateRestaurantDto(restaurant.Id);
            dto.profile = new FakeFormFile("new.jpg");
            var category = TestDataBuilder.BuildCategory();
            var newPath = "new/photo.jpg";

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileService.Setup(f => f.ExtractFileNames(It.IsAny<IEnumerable<string>>())).Returns(new List<string>());
            _mocks.FileUrlBuilder.Setup(f => f.GetPath(MediaType.userImage)).Returns("path/images");
            _mocks.FileService.Setup(f => f.ReplaceFileAsync(restaurant.ProfilePhoto, dto.profile, "path/images")).ReturnsAsync(newPath);

             
            await _sut.UpdateRestaurant(dto);

             
            restaurant.ProfilePhoto.Should().Be(newPath);
            _mocks.FileService.Verify(f => f.ReplaceFileAsync(It.IsAny<string>(), dto.profile, "path/images"), Times.Once);
        }

        [Test]
        public async Task UpdateRestaurant_WithNewGallery_ReplacesFiles()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = TestDataBuilder.BuildUpdateRestaurantDto(restaurant.Id);
            dto.files.newFiles = new List<IFormFile> { new FakeFormFile("new1.jpg") };
            var category = TestDataBuilder.BuildCategory();
            var existingNames = new List<string> { "img1.jpg" };
            var newPaths = new List<string> { "new/img1.jpg" };

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileService.Setup(f => f.ExtractFileNames(It.IsAny<IEnumerable<string>>())).Returns(existingNames);
            _mocks.FileUrlBuilder.Setup(f => f.GetPath(MediaType.restaurantImage)).Returns("path/gallery");
            _mocks.FileService.Setup(f => f.ReplaceFilesAsync(
                restaurant.images, existingNames, dto.files.newFiles, "path/gallery"
            )).ReturnsAsync(newPaths);

             
            await _sut.UpdateRestaurant(dto);

             
            restaurant.images.Should().BeEquivalentTo(newPaths);
        }

        [Test]
        public async Task UpdateRestaurant_NoNewPhoto_DoesNotReplacePhoto()
        {
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = TestDataBuilder.BuildUpdateRestaurantDto(restaurant.Id);
            var category = TestDataBuilder.BuildCategory();

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync(category);
            _mocks.FileService.Setup(f => f.ExtractFileNames(It.IsAny<IEnumerable<string>>())).Returns(new List<string>());

             
            await _sut.UpdateRestaurant(dto);

             
            _mocks.FileService.Verify(f => f.ReplaceFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task UpdateRestaurant_RestaurantNotFound_ThrowsNotFoundException()
        {
             
            var dto = TestDataBuilder.BuildUpdateRestaurantDto();
            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync((Restaurant?)null);

             
            var act = async () => await _sut.UpdateRestaurant(dto);

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task UpdateRestaurant_CategoryNotFound_ThrowsNotFoundException()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = TestDataBuilder.BuildUpdateRestaurantDto(restaurant.Id);

            _mocks.RestaurantRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.restaurantCategoryId)).ReturnsAsync((RestaurantCategory?)null);

             
            var act = async () => await _sut.UpdateRestaurant(dto);

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

      
        #region GetAll

        [Test]
        public async Task GetAll_ReturnsPagedMappedResult()
        {
             
            var paged = TestDataBuilder.BuildPagedRestaurants(2);
            var dto = new GetGeneralWithPaginationDto<int> { PageNumber = 1, PageSize = 10 };

            _mocks.RestaurantRepo.Setup(r => r.GetAllRestaurants(dto)).ReturnsAsync(paged);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.userImage)).Returns("url/photo.jpg");
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.restaurantImage)).Returns("url/img.jpg");

             
            var result = await _sut.GetAll(dto);

             
            Assert.That(result, Is.Not.Null);
            result.Data.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }

        [Test]
        public async Task GetAll_EmptyList_ReturnsEmptyPagedResult()
        {
             
            var paged = TestDataBuilder.BuildPagedRestaurants(0);
            var dto = new GetGeneralWithPaginationDto<int> { PageNumber = 1, PageSize = 10 };

            _mocks.RestaurantRepo.Setup(r => r.GetAllRestaurants(dto)).ReturnsAsync(paged);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

             
            var result = await _sut.GetAll(dto);

             
            result.Data.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Test]
        public async Task GetAll_MapsProfileAndGalleryUrls()
        {
             
            var paged = TestDataBuilder.BuildPagedRestaurants(1);
            var dto = new GetGeneralWithPaginationDto<int> { PageNumber = 1, PageSize = 10 };
            var photoUrl = "url/photo.jpg";
            var galleryUrl = "url/img.jpg";

            _mocks.RestaurantRepo.Setup(r => r.GetAllRestaurants(dto)).ReturnsAsync(paged);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.userImage)).Returns(photoUrl);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.restaurantImage)).Returns(galleryUrl);

             
            var result = await _sut.GetAll(dto);

             
            result.Data.First().profile.Should().Be(photoUrl);
            result.Data.First().gallery.Should().AllBe(galleryUrl);
        }

        #endregion

        #region GetRestaurant

        [Test]
        public async Task GetRestaurant_ValidId_ReturnsMappedResponse()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();
            var dto = new GetGeneralDto<string> { id = restaurant.Id, lan = "en" };

            _mocks.RestaurantRepo.Setup(r => r.GetByUserIdAsync(dto.id)).ReturnsAsync(restaurant);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(restaurant.ProfilePhoto, MediaType.userImage)).Returns("url/photo.jpg");
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), MediaType.restaurantImage)).Returns("url/img.jpg");

             
            var result = await _sut.GetRestaurant(dto);

             
            Assert.That(result, Is.Not.Null);
            result.profile.Should().Be("url/photo.jpg");
            result.gallery.Should().HaveCount(restaurant.images.Count);
        }

        [Test]
        public async Task GetRestaurant_NotFound_ThrowsNotFoundException()
        {
             
            var dto = new GetGeneralDto<string> { id = "not-exist", lan = "en" };
            _mocks.RestaurantRepo.Setup(r => r.GetByUserIdAsync(dto.id)).ReturnsAsync((Restaurant?)null);

             
            var act = async () => await _sut.GetRestaurant(dto);

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region GetCharts

        [Test]
        public async Task GetCharts_ValidRestaurantId_ReturnsCharts()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();

            _mocks.RestaurantRepo.Setup(r => r.GetByUserIdAsync(restaurant.Id)).ReturnsAsync(restaurant);
            _mocks.OrderRepo.Setup(o => o.CountOrders(restaurant.Id, null)).ReturnsAsync(10);
            _mocks.OrderRepo.Setup(o => o.CountOrders(restaurant.Id, OrderStatus.Delivered)).ReturnsAsync(7);
            _mocks.ReservationRepo.Setup(r => r.CountReservations(restaurant.Id)).ReturnsAsync(5);
            _mocks.OrderRepo.Setup(o => o.TotalAmountOfOrders(restaurant.Id)).ReturnsAsync(1500.0);
            _mocks.OrderRepo.Setup(o => o.GetCollectedPriceByYear(restaurant.Id)).ReturnsAsync(new List<CollectedPerYearDto>());

             
            var result = await _sut.GetCharts(restaurant.Id);

             
            result.numOfOrders.Should().Be(10);
            result.numOfCompletedOrders.Should().Be(7);
            result.numberOfReservations.Should().Be(5);
            result.totalAmount.Should().Be(1500m);
        }

        [Test]
        public async Task GetCharts_RestaurantNotFound_ThrowsNotFoundException()
        {
             
            _mocks.RestaurantRepo.Setup(r => r.GetByUserIdAsync(It.IsAny<string>())).ReturnsAsync((Restaurant?)null);

             
            var act = async () => await _sut.GetCharts("not-exist");

             
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task GetCharts_ReturnsCorrectYearlyData()
        {
             
            var restaurant = TestDataBuilder.BuildRestaurant();

            var yearlyData = new List<CollectedPerYearDto>
{
    new()
    {
        year = 2024,
        months = new List<CollectedPerMonthDto>
        {
            new() { month = 1, amount = 2000 },
            new() { month = 2, amount = 3000 }
        }
    },
    new()
    {
        year = 2025,
        months = new List<CollectedPerMonthDto>
        {
            new() { month = 1, amount = 8000 }
        }
    }
};

            _mocks.RestaurantRepo.Setup(r => r.GetByUserIdAsync(restaurant.Id)).ReturnsAsync(restaurant);
            _mocks.OrderRepo.Setup(o => o.CountOrders(It.IsAny<string>(), It.IsAny<OrderStatus?>())).ReturnsAsync(0);
            _mocks.ReservationRepo.Setup(r => r.CountReservations(It.IsAny<string>())).ReturnsAsync(0);
            _mocks.OrderRepo.Setup(o => o.TotalAmountOfOrders(It.IsAny<string>())).ReturnsAsync(0.0);
            _mocks.OrderRepo.Setup(o => o.GetCollectedPriceByYear(restaurant.Id)).ReturnsAsync(yearlyData);

            var result = await _sut.GetCharts(restaurant.Id);

            result.years.Should().HaveCount(2);
            result.years.Should().BeEquivalentTo(yearlyData);
        }

        #endregion
    }

    public class FakeFormFile : IFormFile
    {
        private readonly string _name;
        public FakeFormFile(string name) => _name = name;

        public string ContentType => "image/jpeg";
        public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{_name}\"";
        public IHeaderDictionary Headers => new HeaderDictionary();
        public long Length => 1024;
        public string Name => "file";
        public string FileName => _name;

        public void CopyTo(Stream target) { }
        public Task CopyToAsync(Stream target, CancellationToken ct = default) => Task.CompletedTask;
        public Stream OpenReadStream() => new MemoryStream(new byte[1024]);
    }
}