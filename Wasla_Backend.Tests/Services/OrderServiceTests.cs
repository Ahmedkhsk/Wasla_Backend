using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models;
using Wasla_Backend.Exceptions;
using Wasla_Backend.Enums;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.DTOs.PaginationDTOS;
using Wasla_Backend.Hubs.RestaurantHubs;
using Wasla_Backend.Strategies.Payment;
using Wasla_Backend.Helpers.Localization;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;
using Wasla_Backend.DTOs.RestaurantDTOS;
using Wasla_Backend.DTOs;
using Wasla_Backend.Models.Restaurant;
using Hangfire.MemoryStorage;
using Hangfire;
using AutoMapper;
using Wasla_Backend.Mappings;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private MockFactory _mocks;
        private OrderService _sut;

        private Mock<IClientProxy> _clientProxyMock;
        private Mock<IHubClients> _hubClientsMock;
        private Mock<IHubContext<OrderHub>> _hubContextMock;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
            });
            var mapper = config.CreateMapper();

            GlobalConfiguration.Configuration.UseMemoryStorage();
            JobStorage.Current = new MemoryStorage();


            
            _clientProxyMock = new Mock<IClientProxy>();
            _clientProxyMock
                .Setup(x => x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _hubClientsMock = new Mock<IHubClients>();

           
            _hubClientsMock
                .Setup(c => c.User(It.IsAny<string>()))
                .Returns(_clientProxyMock.Object);

            _hubClientsMock
                .Setup(c => c.Group(It.IsAny<string>()))
                .Returns(_clientProxyMock.Object);

            _hubClientsMock
                .Setup(c => c.All)
                .Returns(_clientProxyMock.Object);

            _hubContextMock = new Mock<IHubContext<OrderHub>>();
            _hubContextMock
                .Setup(h => h.Clients)
                .Returns(_hubClientsMock.Object);

            _sut = new OrderService(
                _mocks.CartRepo.Object,
                _mocks.OrderRepo.Object,
                mapper,
                _mocks.PaymentStrategyFactory.Object,
                _mocks.DateTimeHelper.Object,
                _hubContextMock.Object,
                _mocks.FileUrlBuilder.Object,
                _mocks.UserRepo.Object,
                _mocks.PaymentService.Object,
                 _mocks.UserAuthorizationService.Object

            );
        }


        #region Checkout

        [Test]
        public async Task Checkout_CartIsNull_ThrowsNotFoundException()
        {
            var dto = new CheckoutDto { residentId = "res-001", restaurantId = "rest-001" };
            _mocks.CartRepo
                .Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId))
                .ReturnsAsync((Cart?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.Checkout(dto));
        }

        [Test]
        public async Task Checkout_CartIsEmpty_ThrowsNotFoundException()
        {
            var dto = new CheckoutDto { residentId = "res-001", restaurantId = "rest-001" };
            var cart = new Cart
            {
                id = 1,
                residentId = dto.residentId,
                restaurantId = dto.restaurantId,
                items = new List<CartItem>()
            };

            _mocks.CartRepo
                .Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId))
                .ReturnsAsync(cart);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.Checkout(dto));
        }

        [Test]
        public async Task Checkout_HasDeletedMenuItems_ThrowsBadRequestException()
        {
            var dto = new CheckoutDto { residentId = "res-001", restaurantId = "rest-001" };
            var cart = new Cart
            {
                id = 1,
                residentId = dto.residentId,
                restaurantId = dto.restaurantId,
                items = new List<CartItem>
                {
                    new() { id = 1, quantity = 1, price = 50,
                            menuItem = new MenuItem { id = 1, isDeleted = true } }
                }
            };

            _mocks.CartRepo
                .Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId))
                .ReturnsAsync(cart);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.Checkout(dto));
        }

        [Test]
        public async Task Checkout_CardPayment_ReturnsPaymentUrl()
        {
            var dto = new CheckoutDto
            {
                residentId = "res-001",
                restaurantId = "rest-001",
                paymentMethod = PaymentMethodType.Card,
            };

            var cart = BuildValidCart(dto.residentId, dto.restaurantId);

            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync(cart);
            _mocks.DateTimeHelper.Setup(d => d.Now).Returns(DateTime.UtcNow);
            _mocks.OrderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var cardStrategy = new Mock<IPaymentStrategy>();
            cardStrategy
                .Setup(s => s.Pay(It.IsAny<PaymentContext>()))
                .ReturnsAsync(new PaymentResult
                {
                    status = PaymentStatus.Pending,
                    paymentUrl = "https://pay.com/checkout"
                });

            _mocks.PaymentStrategyFactory
                .Setup(f => f.Create(PaymentMethodType.Card))
                .Returns(cardStrategy.Object);

            var result = await _sut.Checkout(dto);

            Assert.That(result.paymentKey, Is.EqualTo("https://pay.com/checkout"));
            _mocks.CartRepo.Verify(r => r.Delete(It.IsAny<Cart>()), Times.Never);
        }

        [Test]
        public async Task Checkout_CalculatesTotalPriceCorrectly()
        {
            var dto = new CheckoutDto
            {
                residentId = "res-001",
                restaurantId = "rest-001",
                paymentMethod = PaymentMethodType.Card
            };
            var cart = new Cart
            {
                id = 1,
                residentId = dto.residentId,
                restaurantId = dto.restaurantId,
                items = new List<CartItem>
                {
                    new() { id = 1, quantity = 2, price = 50,
                            menuItem = new MenuItem { isDeleted = false } }, 
                    new() { id = 2, quantity = 1, price = 30,
                            menuItem = new MenuItem { isDeleted = false } },
                }
            };

            Order capturedOrder = null;

            _mocks.CartRepo.Setup(r => r.GetCartAsync(dto.residentId, dto.restaurantId)).ReturnsAsync(cart);
            _mocks.DateTimeHelper.Setup(d => d.Now).Returns(DateTime.UtcNow);
            _mocks.OrderRepo
                .Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o)
                .Returns(Task.CompletedTask);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.CartRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var cashStrategy = new Mock<IPaymentStrategy>();
            cashStrategy
                .Setup(s => s.Pay(It.IsAny<PaymentContext>()))
                .ReturnsAsync(new PaymentResult { status = PaymentStatus.Completed });
            _mocks.PaymentStrategyFactory
                .Setup(f => f.Create(PaymentMethodType.Card))
                .Returns(cashStrategy.Object);

            await _sut.Checkout(dto);

            Assert.That(capturedOrder.totalPrice, Is.EqualTo(150));
        }

        #endregion


        #region StartPreparingOrder

        [Test]
        public async Task StartPreparingOrder_OrderNotFound_ThrowsNotFoundException()
        {
            _mocks.OrderRepo
                .Setup(r => r.GetOrderDetails(It.IsAny<int>()))
                .ReturnsAsync((Order?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.StartPreparingOrder(1));
        }

        [Test]
        public async Task StartPreparingOrder_OrderNotPaid_ThrowsBadRequestException()
        {
            var order = new Order { id = 1, status = OrderStatus.Pending };

            _mocks.OrderRepo.Setup(r => r.GetOrderDetails(1)).ReturnsAsync(order);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.StartPreparingOrder(1));
        }

        [Test]
        public async Task StartPreparingOrder_ValidOrder_ChangesStatusToPreparingAndNotifies()

        {
            var order = new Order
            {
                id = 1,
                status = OrderStatus.Paid,
                residentId = "res-001",
                restaurantId = "rest-001",
                items = new List<OrderItem>
                {
                    new() { menuItem = new MenuItem { preparationTime = 15 } }
                }
            };

            _mocks.OrderRepo.Setup(r => r.GetOrderDetails(1)).ReturnsAsync(order);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.UserRepo.Setup(r => r.GetUserPhoto(order.restaurantId)).Returns("photo.jpg");
            _mocks.FileUrlBuilder
                .Setup(f => f.GetMediaUrl("photo.jpg", MediaType.userImage))
                .Returns("url/photo.jpg");

            await _sut.StartPreparingOrder(1);

            Assert.That(order.status, Is.EqualTo(OrderStatus.Preparing));
            _mocks.OrderRepo.Verify(r => r.Update(order), Times.Once);
            _mocks.OrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>()),
                Times.AtLeastOnce);
        }

        #endregion

      
        #region MarkOrderDelivered

        [Test]
        public async Task MarkOrderDelivered_OrderNotFound_ThrowsNotFoundException()
        {
            _mocks.OrderRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Order?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.MarkOrderDelivered(1));
        }

        [Test]
        public async Task MarkOrderDelivered_OrderNotOnTheWay_ThrowsBadRequestException()
        {
            var order = new Order { id = 1, status = OrderStatus.Preparing };
            _mocks.OrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.MarkOrderDelivered(1));
        }

        [Test]
        public async Task MarkOrderDelivered_ValidOrder_ChangesStatusAndNotifies()
        {
            var order = new Order
            {
                id = 1,
                status = OrderStatus.OnTheWay,
                residentId = "res-001",
                restaurantId = "rest-001"
            };

            _mocks.OrderRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _sut.MarkOrderDelivered(1);

            Assert.That(order.status, Is.EqualTo(OrderStatus.Delivered));
            _mocks.OrderRepo.Verify(r => r.Update(order), Times.Once);
            _mocks.OrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>()),
                Times.AtLeastOnce);
        }

        #endregion

       
        #region CancelOrder

        [Test]
        public async Task CancelOrder_OrderNotFound_ThrowsNotFoundException()
        {
            var dto = new CancleOrderDto { orderId = 99 };
            _mocks.OrderRepo
                .Setup(r => r.GetOrderWithIncludeUsers(dto.orderId))
                .ReturnsAsync((Order?)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _sut.CancelOrder(dto));
        }

        [Test]
        public async Task CancelOrder_InvalidStatus_ThrowsBadRequestException()
        {
            var dto = new CancleOrderDto { orderId = 1 };
            var order = new Order { id = 1, status = OrderStatus.Delivered };

            _mocks.OrderRepo
                .Setup(r => r.GetOrderWithIncludeUsers(dto.orderId))
                .ReturnsAsync(order);

            Assert.ThrowsAsync<BadRequestException>(async () => await _sut.CancelOrder(dto));
        }

        [Test]
        public async Task CancelOrder_ValidOrder_ChangeStatusToCancelled()
        {
            var dto = new CancleOrderDto { orderId = 1, isResident = true };
            var order = new Order
            {
                id = 1,
                status = OrderStatus.Pending,
                residentId = "res-001",
                restaurantId = "rest-001",
                paymentStatus = PaymentStatus.Pending,
                paymentMethod = PaymentMethodType.Card,
                resident = new Resident { FullName = "Test User", ProfilePhoto = "res.jpg" },
                restaurant = new Restaurant { FullName = "Test Rest", ProfilePhoto = "rest.jpg" },
            };

            _mocks.OrderRepo.Setup(r => r.GetOrderWithIncludeUsers(dto.orderId)).ReturnsAsync(order);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder
                .Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>()))
                .Returns("url/img.jpg");

            await _sut.CancelOrder(dto);

            Assert.That(order.status, Is.EqualTo(OrderStatus.Cancelled));
            _mocks.OrderRepo.Verify(r => r.Update(order), Times.Once);
            _mocks.OrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CancelOrder_PaidByCard_TriggersRefund()
        {
            var dto = new CancleOrderDto { orderId = 1, isResident = true };
            var order = new Order
            {
                id = 1,
                status = OrderStatus.Paid,
                residentId = "res-001",
                restaurantId = "rest-001",
                paymentStatus = PaymentStatus.Completed,
                paymentMethod = PaymentMethodType.Card,   // Card + Completed → Refund
                resident = new Resident { FullName = "Test", ProfilePhoto = "res.jpg" },
                restaurant = new Restaurant { FullName = "Rest", ProfilePhoto = "rest.jpg" },
            };

            _mocks.OrderRepo.Setup(r => r.GetOrderWithIncludeUsers(dto.orderId)).ReturnsAsync(order);
            _mocks.OrderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mocks.FileUrlBuilder
                .Setup(f => f.GetMediaUrl(It.IsAny<string>(), It.IsAny<MediaType>()))
                .Returns("url/img.jpg");
            _mocks.PaymentService
                .Setup(p => p.RefundPaymentAsync(It.IsAny<EntityTypeDto>()))
                .ReturnsAsync(true);

            await _sut.CancelOrder(dto);

            _mocks.PaymentService.Verify(p => p.RefundPaymentAsync(It.Is<EntityTypeDto>(e =>
                e.entityId == order.id &&
                e.entityType == EntityType.order
            )), Times.Once);
        }

        #endregion


        #region GetOrders

        [Test]
        public async Task OrdersRestaurant_ReturnsPagedMappedResult()
        {
            var dto = new GetGeneralWithPaginationDto<string>
            {
                id = "rest-001",
                PageNumber = 1,
                PageSize = 10,
                lan = "en"
            };
            var pagedOrders = new PagedResult<Order>
            {
                Data = new List<Order> { new() { id = 1, status = OrderStatus.Pending } },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _mocks.OrderRepo.Setup(r => r.OrdersRestaurent(dto)).ReturnsAsync(pagedOrders);

            var result = await _sut.OrdersRestaurant(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(1));
            Assert.That(result.TotalCount, Is.EqualTo(1));
        }

        [Test]
        public async Task OrdersResident_ReturnsPagedMappedResult()
        {
            var dto = new GetGeneralWithPaginationDto<string>
            {
                id = "res-001",
                PageNumber = 1,
                PageSize = 10,
                lan = "en"
            };
            var pagedOrders = new PagedResult<Order>
            {
                Data = new List<Order> { new() { id = 1, status = OrderStatus.Delivered } },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 1
            };

            _mocks.OrderRepo.Setup(r => r.OrdersResident(dto)).ReturnsAsync(pagedOrders);

            var result = await _sut.OrdersResident(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(1));
        }

        #endregion

        private static Cart BuildValidCart(string residentId, string restaurantId) => new()
        {
            id = 1,
            residentId = residentId,
            restaurantId = restaurantId,
            items = new List<CartItem>
            {
                new() { id = 1, quantity = 2, price = 50,
                        menuItem = new MenuItem { id = 1, isDeleted = false } },
            }
        };
    }
}