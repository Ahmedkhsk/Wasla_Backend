using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class HangfireFunctions
{
    private readonly Context _db;
    private readonly IHubContext<BookingHub> _hub;

    public HangfireFunctions(Context db, IHubContext<BookingHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    public async Task CompleteBookingAsync(int bookingId)
    {
        var booking = await _db.Booking
            .Include(b => b.serviceDay)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null || booking.bookingStatus == BookingStatus.completed)
            return;

        booking.bookingStatus = BookingStatus.completed;
        booking.serviceDay.isBooking = false;

        await _db.SaveChangesAsync();

        var hubData = new BookHubData
        {
            serviceId = booking.serviceDayId,
            residentId = booking.userId,
            serviceProviderId = booking.serviceProviderId
        };

        await _hub.Clients.All.SendAsync("BookingCompleted", hubData);

    }
}
