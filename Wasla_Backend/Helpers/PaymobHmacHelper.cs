namespace Wasla_Backend.Helpers
{
    public static class PaymobHmacHelper
    {
        private static readonly string[] ClientCallbackFields = new[]
        {
            "amount_cents","created_at","currency","error_occured","has_parent_transaction",
            "id","integration_id","is_3d_secure","is_auth","is_capture","is_refunded",
            "is_standalone_payment","is_voided","order","owner","pending",
            "source_data.pan","source_data.sub_type","source_data.type","success"
        };

        private static readonly string[] ServerCallbackFields = new[]
        {
            "amount_cents","created_at","currency","error_occured","has_parent_transaction",
            "id","integration_id","is_3d_secure","is_auth","is_capture","is_refunded",
            "is_standalone_payment","is_voided","order.id","owner","pending",
            "source_data.pan","source_data.sub_type","source_data.type","success"
        };

        public static bool IsValidClientHmac(IQueryCollection query, string secret, string receivedHmac)
        {
            var concatenated = BuildFromQuery(query);
            var calculated = ComputeHmacSHA512(concatenated, secret);
            return receivedHmac.Equals(calculated, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsValidServerHmac(JsonElement obj, string secret, string receivedHmac)
        {
            var concatenated = BuildFromJson(obj, ServerCallbackFields);
            var calculated = ComputeHmacSHA512(concatenated, secret);
            return receivedHmac.Equals(calculated, StringComparison.OrdinalIgnoreCase);
        }

        public static string ComputeHmacSHA512(string data, string secret)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            return BitConverter
                .ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(data)))
                .Replace("-", "")
                .ToLower();
        }


        private static string BuildFromQuery(IQueryCollection query)
        {
            var sb = new StringBuilder();
            foreach (var field in ClientCallbackFields)
                sb.Append(query.TryGetValue(field, out var val) ? val.ToString() : "");
            return sb.ToString();
        }

        private static string BuildFromJson(JsonElement obj, string[] fields)
        {
            var sb = new StringBuilder();
            foreach (var field in fields)
            {
                var parts = field.Split('.');
                var current = obj;
                var found = true;

                foreach (var part in parts)
                {
                    if (current.TryGetProperty(part, out var next))
                        current = next;
                    else { found = false; break; }
                }

                if (!found) { sb.Append(""); continue; }

                sb.Append(current.ValueKind == JsonValueKind.True ? "true" :
                          current.ValueKind == JsonValueKind.False ? "false" :
                          current.ToString());
            }
            return sb.ToString();
        }
    }
}