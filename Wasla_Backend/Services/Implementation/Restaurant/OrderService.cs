namespace Wasla_Backend.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly DateTimeHelper _dateTimeHelper;

        public OrderService(ICartRepository cartRepo, IOrderRepository orderRepo
            ,IMapper mapper,IPaymentService paymentService,DateTimeHelper dateTimeHelper)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
            _paymentService = paymentService;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<CheckoutResponse> Checkout(CheckoutDto dto)
        {
            var cart = await _cartRepo.GetCartAsync(dto.residentId, dto.restaurantId);

            if (cart == null || !cart.items.Any())
                throw new NotFoundException(LocalizationKey.CartIsEmpty);

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

            if (dto.paymentMethod != PaymentMethodType.CashCollection)
            {
                var (_, paymentUrl) = await _paymentService.ProcessPaymentAsync(new CreatePaymentDto
                {
                    Amount = order.totalPrice,
                    entityId = order.id,
                    entityType = EntityType.order,
                    UserId = order.residentId,
                    PaymentMethod = dto.paymentMethod
                });

                return new CheckoutResponse
                {
                    orderId = order.id,
                    paymentKey = paymentUrl
                };
            }

            order.paymentStatus = PaymentStatus.Completed;
            order.status = OrderStatus.Preparing;

            _cartRepo.Delete(cart);

            await _orderRepo.SaveChangesAsync();

            return new CheckoutResponse
            {
                orderId = order.id
            };
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
