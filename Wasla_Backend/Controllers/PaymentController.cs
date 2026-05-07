[Route("api/payment")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;

    public PaymentController(IPaymentService paymentService, IConfiguration configuration)
    {
        _paymentService = paymentService;
        _configuration = configuration;
    }


    [HttpPost("create-payment-token")]
    [Authorize(Roles = "resident")]
    public async Task<IActionResult> CreatePaymentToken(CreatePaymentDto dto, string lan = "en")
    {
        var (_, redirectUrl) = await _paymentService.ProcessPaymentAsync(dto);
        return Ok(ResponseHelper.Success(LocalizationKey.PaymentProcessedSuccessfully, lan, redirectUrl));
    }


    [HttpPost("refund")]
    public async Task<IActionResult> Refund(EntityTypeDto dto, string lan = "en")
    {
        var result = await _paymentService.RefundPaymentAsync(dto);
        return Ok(ResponseHelper.Success(LocalizationKey.RefundProcessedSuccessfully, lan, result));
    }


    [HttpGet("callback")]
    [AllowAnonymous]
    public IActionResult Callback()
    {
        var query = Request.Query;
        var frontendUrl = _configuration["Frontend:BaseUrl"];

        if (!PaymobHmacHelper.IsValidClientHmac(query, _configuration["Paymob:HMAC"], query["hmac"].ToString()))
            return Redirect($"{frontendUrl}/payment/failed?reason=invalid_hmac");

        bool.TryParse(query["success"], out bool isSuccess);

        return isSuccess
            ? Redirect($"{frontendUrl}/resident/payment-success")
            : Redirect($"{frontendUrl}/resident/payment-failed");
    }


    [HttpPost("server-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
    {
        try
        {
            if (!payload.TryGetProperty("obj", out var obj))
                return BadRequest();

            if (!PaymobHmacHelper.IsValidServerHmac(obj, _configuration["Paymob:HMAC"], Request.Query["hmac"].ToString()))
                return Unauthorized();

            var transactionId = obj.GetProperty("id").ToString();
            var isSuccess = obj.TryGetProperty("success", out var s) && s.GetBoolean();
            var isRefunded = obj.TryGetProperty("is_refunded", out var r) && r.GetBoolean();
            var paymobOrderId = obj.GetProperty("order").GetProperty("id").ToString();

            await _paymentService.HandlePaymentCallbackByPaymobOrderId(
                paymobOrderId, isSuccess, isRefunded, transactionId);

            return Ok();
        }
        catch
        {
            return StatusCode(500);
        }
    }


    [HttpGet("status")]
    public async Task<IActionResult> GetPaymentStatus([FromQuery] EntityTypeDto dto, string lan = "en")
    {
        var response = await _paymentService.GetPaymentStatusAsync(dto.entityType, dto.entityId);

               return Ok(ResponseHelper.Success(LocalizationKey.PaymentInitializedSuccessfully, lan, response));
    }


    [HttpGet("AllPayment/{residentId}")]
    [Authorize(Roles = "resident")]
    public async Task<IActionResult> GetAllPayment(string residentId, string lan = "en")
    {
        var result = await _paymentService.GetAllPaymentsAsync(residentId);
        return Ok(ResponseHelper.Success(LocalizationKey.PaymentDetailsRetrievedSuccessfully, lan, result));
    }
    [HttpGet("AllPaymentByServiceProvider/{serviceProviderId}")]
    public async Task<IActionResult> GetAllPaymentByServiceProvider(string serviceProviderId, string lan = "en")
    {
        var result = await _paymentService.GetAllPaymentsByServiceProviderAsync(serviceProviderId);
        return Ok(ResponseHelper.Success(LocalizationKey.PaymentDetailsRetrievedSuccessfully, lan, result));
    }

}