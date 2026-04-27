[Route("api/payment")]
[ApiController]
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
    public async Task<IActionResult> CreatePaymentToken(CreatePaymentDto createPaymentDto, string lan = "en")
    {
        var (paymentResult, redirectUrl) = await _paymentService.ProcessPaymentAsync(createPaymentDto);
        return Ok(ResponseHelper.Success(LocalizationKey.PaymentProcessedSuccessfully, lan, redirectUrl));
    }

    [HttpPost("refund")]
    public async Task<IActionResult> Refund(EntityTypeDto dto, string lan = "en")
    {
        var result = await _paymentService.RefundPaymentAsync(dto);
        return Ok(ResponseHelper.Success(LocalizationKey.RefundProcessedSuccessfully, lan, result));
    }

    [HttpGet("callback")]
    public IActionResult Callback()
    {
        var query = Request.Query;
        string frontendUrl = _configuration["Frontend:BaseUrl"];

        string[] fields = new[]
        {
            "amount_cents","created_at","currency","error_occured","has_parent_transaction",
            "id","integration_id","is_3d_secure","is_auth","is_capture","is_refunded",
            "is_standalone_payment","is_voided","order","owner","pending",
            "source_data.pan","source_data.sub_type","source_data.type","success"
        };

        var concatenated = new StringBuilder();
        foreach (var field in fields)
        {
            if (query.TryGetValue(field, out var value))
                concatenated.Append(value);
            else
                concatenated.Append("");
        }

        string receivedHmac = query["hmac"];
        string secretKey = _configuration["Paymob:HMAC"];
        string calculatedHmac = _paymentService.ComputeHmacSHA512(concatenated.ToString(), secretKey);

        if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
            return Redirect($"{frontendUrl}/payment/failed?reason=invalid_hmac");

        bool.TryParse(query["success"], out bool isSuccess);

        if (isSuccess)
            return Redirect($"{frontendUrl}/resident/payment-success");

        return Redirect($"{frontendUrl}/resident/payment-failed");
    }

    [HttpPost("server-callback")]
    public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
    {
        try
        {
            string receivedHmac = Request.Query["hmac"];
            string secret = _configuration["Paymob:HMAC"];

            if (!payload.TryGetProperty("obj", out var obj))
                return BadRequest();

            string[] fields = new[]
            {
            "amount_cents","created_at","currency","error_occured","has_parent_transaction",
            "id","integration_id","is_3d_secure","is_auth","is_capture","is_refunded",
            "is_standalone_payment","is_voided","order.id","owner","pending",
            "source_data.pan","source_data.sub_type","source_data.type","success"
        };

            var concatenated = new StringBuilder();
            foreach (var field in fields)
            {
                string[] parts = field.Split('.');
                JsonElement current = obj;
                bool found = true;

                foreach (var part in parts)
                {
                    if (current.TryGetProperty(part, out var next))
                        current = next;
                    else
                    {
                        found = false;
                        break;
                    }
                }

                concatenated.Append(found ? current.ToString() : "");
            }

            var calculatedHmac = _paymentService.ComputeHmacSHA512(concatenated.ToString(), secret);

            //if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
            //    return Unauthorized();

            var transactionId = obj.GetProperty("id").ToString();
            var isSuccess = obj.TryGetProperty("success", out var successElement) && successElement.GetBoolean();
            var isRefunded = obj.TryGetProperty("is_refunded", out var refundedElement) && refundedElement.GetBoolean();
            var paymobOrderId = obj.GetProperty("order").GetProperty("id").ToString();

            await _paymentService.HandlePaymentCallbackByPaymobOrderId(
                paymobOrderId, isSuccess, isRefunded, transactionId
            );

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 ERROR IN CALLBACK: {ex.Message}");
            return StatusCode(500);
        }
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetPaymentStatus([FromQuery] EntityTypeDto dto, string lan = "en")
    {
        var payment = await _paymentService.GetPaymentStatusAsync(dto.entityType, dto.entityId);

        var response = new PaymentStatusResponse
        {
            status = payment.Status.ToString(),
            isPaid = payment.Status == PaymentStatus.Completed,
            paymentMethod = payment.PaymentMethod.ToString(),
            amount = payment.Amount,
            paymobTransactionId = payment.PaymobTransactionId
        };

        return Ok(ResponseHelper.Success(LocalizationKey.PaymentInitializedSuccessfully, lan, response));
    }
    [HttpGet("AllPayment/{ResidentId}")]
    public async Task<IActionResult> GetAllPayment(string ResidentId,string lan="en")
    {
        var result = await _paymentService.GetAllPayment(ResidentId);
        return Ok(ResponseHelper.Success(LocalizationKey.PaymentDetailsRetrievedSuccessfully, lan,result ));
    }
}