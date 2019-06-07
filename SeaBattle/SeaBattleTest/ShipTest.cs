using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sea_Battle;

namespace SeaBattleTest
{
    [TestClass]
    public class ShipTest
    {
        [TestMethod]
        public void TestStateKilled()
        {
            int cellsNumber = 4,
                number = 2;
            var testShip = new Ship(cellsNumber, number);
            State result = State.killed;
            State actual = testShip.state();
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        public void TestStateHurt()
        {
            int cellsNumber = 4,
                number = 3;
            var testShip = new Ship(cellsNumber, number);
            State result = State.hurt;
            State actual = testShip.state();
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        public void TestState()
        {
            int cellsNumber = 4,
                number = 2;
            var testShip = new Ship(cellsNumber, number);
            State result = (State)number;
            State actual = testShip.state();
            Assert.AreEqual(result, actual);
        }
    }
}
