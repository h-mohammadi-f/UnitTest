using Bongo.Core.Services;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Core
{
    [TestFixture]
    public class StudyRoomBookingServiceTests
    {
        private StudyRoomBooking _request;
        private List<StudyRoom> _availableStudyRoom;
        private Mock<IStudyRoomBookingRepository> _studyRoomBookingRepoMock;
        private Mock<IStudyRoomRepository> _studyRoomRepoMock;
        private StudyRoomBookingService _bookingService;

        [SetUp]
        public void Setup()
        {
            _request = new StudyRoomBooking
            {
                FirstName = "Hooman",
                LastName = "Spark",
                Email = "test@test.com",
                Date = new DateTime(2023, 1, 1)
            };

            _availableStudyRoom = new List<StudyRoom>
            {
                new StudyRoom
                {
                    Id=10, RoomName="Johannesburg", RoomNumber="J101"
                }
            };
            _studyRoomBookingRepoMock = new Mock<IStudyRoomBookingRepository>();
            _studyRoomRepoMock = new Mock<IStudyRoomRepository>();
            _studyRoomRepoMock.Setup(r => r.GetAll()).Returns(_availableStudyRoom);
            _bookingService = new StudyRoomBookingService(_studyRoomBookingRepoMock.Object, _studyRoomRepoMock.Object);
        }

        [TestCase]
        public void GetAllBooking_InvokeMethod_CheckIfRepoIsCalled()
        {
            _bookingService.GetAllBooking();
            _studyRoomBookingRepoMock.Verify(x => x.GetAll(null), Times.Once);
        }

        [TestCase]
        public void BookingException_nullRequest_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => _bookingService.BookStudyRoom(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'request')", exception.Message);
            Assert.AreEqual("request", exception.ParamName);
        }
        [Test]
        public void StudyRoomBooking_SaveBookingWithAvailableRoom_ReturnsResultWithAllValues()
        {
            StudyRoomBooking savedStudyRoomBooking = null;
            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    savedStudyRoomBooking = booking;
                });

            //Act
            _bookingService.BookStudyRoom(_request);

            //Assert
            _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Once);
            Assert.NotNull(savedStudyRoomBooking);
            Assert.AreEqual(_request.FirstName, savedStudyRoomBooking.FirstName);
            Assert.AreEqual(_request.LastName, savedStudyRoomBooking.LastName);
            Assert.AreEqual(_request.Email, savedStudyRoomBooking.Email);
            Assert.AreEqual(_request.Date, savedStudyRoomBooking.Date);
            Assert.AreEqual(_availableStudyRoom.First().Id, savedStudyRoomBooking.StudyRoomId);

        }

        [Test]
        public void StudyBookingResultCheck_InputRequest_ValidateMatchInResult()
        {
            StudyRoomBookingResult result = _bookingService.BookStudyRoom(_request);
            Assert.IsNotNull(result);
            Assert.AreEqual(_request.FirstName, result.FirstName);
            Assert.AreEqual(_request.LastName, result.LastName);
            Assert.AreEqual(_request.Email, result.Email);
            Assert.AreEqual(_request.Date, result.Date);
        }

        [Test]
        public void ResultCodeSuccess_RoomAvailable_ReturnSuccessResultCode()
        {
            var result = _bookingService.BookStudyRoom(_request);

            Assert.AreEqual(StudyRoomBookingCode.Success, result.Code);
        }

        [TestCase(true, ExpectedResult = StudyRoomBookingCode.Success)]
        [TestCase(false, ExpectedResult = StudyRoomBookingCode.NoRoomAvailable)]
        public StudyRoomBookingCode ResultCodeSuccess_RoomAvailable_ReturnSuccessResultCode(bool roomAvailable)
        {
            if (!roomAvailable)
            {
                _availableStudyRoom.Clear();
            }
            return _bookingService.BookStudyRoom(_request).Code;
        }

        [TestCase(0, false)]
        [TestCase(55, true)]
        public void StudyRoomBooking_BookRoomWithAvailability_ReturnsBookingId(int expectedBookingId, bool roomAvailability)
        {

            if (!roomAvailability)
            {
                _availableStudyRoom.Clear();
            }

            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    booking.BookingId = 55;
                });
            var result = _bookingService.BookStudyRoom(_request);

            Assert.AreEqual(expectedBookingId, result.BookingId);
        }

        [Test]
        public void BookNotInvoked_SaveBookingWithoutAvailabilityRoom_ReturnsMothodNotInvoked()
        {
            _availableStudyRoom.Clear();
            var result = _bookingService.BookStudyRoom(_request);
            _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Never);

        }

    }
}
