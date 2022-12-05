using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.DataAccess
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {

        private StudyRoomBooking studyRoomBooking_One;
        private StudyRoomBooking studyRoomBooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        public StudyRoomBookingRepositoryTests()
        {
            studyRoomBooking_One = new StudyRoomBooking()
            {
                FirstName = "Test1",
                LastName = "TestFamily1",
                Date = new DateTime(2023, 1, 1),
                Email = "test1@test.com",
                BookingId = 11,
                StudyRoomId = 1
            };

            studyRoomBooking_Two = new StudyRoomBooking()
            {
                FirstName = "Test2",
                LastName = "TestFamily2",
                Date = new DateTime(2023, 2, 2),
                Email = "test1@test.com",
                BookingId = 22,
                StudyRoomId = 2
            };
        }

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
        }


        [Test]
        [Order(0)]
        public void SaveBooking_Booking_One_CheckTheValuesFromDatabase()
        {
            //arrange
            //we can move this options in setup method as well.
            // var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            // .UseInMemoryDatabase(databaseName:"temp_Bongo").Options;

            //act
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);
                Assert.That(bookingFromDb.BookingId, Is.EqualTo(studyRoomBooking_One.BookingId));
                Assert.That(bookingFromDb.FirstName, Is.EqualTo(studyRoomBooking_One.FirstName));
                Assert.That(bookingFromDb.LastName, Is.EqualTo(studyRoomBooking_One.LastName));
                Assert.That(bookingFromDb.Email, Is.EqualTo(studyRoomBooking_One.Email));
                Assert.That(bookingFromDb.Date, Is.EqualTo(studyRoomBooking_One.Date));
            }

        }

        [Test]
        [Order(1)]
        public void GetAllBooking_BookingOneAndTwo_CheckGetBothFromDatabase()
        {
            //arrange
            var expectedResult = new List<StudyRoomBooking> { studyRoomBooking_One, studyRoomBooking_Two };
            // var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //   .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;

            using (var context = new ApplicationDbContext(options))
            {
                //Because we have 2 test that using temp_bengo, if we run all tests we will have get error. 
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
                repository.Book(studyRoomBooking_Two);
            }

            //act
            List<StudyRoomBooking> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                actualList = repository.GetAll(null).ToList();
            }

            //assert
            CollectionAssert.AreEqual(expectedResult, actualList, new BookingComapre());
        }

        private class BookingComapre : IComparer
        {
            public int Compare(object x, object y)
            {
                var booking1 = (StudyRoomBooking)x;
                var booking2 = (StudyRoomBooking)y;

                if (booking1.BookingId != booking2.BookingId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

    }
}
