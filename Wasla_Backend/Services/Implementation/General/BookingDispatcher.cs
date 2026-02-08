namespace Wasla_Backend.Services.Implementation.General
{
    public class BookingDispatcher
    {
        private readonly IEnumerable<IBookingHandler> _bookingHandlers;
        public BookingDispatcher(IEnumerable<IBookingHandler> bookingHandlers)
        {
            _bookingHandlers = bookingHandlers;
        }
       
    }
}
