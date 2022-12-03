using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class ProductNUnitTests
    {
        [Test]
        public void GetProductPrice_PlatinumCustomer_ReturnPriceWith20Discount()
        {
            //because we do not need an interface we cannot use Moq. 
            Product product = new Product() { Price = 50 };
            var result = product.GetPrice(new Customer() { IsPlatinum= true });
            Assert.That(result, Is.EqualTo(40));
        }

        
        //it is not required to write interface only to use MOQ. It is waste of time
        [Test]
        public void GetProductPriceMOQAbuse_PlatinumCustomer_ReturnPriceWith20Discount()
        {
            var customer = new Mock<ICustomer>();
            customer.Setup(u=>u.IsPlatinum).Returns(true);

            Product product = new Product() { Price = 50 };
            var result = product.GetPrice(customer.Object);
            Assert.That(result, Is.EqualTo(40));
        }
    }
}
