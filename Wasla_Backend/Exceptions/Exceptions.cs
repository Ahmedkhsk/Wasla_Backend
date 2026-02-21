using Wasla_Backend.Enums;

namespace Wasla_Backend.Exceptions
{
    public class BadRequestException : Exception
    {
        public LocalizationKey Key { get; }

        public BadRequestException(LocalizationKey key) : base(key.ToString())
        {
            Key = key;
        }
    }

    public class NotFoundException : Exception
    {
        public LocalizationKey Key { get; }

        public NotFoundException(LocalizationKey key) : base(key.ToString())
        {
            Key = key;
        }
    }

    public class UnauthorizedException : Exception
    {
        public LocalizationKey Key { get; }

        public UnauthorizedException(LocalizationKey key) : base(key.ToString())
        {
            Key = key;
        }
    }
}