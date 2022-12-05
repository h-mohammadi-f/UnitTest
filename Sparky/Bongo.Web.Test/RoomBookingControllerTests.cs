using Bongo.Core.Services.IServices;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Bongo.Web
{
    [TestFixture]
    public class RoomBookingControllerTests
    {
        private Mock<IStudyRoomBookingService> _studyRoomBookingService;
        private RoomBookingController _roomBookingController;

        [SetUp]
        public void setup()
        {
            _studyRoomBookingService = new Mock<IStudyRoomBookingService>();
            _roomBookingController = new RoomBookingController(_studyRoomBookingService.Object);
        }

        [Test]
        public void IndexPage_CallRequest_VerifyGetAllInvoked()
        {
            _roomBookingController.Index();
            _studyRoomBookingService.Verify(x => x.GetAllBooking(), Times.Once);
        }

        [Test]
        public void BookRoomCheck_ModelStateInvalid_ReturnView()
        {
            _roomBookingController.ModelState.AddModelError("test", "test");
            var result = _roomBookingController.Book(new StudyRoomBooking());

            ViewResult viewResult = result as ViewResult;

            Assert.AreEqual("Book", viewResult.ViewName);
        }

        [Test]
        public void BookRoomCheck_NotSuccessful_NoRoomCode()
        {
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns(new StudyRoomBookingResult()
                {
                    Code = StudyRoomBookingCode.NoRoomAvailable
                });

            var result = _roomBookingController.Book(new StudyRoomBooking());
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("No Study Room available for selected date", viewResult.ViewData["Error"]);
        }

        [Test]
        public void BookRoomCheck_Successful_SuccessCodeAndRedirect()
        {
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns((StudyRoomBooking booking)=> new StudyRoomBookingResult()
                {
                    Code = StudyRoomBookingCode.Success,
                    FirstName=booking.FirstName,
                    LastName=booking.LastName,
                    Date=booking.Date,
                    Email=booking.Email,
                });

            var result = _roomBookingController.Book(new StudyRoomBooking()
            {
                FirstName = "Hooman",
                LastName = "Mohammadi",
                Date = DateTime.Now,
                Email = "test@test.com",
                StudyRoomId=1

            });


            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.AreEqual("Hooman", actionResult.RouteValues["FirstName"]);
            Assert.AreEqual(StudyRoomBookingCode.Success, actionResult.RouteValues["Code"]);
        }
    }
}