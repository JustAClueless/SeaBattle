using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sea_Battle;

namespace SeaBattleTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestFindPoints()
        {
            var testPalyer = new Player();
            int[] result = null;
            int[] actual = testPalyer.findPoints();
            Assert.AreEqual(result, actual);
        }
    }
}
