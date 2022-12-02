﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sparky
{
    [TestFixture]
    public class CalculatorNUnitTests
    {
        [Test]
        public void AddNumbers_InputTwoInt_GetCorrectAddition()
        {
            //Arrange
            Calculator calc = new();

            //Act
            int result = calc.AddNumbers(10, 20);

            //Assert
            Assert.AreEqual(30, result);
        }

        [Test]
        [TestCase(5.4, 10.5)]//15.9
        [TestCase(5.43, 10.53)]//15.96
        [TestCase(5.49, 10.59)]//16.08
        public void AddDoubleNumbers_InputTwoDouble_GetCorrectAddition(double a, double b)
        {
            //Arrange
            Calculator calc = new();

            //Act
            double result = calc.AddDoubleNumbers(a, b);

            //Assert
            Assert.AreEqual(15.9, result,.2);
        }

        [Test]
        public void IsOddChecker_InputEvenNUmber_ReturnFalse()
        {
            Calculator calculatorcalc = new();

            bool isOdd = calculatorcalc.IsOddNumber(10);

            Assert.That(isOdd, Is.False);
            
            //Another way
            //Assert.IsFalse(isOdd);
        }    
        
        [Test]
        [TestCase(11)]
        [TestCase(13)]
        public void IsOddChecker_InputOddNUmber_ReturnTrue(int a)
        {
            Calculator calculatorcalc = new();

            bool isOdd = calculatorcalc.IsOddNumber(a);

            Assert.That(isOdd, Is.True);
            
            //Another way
            //Assert.IsTrue(isOdd);
        }

        [Test]
        [TestCase(10, ExpectedResult =false)]
        [TestCase(11, ExpectedResult =true)]
        public bool IsOddChecker_InputNUmber_ReturnTrueIfOdd(int a)
        {
            Calculator calculatorcalc = new();

            return calculatorcalc.IsOddNumber(a);

        }
    }
}
