namespace Wasla_Backend.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOrderController : ControllerBase
    {
        private readonly ICartService _cartService;

        public RestaurantOrderController(ICartService cartService)
        {
            _cartService = cartService;
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
    }
}
