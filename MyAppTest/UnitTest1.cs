using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyApp;
namespace MyAppTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SavingsRate_Positive()
        {
            var c = new SavingsCalculate (100, 200);
            System.Console.WriteLine (c.SavingsRate());
            Assert.AreEqual(c.SavingsRate(), 50);
        }

        [TestMethod]
        public void SavingsRate_negative()
        {
            var c = new SavingsCalculate (400, 200);
            System.Console.WriteLine (c.SavingsRate());
            Assert.AreEqual(c.SavingsRate(), -100);
        }

        [TestMethod]
        public void SavingsRate_No_Earnings()
        {
            var c = new SavingsCalculate (1000, 0);
            System.Console.WriteLine (c.SavingsRate());
            Assert.AreEqual(c.SavingsRate(), 0);
        }

        
        [TestMethod]
        public void SavingsRate_No_Values()
        {
            var c = new SavingsCalculate (0, 0);
            System.Console.WriteLine (c.SavingsRate());
            Assert.AreEqual(c.SavingsRate(), 0);
        }

         [TestMethod]
        public void Invalid_Values()
        {
            try {
                var c = new SavingsCalculate (0, -10);
            } catch (Exception e) {
                Assert.AreEqual(e.Message.ToString(), "Expenses and Earnings can not be negative!");
            }
           
        }
    }
}