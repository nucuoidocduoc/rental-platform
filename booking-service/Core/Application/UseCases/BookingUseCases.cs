using booking_service.Core.Application.Interfaces;
using booking_service.Core.Domain.Aggregates;
using booking_service.Core.Domain.Events;
using booking_service.Events;

namespace booking_service.Core.Application.UseCases
{
    public class BookingUseCases : IBookingUseCases
    {
        private readonly IEventRepository _eventStore;
        private readonly IInventoryService _inventoryService;
        private readonly IEventPublisher _eventPublisher;

        public BookingUseCases(
        IInventoryService inventoryService,
        IEventPublisher eventPublisher,
        IEventRepository eventStore)
        {
            _eventStore = eventStore;
            _inventoryService = inventoryService;
            _eventPublisher = eventPublisher;
        }

        public async Task<Booking> CreateBookingAsync(Guid guestId, Guid propertyId, DateTime checkIn, DateTime checkOut, int guests, decimal amount)
        {
            var isAvailable = await _inventoryService.IsAvailable(propertyId, checkIn, checkOut);
            if (!isAvailable)
            {
                throw new InvalidOperationException("Property is not available.");
            }

            // Create Booking Aggregate
            var dateRange = new DateRange(checkIn, checkOut);
            var price = new Price(amount, "usd");
            var booking = new Booking(guestId, propertyId, dateRange,guests, price);

            // Publish BookingCreated event to SQS
            var @event = new BookingCreatedEvent
            {
                BookingId = booking.BookingId,
                GuestId = guestId,
                PropertyId = propertyId,
                Date = new DateRange(checkIn, checkOut),
                Amount = amount,
                Currency = "usd",
                Guests = guests,
                EventName = "BookingCreatedEvent",
            };
            await _eventStore.SaveEventAsync(@event);

            await _eventPublisher.PublishEvent(@event, "property");

            return booking;
        }

        public async Task HandlePaymentProcessedAsync(PaymentCapturedEvent @event)
        {
            var currentVersion = (await _eventStore.GetEventsAsync(@event.PaymentId)).Count();
            var booking = await RebuildBookingAsync(@event.PaymentId);
            var paymentCompletedEvent = new BookingPaymentCompletedEvent
            {
                BookingId = booking.BookingId,
                PropertyId = booking.PropertyId,
                Date = booking.DateRange,
                EventName = "BookingPaymentCompletedEvent",
            };
            await _eventStore.SaveEventAsync(paymentCompletedEvent, currentVersion);
            await _eventPublisher.PublishEvent(paymentCompletedEvent, "property");
        }

        public async Task HandlePropertyBlockedAsync(PropertyBlockedEvent @event)
        {
            var currentVersion = (await _eventStore.GetEventsAsync(@event.BookingId)).Count();
            var booking = await RebuildBookingAsync(@event.BookingId);

            // Publish BookingCompleted event to SQS
            var @reservedEvent = new BookingReservedEvent
            {
                BookingId = booking.BookingId,
                PropertyId = booking.PropertyId,
                EventName = "BookingReservedEvent",
            };
            await _eventStore.SaveEventAsync(@reservedEvent, currentVersion);
            await _eventPublisher.PublishEvent(@reservedEvent, "payment");
        }

        public async Task HandlePropertyConfirmedAsync(PropertyConfirmedEvent @event)
        {
            var currentVersion = (await _eventStore.GetEventsAsync(@event.BookingId)).Count();

            var booking = await RebuildBookingAsync(@event.BookingId);

            var @confirmedEvent = new BookingConfirmedEvent
            {
                BookingId = booking.BookingId,
                PropertyId = booking.PropertyId,
                EventName = "BookingConfirmedEvent",
            };
            booking.Apply(@confirmedEvent);
            await _eventStore.SaveEventAsync(@confirmedEvent, currentVersion);

            var @completedEvent = new BookingCompletedEvent
            {
                BookingId = booking.BookingId,
                GuestId = booking.GuestId,
                PropertyId = booking.PropertyId,
                EventName = "BookingCompletedEvent",
            };
            booking.Complete();
            await _eventStore.SaveEventAsync(@completedEvent, currentVersion);
        }

        private async Task<Booking> RebuildBookingAsync(Guid bookingId)
        {
            var events = await _eventStore.GetEventsAsync(bookingId);
            if (!events.Any())
                return null;

            var booking = new Booking();
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case BookingCreatedEvent created:
                        booking.Apply(created);
                        break;

                    case BookingReservedEvent processed:
                        booking.Apply(processed);
                        break;

                    case BookingPaymentCompletedEvent paymented:
                        booking.Apply(paymented);
                        break;

                    case BookingConfirmedEvent confirmed:
                        booking.Apply(confirmed);
                        break;

                    case BookingCompletedEvent completed:
                        booking.Apply(completed);
                        break;
                }
            }
            return booking;
        }
    }
}