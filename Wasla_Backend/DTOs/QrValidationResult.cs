namespace Wasla_Backend.DTOs
{
    public class QrValidationResult
    {
        public bool IsValid { get; private set; }
        public string Reason { get; private set; }

        private QrValidationResult(bool isValid, string reason = null)
        {
            IsValid = isValid;
            Reason = reason;
        }

        public static QrValidationResult Valid()
            => new(true);

        public static QrValidationResult Invalid(string reason)
            => new(false, reason);
    }

}
