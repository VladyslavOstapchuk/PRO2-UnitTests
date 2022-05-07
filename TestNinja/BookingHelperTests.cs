using NUnit.Framework;
using TestNinja.Mocking;
using Assert = NUnit.Framework.Assert;

namespace BookingHelperTests
{
    [TestFixture]
    public class BookingHelperTests
    {
        private Booking testBooking;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            testBooking = new Booking()
            {
                Id = 2,
                ArrivalDate = ArriveOn(2017, 1, 15),
                DepartureDate = DepartOn(2017, 1, 20),
                Reference = "asd"
            };

        }

        // if (booking.Status == "Cancelled")
        //        return string.Empty;
        [Test]
        public void BookingsOverlapButNewBookingIsCanceled_ReturnEmptyString()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(testBooking.ArrivalDate),
                DepartureDate = Before(testBooking.DepartureDate),
                Status = "Cancelled"
            });

            // Assert
            Assert.That(result, Is.Empty);
        }

        //bookings.FirstOrDefault(
        //            b =>
        //                booking.ArrivalDate >= b.ArrivalDate
        //                && booking.ArrivalDate<b.DepartureDate
        //                || booking.DepartureDate> b.ArrivalDate
        //                && booking.DepartureDate <= b.DepartureDate);

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before(testBooking.ArrivalDate, days: 2),
                DepartureDate = Before(testBooking.ArrivalDate)
            });

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before(testBooking.ArrivalDate),
                DepartureDate = After(testBooking.ArrivalDate)
            });

            // Assert
            Assert.That(result, Is.EqualTo(testBooking.Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = Before(testBooking.ArrivalDate),
                DepartureDate = After(testBooking.DepartureDate)
            });

            // Assert
            Assert.That(result, Is.EqualTo(testBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(testBooking.ArrivalDate),
                DepartureDate = Before(testBooking.DepartureDate)
            });

            // Assert
            Assert.That(result, Is.EqualTo(testBooking.Reference));
        }

        [Test]
        public void BookingStartsInTheMiddleOfAnExistingBookingButFinishesAfter_ReturnExistingBookingsReference()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(testBooking.ArrivalDate),
                DepartureDate = After(testBooking.DepartureDate)
            });

            // Assert
            Assert.That(result, Is.EqualTo(testBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesAfterAnExistingBooking_ReturnEmptyString()
        {
            // Act
            var result = BookingHelper.OverlappingBookingsExist(new Booking()
            {
                Id = 1,
                ArrivalDate = After(testBooking.DepartureDate),
                DepartureDate = After(testBooking.DepartureDate, days: 10)
            });

            // Assert
            Assert.That(result, Is.Empty);
        }



        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }

        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }

        private DateTime After(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(days);
        }
    }
}
