using NUnit.Framework;
using FluentAssertions;
using Moq;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.DTOs.PaginationDTOS;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Helpers.Localization;
using Wasla_Backend.Enums;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;


namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class MenuItemCategoryServiceTests
    {
        private MockFactory _mocks;
        private MenuItemCategoryService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            _sut = new MenuItemCategoryService(
                _mocks.MenuItemCategoryRepo.Object,
                _mocks.RestaurantRepository.Object,
                _mocks.MenuItemRepository.Object,
                _mocks.UserAuthorizationService.Object

            );
        }

        #region AddCategory

        [Test]
        public async Task AddCategory_ValidDto_AddsSuccessfully()
        {
            var dto = new AddMenuItemCategoryDto
            {
                name = new MultilingualText
                {
                    English = "Pizza",
                    Arabic = "بيتزا"
                },
                restaurantId = "r1"
            }; var restaurant = new Restaurant();

            _mocks.RestaurantRepository.Setup(x => x.GetByIdAsync(dto.restaurantId))
                .ReturnsAsync(restaurant);

            _mocks.MenuItemCategoryRepo.Setup(x => x.AddAsync(It.IsAny<MenuItemCategory>()))
                .Returns(Task.CompletedTask);

            _mocks.MenuItemCategoryRepo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _sut.AddCategory(dto);

            _mocks.MenuItemCategoryRepo.Verify(x =>
                x.AddAsync(It.Is<MenuItemCategory>(c =>
                    c.name == dto.name &&
                    c.restaurantId == dto.restaurantId
                )), Times.Once);

            _mocks.MenuItemCategoryRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void AddCategory_RestaurantNotFound_ThrowsNotFound()
        {
            var dto = new AddMenuItemCategoryDto
            {
                name = new MultilingualText
                {
                    English = "Pizza",
                    Arabic = "بيتزا"
                },
                restaurantId = "r1"
            };

            _mocks.RestaurantRepository.Setup(x => x.GetByIdAsync(dto.restaurantId))
                .ReturnsAsync((Restaurant)null);

            Func<Task> act = async () => await _sut.AddCategory(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region UpdateCategory

        [Test]
        public async Task UpdateCategory_Valid_UpdatesSuccessfully()
        {
            var dto = new UpdateMenuItemCategoryDto
            {
                id = 1,
                name = new MultilingualText
                {
                    English = "Updated",
                    Arabic = "محدث"
                }
            };
            var category = new MenuItemCategory
            {
                id = 1,
                name = new MultilingualText
                {
                    English = "Old",
                    Arabic = "قديم"
                }
            };
            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(dto.id))
                .ReturnsAsync(category);

            _mocks.MenuItemCategoryRepo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _sut.UpdateCategory(dto);

            category.name.English.Should().Be(dto.name.English);
            _mocks.MenuItemCategoryRepo.Verify(x => x.Update(category), Times.Once);
        }

        [Test]
        public void UpdateCategory_NotFound_Throws()
        {
            var dto = new UpdateMenuItemCategoryDto { id = 1 };

            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(dto.id))
                .ReturnsAsync((MenuItemCategory)null);

            Func<Task> act = async () => await _sut.UpdateCategory(dto);

            act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region DeleteCategory

        [Test]
        public async Task DeleteCategory_Valid_DeletesSuccessfully()
        {
            var category = new MenuItemCategory { id = 1 };

            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);

            _mocks.MenuItemRepository.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<MenuItem, bool>>>()))
                .ReturnsAsync(false);

            _mocks.MenuItemCategoryRepo.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _sut.DeleteCategory(1);

            _mocks.MenuItemCategoryRepo.Verify(x => x.Delete(category), Times.Once);
        }

        [Test]
        public void DeleteCategory_NotFound_Throws()
        {
            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((MenuItemCategory)null);

            Func<Task> act = async () => await _sut.DeleteCategory(1);

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public void DeleteCategory_HasItems_ThrowsBadRequest()
        {
            var category = new MenuItemCategory { id = 1 };

            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(category);

            _mocks.MenuItemRepository.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<MenuItem, bool>>>()))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _sut.DeleteCategory(1);

            act.Should().ThrowAsync<BadRequestException>();
        }

        #endregion

        #region GetCategories

        [Test]
        public async Task GetCategories_ReturnsList()
        {
            var dto = new GetGeneralDto<string>();
            var list = new List<GetMenuItemCategoryDto>
{
    new()
    {
        name = new MultilingualText
        {
            English = "Pizza",
            Arabic = "بيتزا"
        }
    }
};

            _mocks.MenuItemCategoryRepo.Setup(x => x.GetMenuItemCategory(dto))
                .ReturnsAsync(list);

            var result = await _sut.GetMenuItemCategoryDtos(dto);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        #endregion

        #region Extra edge cases (bonus coverage)

        [Test]
        public async Task AddCategory_SavesOnce()
        {
            var dto = new AddMenuItemCategoryDto
            {
                name = new MultilingualText
                {
                    English = "Pizza",
                    Arabic = "بيتزا"
                },
                restaurantId = "r1"
            };
            _mocks.RestaurantRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Restaurant());

            await _sut.AddCategory(dto);

            _mocks.MenuItemCategoryRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_SavesOnce()
        {
            var dto = new UpdateMenuItemCategoryDto { id = 1 };
            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new MenuItemCategory());

            await _sut.UpdateCategory(dto);

            _mocks.MenuItemCategoryRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_SavesOnce()
        {
            _mocks.MenuItemCategoryRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new MenuItemCategory());

            _mocks.MenuItemRepository.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<MenuItem, bool>>>()))
                .ReturnsAsync(false);

            await _sut.DeleteCategory(1);

            _mocks.MenuItemCategoryRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion
    }
}