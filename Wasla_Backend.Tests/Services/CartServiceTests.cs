using NUnit.Framework;
using Moq;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Enums;
using Wasla_Backend.DTOs.RestaurantDTOS;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Wasla_Backend.Models.Restaurant;
using Wasla_Backend.Helpers.Localization;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class CartServiceTests
    {
        private MockFactory _mocks;
        private CartService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            _sut = new CartService(
                _mocks.CartRepo.Object,
                _mocks.CartItemRepo.Object,
                _mocks.MenuItemRepo.Object,
                _mocks.FileUrlBuilder.Object,
                _mocks.UserAuthorizationService.Object
            );
        }


        #region AddCart

        [Test]
        public async Task AddCart_InvalidQuantity_ThrowsBadRequestException()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 1, quantity = 0 };

             
            var act = async () => await _sut.AddCart(dto);

             
            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.AddCart(dto));
        }

        [Test]
        public async Task AddCart_MenuItemNotFound_ThrowsNotFoundException()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 1, quantity = 2 };

            _mocks.MenuItemRepo.Setup(r => r.GetByIdAsync(dto.menuItemId)).ReturnsAsync((MenuItem?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.AddCart(dto));
        }

        [Test]
        public async Task AddCart_CartFromDifferentRestaurant_ThrowsBadRequestException()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 1, quantity = 2 };

            var menuItem = new MenuItem { id = 1, restaurantId = "rest-002", price = 50 }; 
            var existingCart = new Cart { id = 1, residentId = "res-001", restaurantId = "rest-001" };

            _mocks.MenuItemRepo.Setup(r => r.GetByIdAsync(dto.menuItemId)).ReturnsAsync(menuItem);
            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync(existingCart);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.AddCart(dto));
        }

        [Test]
        public async Task AddCart_NoExistingCart_CreatesNewCartAndAddsItem()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 1, quantity = 2 };
            var menuItem = new MenuItem { id = 1, restaurantId = "rest-001", price = 50 };

            _mocks.MenuItemRepo.Setup(r => r.GetByIdAsync(dto.menuItemId)).ReturnsAsync(menuItem);
            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync((Cart?)null);
            _mocks.CartRepo.Setup(r => r.AddAsync(It.IsAny<Cart>())).Returns(Task.CompletedTask);
            _mocks.CartRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.CartItemRepo.Setup(r => r.AddAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);
            _mocks.CartItemRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.AddCart(dto);

             
            _mocks.CartRepo.Verify(r => r.AddAsync(It.Is<Cart>(c =>
                c.residentId == dto.residentId &&
                c.restaurantId == menuItem.restaurantId
            )), Times.Once);

            _mocks.CartItemRepo.Verify(r => r.AddAsync(It.Is<CartItem>(ci =>
                ci.menuItemId == dto.menuItemId &&
                ci.quantity == dto.quantity &&
                ci.price == menuItem.price
            )), Times.Once);
        }

        [Test]
        public async Task AddCart_ExistingItemInCart_IncrementsQuantity()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 1, quantity = 3 };
            var menuItem = new MenuItem { id = 1, restaurantId = "rest-001", price = 50 };

            var existingCartItem = new CartItem { id = 1, menuItemId = 1, quantity = 2, price = 50 };
            var cart = new Cart
            {
                id = 1,
                residentId = "res-001",
                restaurantId = "rest-001",
                items = new List<CartItem> { existingCartItem }
            };

            _mocks.MenuItemRepo.Setup(r => r.GetByIdAsync(dto.menuItemId)).ReturnsAsync(menuItem);
            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync(cart);
            _mocks.CartItemRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.AddCart(dto);

            Assert.That(existingCartItem.quantity, Is.EqualTo(5));
            _mocks.CartItemRepo.Verify(r => r.Update(existingCartItem), Times.Once);
            _mocks.CartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public async Task AddCart_NewItemInExistingCart_AddsCartItem()
        {
             
            var dto = new AddCartItem { residentId = "res-001", restaurantId = "rest-001", menuItemId = 2, quantity = 1 };
            var menuItem = new MenuItem { id = 2, restaurantId = "rest-001", price = 30 };

            var cart = new Cart
            {
                id = 1,
                residentId = "res-001",
                restaurantId = "rest-001",
                items = new List<CartItem>() // فاضية — مفيش نفس الـ item
            };

            _mocks.MenuItemRepo.Setup(r => r.GetByIdAsync(dto.menuItemId)).ReturnsAsync(menuItem);
            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync(cart);
            _mocks.CartItemRepo.Setup(r => r.AddAsync(It.IsAny<CartItem>())).Returns(Task.CompletedTask);
            _mocks.CartItemRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.AddCart(dto);

             
            _mocks.CartItemRepo.Verify(r => r.AddAsync(It.Is<CartItem>(ci =>
                ci.menuItemId == dto.menuItemId &&
                ci.quantity == dto.quantity &&
                ci.price == menuItem.price
            )), Times.Once);
            _mocks.CartItemRepo.Verify(r => r.Update(It.IsAny<CartItem>()), Times.Never);
        }

        #endregion

   
        #region RemoveCartItem

        [Test]
        public async Task RemoveCartItem_ValidRequest_DeletesAndSaves()
        {
             
            var dto = new RemoveCartItemDto { cartItemId = 1, residentId = "res-001" };
            var cart = new Cart { residentId = "res-001" };
            var item = new CartItem { id = 1, cart = cart };

            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync(item);
            _mocks.CartItemRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.RemoveCartItem(dto);

             
            _mocks.CartItemRepo.Verify(r => r.Delete(item), Times.Once);
            _mocks.CartItemRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveCartItem_ItemNotFound_ThrowsNotFoundException()
        {
             
            var dto = new RemoveCartItemDto { cartItemId = 99, residentId = "res-001" };
            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync((CartItem?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.RemoveCartItem(dto));
        }

        [Test]
        public async Task RemoveCartItem_WrongResident_ThrowsUnauthorizedAccessException()
        {
             
            var dto = new RemoveCartItemDto { cartItemId = 1, residentId = "wrong-res" };
            var cart = new Cart { residentId = "res-001" }; 
            var item = new CartItem { id = 1, cart = cart };

            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync(item);
            _mocks.UserAuthorizationService
    .Setup(x => x.CheckOwnershipByIdAsync(It.IsAny<string>()))
    .ThrowsAsync(new UnauthorizedAccessException());

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.RemoveCartItem(dto));
        }

        #endregion

     
        #region UpdateQuantity

        [Test]
        public async Task UpdateQuantity_ValidRequest_UpdatesAndSaves()
        {
             
            var dto = new UpdateQuantityDto { cartItemId = 1, residentId = "res-001", quantity = 5 };
            var cart = new Cart { residentId = "res-001" };
            var item = new CartItem { id = 1, cart = cart, quantity = 2 };

            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync(item);
            _mocks.CartItemRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

             
            await _sut.UpdateQuantity(dto);

             
            Assert.That(item.quantity, Is.EqualTo(5));
            _mocks.CartItemRepo.Verify(r => r.Update(item), Times.Once);
            _mocks.CartItemRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateQuantity_InvalidQuantity_ThrowsBadRequestException()
        {
             
            var dto = new UpdateQuantityDto { cartItemId = 1, residentId = "res-001", quantity = 0 };

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.UpdateQuantity(dto));
        }

        [Test]
        public async Task UpdateQuantity_ItemNotFound_ThrowsNotFoundException()
        {
             
            var dto = new UpdateQuantityDto { cartItemId = 99, residentId = "res-001", quantity = 2 };
            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync((CartItem?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.UpdateQuantity(dto));
        }

        [Test]
        public async Task UpdateQuantity_WrongResident_ThrowsUnauthorizedAccessException()
        {
             
            var dto = new UpdateQuantityDto { cartItemId = 1, residentId = "wrong-res", quantity = 2 };
            var cart = new Cart { residentId = "res-001" };
            var item = new CartItem { id = 1, cart = cart };

            _mocks.CartItemRepo.Setup(r => r.GetCartItemAsync(dto.cartItemId)).ReturnsAsync(item);
            _mocks.UserAuthorizationService
    .Setup(x => x.CheckOwnershipByIdAsync(It.IsAny<string>()))
    .ThrowsAsync(new UnauthorizedAccessException());

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.UpdateQuantity(dto));
        }

        #endregion

      
        #region GetCartItems

        [Test]
        public async Task GetCartItems_ReturnsCorrectMappedResponse()
        {
             
            var dto = new GetCartItems { residentId = "res-001", restaurantId = "rest-001", lan = "en" };

            var cartItems = new List<CartItem>
            {
                new()
                {
                    id       = 1,
                    quantity = 2,
                    price    = 50,
                    menuItem = new MenuItem
                    {
                        id       = 1,
                        name     = new MultilingualText { English = "Pizza", Arabic = "بيتزا" },
                        imageUrl = "pizza.jpg",
                        category = new MenuItemCategory
                        {
                            name = new MultilingualText { English = "Main", Arabic = "رئيسي" }
                        }
                    }
                }
            };

            _mocks.CartItemRepo.Setup(r => r.GetCartItems(dto)).ReturnsAsync(cartItems);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl("pizza.jpg", MediaType.restaurantImage)).Returns("url/pizza.jpg");

             
            var result = await _sut.GetCartItems(dto);

             
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].menuItemName, Is.EqualTo("Pizza"));
            Assert.That(result[0].totalPrice, Is.EqualTo(100));   // 2 * 50
            Assert.That(result[0].imageUrl, Is.EqualTo("url/pizza.jpg"));
        }

        [Test]
        public async Task GetCartItems_EmptyCart_ReturnsEmptyList()
        {
             
            var dto = new GetCartItems { residentId = "res-001", restaurantId = "rest-001", lan = "en" };
            _mocks.CartItemRepo.Setup(r => r.GetCartItems(dto)).ReturnsAsync(new List<CartItem>());

             
            var result = await _sut.GetCartItems(dto);

             
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCartItems_ReturnsArabicName_WhenLanIsAr()
        {
             
            var dto = new GetCartItems { residentId = "res-001", restaurantId = "rest-001", lan = "ar" };

            var cartItems = new List<CartItem>
            {
                new()
                {
                    id       = 1,
                    quantity = 1,
                    price    = 30,
                    menuItem = new MenuItem
                    {
                        id       = 1,
                        name     = new MultilingualText { English = "Pizza", Arabic = "بيتزا" },
                        imageUrl = "pizza.jpg",
                        category = new MenuItemCategory
                        {
                            name = new MultilingualText { English = "Main", Arabic = "رئيسي" }
                        }
                    }
                }
            };

            _mocks.CartItemRepo.Setup(r => r.GetCartItems(dto)).ReturnsAsync(cartItems);
            _mocks.FileUrlBuilder.Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>())).Returns("url/img.jpg");

             
            var result = await _sut.GetCartItems(dto);

             
            Assert.That(result[0].menuItemName, Is.EqualTo("بيتزا"));
            Assert.That(result[0].menuItemCategoryName, Is.EqualTo("رئيسي"));
        }

        #endregion
    }
}