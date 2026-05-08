using NUnit.Framework;
using Moq;
using FluentAssertions;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.DTOs.PaginationDTOS;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Enums;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.Helpers.Localization;
using Wasla_Backend.Tests.Helpers;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Microsoft.AspNetCore.Http;
using AutoMapper;


namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class MenuItemServiceTests
    {
        private MockFactory _mocks;
        private MenuItemService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            _sut = new MenuItemService(
                _mocks.MenuItemRepository.Object,
                _mocks.RestaurantRepository.Object,
                _mocks.MenuItemCategoryRepository.Object,
                _mocks.Mapper.Object,
                _mocks.FileUrlBuilderService.Object,
                _mocks.FileService.Object,
                _mocks.UserAuthorizationService.Object

            );
        }

        #region AddItem

        [Test]
        public async Task AddItem_Valid_AddsSuccessfully()
        {
            var dto = TestDataBuilder.BuildAddMenuItemDto();

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync(new Restaurant());

            _mocks.MenuItemCategoryRepository.Setup(x => x.GetByIdAsync(dto.categoryId))
                .ReturnsAsync(new MenuItemCategory());

            _mocks.Mapper.Setup(x => x.Map<MenuItem>(dto))
                .Returns(new MenuItem());

            _mocks.FileService.Setup(x => x.AddFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("img.jpg");

            await _sut.AddItem(dto);

            _mocks.MenuItemRepository.Verify(x => x.AddAsync(It.IsAny<MenuItem>()), Times.Once);
        }

        [Test]
        public void AddItem_RestaurantNotFound_Throws()
        {
            var dto = TestDataBuilder.BuildAddMenuItemDto();

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync((Restaurant)null);

            Func<Task> act = async () => await _sut.AddItem(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void AddItem_CategoryNotFound_Throws()
        {
            var dto = TestDataBuilder.BuildAddMenuItemDto();

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync(new Restaurant());

            _mocks.MenuItemCategoryRepository.Setup(x => x.GetByIdAsync(dto.categoryId))
                .ReturnsAsync((MenuItemCategory)null);

            Func<Task> act = async () => await _sut.AddItem(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region UpdateItem

        [Test]
        public async Task UpdateItem_Valid_UpdatesSuccessfully()
        {
            var dto = new UpdateMenuItemDto
            {
                id = 1,
                imageUrl = new FormFile(
                    new MemoryStream(new byte[1]),
                    0,
                    1,
                    "file",
                    "test.jpg")
            };
            var item = new MenuItem();

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(dto.id))
                .ReturnsAsync(item);

            _mocks.Mapper.Setup(x => x.Map(dto, item));

            _mocks.FileService.Setup(x => x.ReplaceFileAsync(It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("new.jpg");

            await _sut.UpdateItem(dto);

            item.imageUrl.Should().Be("new.jpg");
        }

        [Test]
        public void UpdateItem_NotFound_Throws()
        {
            var dto = TestDataBuilder.BuildUpdateMenuItemDto();

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(dto.id))
                .ReturnsAsync((MenuItem)null);

            Func<Task> act = async () => await _sut.UpdateItem(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region ChangeStatus

        [Test]
        public async Task ChangeStatus_TogglesAvailability()
        {
            var dto = TestDataBuilder.BuildChangeStatusDto();
            var item = new MenuItem { isAvailable = false };

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync(new Restaurant());

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(dto.menuItemId))
                .ReturnsAsync(item);

            await _sut.ChangeStatus(dto);

            item.isAvailable.Should().BeTrue();
        }

        [Test]
        public void ChangeStatus_RestaurantNotFound_Throws()
        {
            var dto = TestDataBuilder.BuildChangeStatusDto();

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync((Restaurant)null);

            Func<Task> act = async () => await _sut.ChangeStatus(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region DeleteItem

        [Test]
        public async Task DeleteItem_SoftDeletes()
        {
            var item = new MenuItem { id = 1 };

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            await _sut.DeleteItem(1);

            item.isDeleted.Should().BeTrue();
        }

        [Test]
        public void DeleteItem_NotFound_Throws()
        {
            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((MenuItem)null);

            Func<Task> act = async () => await _sut.DeleteItem(1);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region GetItemsByRestaurant

        [Test]
        public async Task GetItemsByRestaurant_ReturnsPaged()
        {
            var dto = new GetGeneralWithPaginationDto<string>
            {
                lan = "en"
            };

            var paged = new PagedResult<MenuItem>
            {
                Data = new List<MenuItem>
        {
            new MenuItem
            {
                imageUrl = "img.jpg",
                category = new MenuItemCategory
                {
                    id = 1,
                    name = new MultilingualText
                    {
                        English = "Pizza",
                        Arabic = "بيتزا"
                    }
                }
            }
        },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _mocks.MenuItemRepository
                .Setup(x => x.GetMenuItemsByRestaurantIdAsync(It.IsAny<GetGeneralWithPaginationDto<string>>()))
                .ReturnsAsync(paged);

            _mocks.Mapper
                .Setup(x => x.Map<GetMenuItemDto>(
                    It.IsAny<MenuItem>(),
                    It.IsAny<Action<AutoMapper.IMappingOperationOptions<object, GetMenuItemDto>>>()))
                .Returns(new GetMenuItemDto());

            _mocks.FileUrlBuilderService
                .Setup(x => x.GetMediaUrl(It.IsAny<string>(), MediaType.restaurantImage))
                .Returns("url");

            var result = await _sut.GetMenuItemsByRestaurantIdAsync(dto);

            Assert.That(result, Is.Not.Null);
            result.Data.Should().HaveCount(1);
        }
        #endregion

        #region GetItemsByCategory

        [Test]
        public async Task GetItemsByCategory_ReturnsGrouped()
        {
            var dto = new GetGeneralDto<string>
            {
                lan = "en"
            };

            var category = new MenuItemCategory
            {
                id = 1,
                name = new MultilingualText
                {
                    English = "Pizza",
                    Arabic = "بيتزا"
                }
            };

            var items = new List<MenuItem>
    {
        new MenuItem
        {
            imageUrl = "img.jpg",
            category = category
        }
    };

            _mocks.MenuItemRepository
                .Setup(x => x.GetMenuItemsByRestaurantIdAsync(It.IsAny<GetGeneralDto<string>>()))
                .ReturnsAsync(items);

            _mocks.Mapper
                .Setup(x => x.Map<ItemResponse>(
                    It.IsAny<MenuItem>(),
                    It.IsAny<Action<AutoMapper.IMappingOperationOptions<object, ItemResponse>>>()))
                .Returns(new ItemResponse());

            _mocks.FileUrlBuilderService
                .Setup(x => x.GetMediaUrl(It.IsAny<string>(), MediaType.restaurantImage))
                .Returns("url/img.jpg");

            var result = await _sut.GetMenuItemsByCategoryAsync(dto);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            result.First().categoryId.Should().Be(1);
            result.First().categoryName.Should().Be("Pizza");
            result.First().items.Should().HaveCount(1);
        }

        #endregion

        #region Extra coverage

        [Test]
        public async Task DeleteItem_CallsSaveChanges()
        {
            var item = new MenuItem();

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            await _sut.DeleteItem(1);

            _mocks.MenuItemRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ChangeStatus_CallsUpdate()
        {
            var dto = TestDataBuilder.BuildChangeStatusDto();

            _mocks.RestaurantRepository.Setup(x => x.GetByUserIdAsync(dto.restaurantId))
                .ReturnsAsync(new Restaurant());

            _mocks.MenuItemRepository.Setup(x => x.GetByIdAsync(dto.menuItemId))
                .ReturnsAsync(new MenuItem());

            await _sut.ChangeStatus(dto);

            _mocks.MenuItemRepository.Verify(x => x.Update(It.IsAny<MenuItem>()), Times.Once);
        }

        #endregion
    }
}