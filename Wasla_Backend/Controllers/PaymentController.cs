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
                return BadRequest("Missing 'obj' in payload.");

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
                    if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
                        current = next;
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (!found || current.ValueKind == JsonValueKind.Null)
                    concatenated.Append("");
                else if (current.ValueKind == JsonValueKind.True || current.ValueKind == JsonValueKind.False)
                    concatenated.Append(current.GetBoolean() ? "true" : "false");
                else
                    concatenated.Append(current.ToString());
            }

            string calculatedHmac = _paymentService.ComputeHmacSHA512(concatenated.ToString(), secret);

            if (!receivedHmac.Equals(calculatedHmac, StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Invalid HMAC");

            string paymobTransactionId = null;
            if (obj.TryGetProperty("id", out var idElement))
                paymobTransactionId = idElement.ToString();

            string merchantOrderId = null;
            if (obj.TryGetProperty("order", out var order) &&
                order.TryGetProperty("merchant_order_id", out var merchantOrderIdElement) &&
                merchantOrderIdElement.ValueKind != JsonValueKind.Null)
            {
                merchantOrderId = merchantOrderIdElement.ToString();
            }

            bool isSuccess = obj.TryGetProperty("success", out var successElement) && successElement.GetBoolean();
            bool isRefunded = obj.TryGetProperty("is_refunded", out var refundedElement) && refundedElement.GetBoolean();

            if (!string.IsNullOrEmpty(merchantOrderId))
            {
                await _paymentService.HandlePaymentCallback(merchantOrderId, isSuccess, isRefunded, paymobTransactionId);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing server callback: {ex.Message}");
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