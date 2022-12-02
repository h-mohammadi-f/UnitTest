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
            customer = new Customer();
        }

        [Test]
        public void CombineName_InputFirstAndLastName_ReturnFullName()
        {


            //Act
            string fullName = customer.CombineNames("Hooman", "Mohammadi");

            //Assert
            //Assert.AreEqual(fullName, "Hello, Hooman Mohammadi");
            //to test all assertion and then give error we use assert.multiple
            Assert.Multiple(() =>
            {
                Assert.That(fullName, Is.EqualTo("Hello, Hooman Mohammadi"));
                Assert.That(fullName, Does.Contain("hooman").IgnoreCase);
                Assert.That(fullName, Does.StartWith("Hello,"));
                Assert.That(fullName, Does.EndWith("Mohammadi"));
                Assert.That(fullName, Does.Match("Hello, [A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+"));
            });
        }

        [Test]
        public void GreetMessage_NotGreeted_ReturnsNull()
        {

            //act

            //assert

            Assert.IsNull(customer.GreetMessage);
        }

        [Test]
        public void DiscountCheck_DefaultCustomer_ReturnDiscountInRange()
        {
            int result = customer.Discount;

            //discount should be between 15 to 25 %
            Assert.That(result, Is.InRange(10, 25));
        }

        [Test]
        public void GreetMessage_GreetedWithoutLastName_ReturnsNotNull()
        {
            customer.CombineNames("Hooman", "");
            Assert.IsNotNull(customer.GreetMessage);
            Assert.IsFalse(string.IsNullOrEmpty(customer.GreetMessage));
        }

        [Test]
        public void GreetMessage_EmptyFirstName_ThrowException()
        {
            var exceptionDetails = Assert.Throws<ArgumentException>(() =>
            {
                customer.CombineNames("", "Mohammadi");
            });

            Assert.AreEqual("Empty First Name", exceptionDetails.Message);

            //this line conclude 2 above lines in one line
            Assert.That(() => customer.CombineNames("", "Mohammadi"), Throws.ArgumentException.With.Message.EqualTo("Empty First Name"));

            //this line will check this method return an exception and do not care abot messgae
            Assert.Throws<ArgumentException>(() => customer.CombineNames("", "Mohamamdi"));
        }

        [Test]
        public void CustomerType_CreateCustomerWithLessThan100Order_ReturnBasicCustomer()
        {
            customer.OrderTotal = 10;
            var result = customer.GetCustomerType();
            Assert.That(result, Is.TypeOf<BasicCustomer>());
        }    
        
        [Test]
        public void CustomerType_CreateCustomerWithMoreThan100Order_ReturnPlatinumCustomer()
        {
            customer.OrderTotal = 110;
            var result = customer.GetCustomerType();
            Assert.That(result, Is.TypeOf<PlatinumCustomer>());
        }
    }
}
