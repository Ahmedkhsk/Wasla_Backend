
namespace Wasla_Backend.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;
        private readonly DateTimeHelper _dateTimeHelper;
        private readonly IHubContext<OrderHub> _hub;
        private readonly IFileUrlBuilderService _fileUrlBuilderService;
        private readonly IUserRepository _userRepository;

        public OrderService(
            ICartRepository cartRepo,
            IOrderRepository orderRepo,
            IMapper mapper,
            IPaymentStrategyFactory paymentStrategyFactory,
            DateTimeHelper dateTimeHelper,
            IHubContext<OrderHub> hubContext,
            IFileUrlBuilderService fileUrlBuilderService,
            IUserRepository userRepository)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
            _paymentStrategyFactory = paymentStrategyFactory;
            _dateTimeHelper = dateTimeHelper;
            _hub = hubContext;
            _fileUrlBuilderService = fileUrlBuilderService;
            _userRepository = userRepository;
        }

        public async Task<CheckoutResponse> Checkout(CheckoutDto dto)
        {
            var cart = await _cartRepo.GetCartAsync(dto.residentId, dto.restaurantId);

            if (cart == null || !cart.items.Any())
                throw new NotFoundException(LocalizationKey.CartIsEmpty);

            var invalidItems = cart.items
                .Where(x => x.menuItem.isDeleted)
                .ToList();

            if (invalidItems.Any())
                throw new BadRequestException(LocalizationKey.MenuItemsNotAvailable);

            var order = _mapper.Map<Order>(cart);

            foreach (var item in order.items)
            {
                item.order = order;
            }

            order.notes = dto.notes;
            order.address = dto.address;
            order.deliveryFee = 20;
            order.totalPrice = cart.items.Sum(x => x.price * x.quantity) + order.deliveryFee;
            order.status = OrderStatus.Pending;
            order.paymentMethod = dto.paymentMethod;
            order.createdAt = _dateTimeHelper.Now;

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            var strategy = _paymentStrategyFactory.Create(dto.paymentMethod);

            var result = await strategy.Pay(new PaymentContext
            {
                Amount = order.totalPrice,
                OrderId = order.id,
                UserId = order.residentId,
                ServiceProviderId = order.restaurantId
            });

            order.paymentStatus = result.status;

            if (!string.IsNullOrEmpty(result.paymentUrl))
            {
                return new CheckoutResponse
                {
                    orderId = order.id,
                    paymentKey = result.paymentUrl
                };
            }

            order.status = OrderStatus.Paid;

            _cartRepo.Delete(cart);
            await _cartRepo.SaveChangesAsync();
            await _orderRepo.SaveChangesAsync();

            return new CheckoutResponse
            {
                orderId = order.id
            };
        }
        public async Task StartPreparingOrder(int orderId)
        {
            var order = await _orderRepo.GetOrderDetails(orderId);

            if (order == null)
                throw new NotFoundException(LocalizationKey.OrderNotFound);

            if (order.status != OrderStatus.Paid)
                throw new BadRequestException(LocalizationKey.InvalidOrderStatus);

            order.status = OrderStatus.Preparing;

            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();
            
            await _hub.Clients.Group(order.residentId)
                .SendAsync("OrderStatusChanged", order.id, order.status);

            var prepTime = order.items
                .Max(i => i.menuItem.preparationTime ?? 10);

            BackgroundJob.Schedule<HangfireFunctions>(
                x => x.MarkOrderOnTheWay(order.id),
                TimeSpan.FromMinutes(prepTime)
            );
            var photo= _userRepository.GetUserPhoto(order.restaurantId);
            var RestaurantPhoto =  _fileUrlBuilderService.GetMediaUrl(photo, MediaType.userImage);
            Hangfire.BackgroundJob.Enqueue<NotificationFunction>(x => x.sendNotification(
    order.residentId,
    NotificationType.orderStartedPreparing,
    order.id.ToString(),
    RestaurantPhoto,
    "en",
    null
));
        }

        public async Task MarkOrderDelivered(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            
            if (order == null)
                throw new NotFoundException(LocalizationKey.OrderNotFound);
            
            if (order.status != OrderStatus.OnTheWay)
                throw new BadRequestException(LocalizationKey.InvalidOrderStatus);
            
            order.status = OrderStatus.Delivered;
           
            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();

            await _hub.Clients.Group(order.residentId)
                 .SendAsync("OrderStatusChanged", order.id, order.status);
        }
       
        public async Task<PagedResult<OrderRestaurantResponse>> OrdersRestaurant(GetGeneralWithPaginationDto<string> dto)
        {
            var orders = await _orderRepo.OrdersRestaurent(dto);

            var mapped = orders.Data
                .Select(o => _mapper.Map<OrderRestaurantResponse>(o, opt =>
                {
                    opt.Items["lang"] = dto.lan;
                }))
                .ToList();

            return new PagedResult<OrderRestaurantResponse>
            {
                Data = mapped,
                TotalCount = orders.TotalCount,
                PageNumber = orders.PageNumber,
                PageSize = orders.PageSize
            };
        }

        public async Task<PagedResult<OrderResidentResponse>> OrdersResident(GetGeneralWithPaginationDto<string> dto)
        {
            var orders = await _orderRepo.OrdersResident(dto);

            var mapped = orders.Data
                .Select(o => _mapper.Map<OrderResidentResponse>(o, opt =>
                {
                    opt.Items["lang"] = dto.lan;
                }))
                .ToList();

            return new PagedResult<OrderResidentResponse>
            {
                Data = mapped,
                TotalCount = orders.TotalCount,
                PageNumber = orders.PageNumber,
                PageSize = orders.PageSize
            };
        }
        
    }
}
