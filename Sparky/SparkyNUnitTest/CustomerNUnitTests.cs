using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class CustomerNUnitTests
    {
        private Customer customer;
        [SetUp]
        public void Setup()
        {
            customer= new Customer();
        }

        [Test]
        public void CombineName_InputFirstAndLastName_ReturnFullName()
        {
 

            //Act
            string fullName = customer.CombineNames("Hooman", "Mohammadi");

            //Assert
            //Assert.AreEqual(fullName, "Hello, Hooman Mohammadi");
            Assert.That(fullName, Is.EqualTo("Hello, Hooman Mohammadi"));
            Assert.That(fullName, Does.Contain("hooman").IgnoreCase);
            Assert.That(fullName, Does.StartWith("Hello,"));
            Assert.That(fullName, Does.EndWith("Mohammadi"));
            Assert.That(fullName, Does.Match("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+"));
        }

        [Test]
        public void GreetMessage_NotGreeted_ReturnsNull()
        {
 
            //act

            //assert

            Assert.IsNull(customer.GreetMessage);
        }
    }
}
