public class HangfireFunctions
{
    private readonly Context _db;
    private readonly IHubContext<OrderHub> _hubOrder;
    private readonly DateTimeHelper _dateTimeHelper;
    private readonly IHubContext<BookingHub> _hubBooking;

    public HangfireFunctions(Context db, IHubContext<BookingHub> hubBooking, IHubContext<OrderHub> hubOrder,
        DateTimeHelper dateTimeHelper)
    {
        _db = db;
        _hubOrder = hubOrder;
        _dateTimeHelper = dateTimeHelper;
        _hubBooking = hubBooking;
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
            residentId = booking.ResidentId,
            serviceProviderId = booking.serviceProviderId
        };

        await _hubBooking.Clients.All.SendAsync("BookingCompleted", hubData);

    }

    public async Task MarkOrderOnTheWay(int orderId)
    {
        var order = await _db.Orders
            .Include(o => o.items)
            .ThenInclude(i => i.menuItem)
            .FirstOrDefaultAsync(o => o.id == orderId);

        if (order == null || order.status != OrderStatus.Preparing)
            return;

        order.status = OrderStatus.OnTheWay;

        await _db.SaveChangesAsync();

        await _hubOrder.Clients.Group(order.residentId)
            .SendAsync("OrderStatusChanged", order.id, order.status);
    }

    public async Task CheckReservationsStatus()
    {
        var now = _dateTimeHelper.Now;
        
        var nowDate = DateOnly.FromDateTime(now);
        var nowTime = TimeOnly.FromDateTime(now);

        await _db.Reservations
            .Where(r => r.status == Status.Pending &&
                (r.reservationDate < nowDate ||
                (r.reservationDate == nowDate && r.reservationTime <= nowTime)))
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.status, Status.Canceled));

        await _db.Reservations
            .Where(r => r.status == Status.Accepted &&
                (r.reservationDate < nowDate ||
                (r.reservationDate == nowDate && r.reservationTime <= nowTime)))
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.status, Status.Completed));

        await _db.SaveChangesAsync();
    }
}
