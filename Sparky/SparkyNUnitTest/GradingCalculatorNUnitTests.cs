using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class GradingCalculatorNUnitTests
    {
        private GradingCalculator gradingCalculator;
        [SetUp]
        public void SetUp()
        {
            gradingCalculator = new GradingCalculator();
        }

        [Test]
        public void GetGradeChecker_InputScore95Attendance90_ReturnStringA()
        {
            //Arrange
            gradingCalculator.AttendancePercentage = 90;
            gradingCalculator.Score = 95;

            //Act

            string result = gradingCalculator.GetGrade();

            Assert.That(result, Is.EqualTo("A"));
        }

        [Test]
        public void GetGradeChecker_InputScore85Attendance90_ReturnStringB()
        {
            //Arrange
            gradingCalculator.AttendancePercentage = 90;
            gradingCalculator.Score = 85;

            //Act

            string result = gradingCalculator.GetGrade();

            Assert.That(result, Is.EqualTo("B"));
        }

        [Test]
        public void GetGradeChecker_InputScore65Attendance90_ReturnStringC()
        {
            //Arrange
            gradingCalculator.AttendancePercentage = 90;
            gradingCalculator.Score = 65;

            //Act

            string result = gradingCalculator.GetGrade();

            Assert.That(result, Is.EqualTo("C"));
        }

        [Test]
        public void GetGradeChecker_InputScore95Attendance65_ReturnStringB()
        {
            //Arrange
            gradingCalculator.AttendancePercentage = 65;
            gradingCalculator.Score = 95;

            //Act

            string result = gradingCalculator.GetGrade();

            Assert.That(result, Is.EqualTo("B"));
        }

        [Test]
        [TestCase(95,55, ExpectedResult="F")]
        [TestCase(65,55, ExpectedResult="F")]
        [TestCase(50,90, ExpectedResult="F")]
        [TestCase(95,90, ExpectedResult="A")]
        [TestCase(85,90, ExpectedResult="B")]
        [TestCase(65,90, ExpectedResult="C")]
        [TestCase(95,65, ExpectedResult = "B")]
        public string GetGradeChecker_InputScoreAttendance_ReturnStringF(int score, int attendance)
        {
            //Arrange
            gradingCalculator.AttendancePercentage = attendance;
            gradingCalculator.Score = score;

            //Act

            return gradingCalculator.GetGrade();

        }
    }
}
