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
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;

namespace Wasla_Backend.Tests.Services
{

    [TestFixture]
    public class RestaurantCategoryServiceTests
    {
        private MockFactory _mocks;
        private RestaurantCategoryService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();
            _sut = new RestaurantCategoryService(_mocks.RestaurantCategoryRepo.Object);
        }

        #region AddCategory

        [Test]
        public async Task AddCategory_ValidDto_AddsAndSaves()
        {
             
            var dto = new AddResturentCategoryDto
            {
                name = new MultilingualText { English = "Italian", Arabic = "إيطالي" }
            };

            _mocks.RestaurantCategoryRepo.Setup(r => r.AddAsync(It.IsAny<RestaurantCategory>())).Returns(Task.CompletedTask);
            _mocks.RestaurantCategoryRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.AddCategory(dto);

            
            _mocks.RestaurantCategoryRepo.Verify(r => r.AddAsync(It.Is<RestaurantCategory>(c => c.name == dto.name)), Times.Once);
            _mocks.RestaurantCategoryRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateCategory

        [Test]
        public async Task UpdateCategory_ValidDto_UpdatesAndSaves()
        {
             
            var category = TestDataBuilder.BuildCategory();
            var dto = new UpdateResturentCategoryDto
            {
                id = 1,
                name = new MultilingualText { English = "Mexican", Arabic = "مكسيكي" }
            };

            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync(category);
            _mocks.RestaurantCategoryRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.UpdateCategory(dto);

            
            Assert.That(category.name, Is.EqualTo(dto.name));
            _mocks.RestaurantCategoryRepo.Verify(r => r.Update(category), Times.Once);
            _mocks.RestaurantCategoryRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_NotFound_ThrowsNotFoundException()
        {
             
            var dto = new UpdateResturentCategoryDto { id = 99 };
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(dto.id)).ReturnsAsync((RestaurantCategory?)null);

             
            var act = async () => await _sut.UpdateCategory(dto);

            
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region DeleteCategory

        [Test]
        public async Task DeleteCategory_ValidId_DeletesAndSaves()
        {
             
            var category = TestDataBuilder.BuildCategory();
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _mocks.RestaurantCategoryRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.DeleteCategory(1);

            
            _mocks.RestaurantCategoryRepo.Verify(r => r.Delete(category), Times.Once);
            _mocks.RestaurantCategoryRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_NotFound_ThrowsNotFoundException()
        {
             
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((RestaurantCategory?)null);

            var act = async () => await _sut.DeleteCategory(99);

            
            await act.Should().ThrowAsync<NotFoundException>();
        }

        #endregion

        #region GetAll

        [Test]
        public async Task GetAll_ReturnsCorrectText_ForEnglish()
        {
             
            var categories = new List<RestaurantCategory>
            {
                new() { name = new MultilingualText { English = "Italian", Arabic = "إيطالي" } },
                new() { name = new MultilingualText { English = "Mexican", Arabic = "مكسيكي" } },
            };

            _mocks.RestaurantCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _sut.GetAll("en");

            result.Should().HaveCount(2);
            result.Select(r => r.name).Should().Contain("Italian");
            result.Select(r => r.name).Should().Contain("Mexican");
        }

        [Test]
        public async Task GetAll_ReturnsCorrectText_ForArabic()
        {
             
            var categories = new List<RestaurantCategory>
            {
                new() { name = new MultilingualText { English = "Italian", Arabic = "إيطالي" } },
            };

            _mocks.RestaurantCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var result = await _sut.GetAll("ar");

            
            result.First().name.Should().Be("إيطالي");
        }

        [Test]
        public async Task GetAll_EmptyList_ReturnsEmpty()
        {
             
            _mocks.RestaurantCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<RestaurantCategory>());

             
            var result = await _sut.GetAll("en");

            
            result.Should().BeEmpty();
        }

        #endregion
    }
}