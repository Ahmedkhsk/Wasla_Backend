namespace Wasla_Backend.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Cart, Order>()
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.items))
                .ForMember(dest => dest.totalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.deliveryFee, opt => opt.Ignore())
                .ForMember(dest => dest.status, opt => opt.Ignore())
                .ForMember(dest => dest.paymentStatus, opt => opt.Ignore())
                .ForMember(dest => dest.createdAt, opt => opt.Ignore());

            CreateMap<CartItem, OrderItem>()
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ForMember(dest => dest.orderId, opt => opt.Ignore());

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.residentName, opt => opt.MapFrom(src => src.resident.FullName))
                .ForMember(dest => dest.restaurantName, opt => opt.MapFrom(src => src.restaurant.FullName))
                .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.items));

            CreateMap<OrderItem, OrderItemsResponse>()
                .ForMember(dest => dest.orderItemId, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.orderItemName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.menuItem.name.GetText(context.Items["lang"].ToString())
                ))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price));
        }
    }
}
