using Wasla_Backend.Models.Restaurant;

namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOrderController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public RestaurantOrderController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(AddCartItem dto, [FromQuery] LanDto lanDto)
        {
            await _cartService.AddCart(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.CartItemAddedSuccessfully, lanDto.lan));
        }

        [HttpDelete("remove-from-cart")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] RemoveCartItemDto dto)
        {
            await _cartService.RemoveCartItem(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.CartItemRemovedSuccessfully, dto.lan));
        }

        [HttpPut("quantity-cart-item")]
        public async Task<IActionResult> UpdateCartItem([FromQuery] UpdateQuantityDto dto)
        {
            await _cartService.UpdateQuantity(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.CartItemUpdatedSuccessfully, dto.lan));
        }

        [HttpGet("cart-items")]
        public async Task<IActionResult> GetCartItems([FromQuery] GetCartItems dto)
        {
            var cartItems = await _cartService.GetCartItems(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.CartRetrievedSuccessfully, dto.lan, cartItems));
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutDto dto, [FromQuery] LanDto lanDto)
        {
            var res = await _orderService.Checkout(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.OrderCreatedSuccessfully, lanDto.lan, res));
        }

        [HttpGet("orders-restaurant")]
        public async Task<IActionResult> OrdersRestaurant([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var orders = await _orderService.OrdersRestaurant(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.OrdersRetrievedSuccessfully, dto.lan, orders));
        }

        [HttpGet("orders-resident")]
        public async Task<IActionResult> OrdersResident([FromQuery] GetGeneralWithPaginationDto<string> dto)
        {
            var orders = await _orderService.OrdersResident(dto);
            return Ok(ResponseHelper.Success(LocalizationKey.OrdersRetrievedSuccessfully, dto.lan, orders));
        }
    }
}
