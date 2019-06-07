namespace Sea_Battle
{

    public class Ship
    {
        private int cellsNumber;
        private int hits = 0;
        private int number;

        public Ship(int cells, int n)
        {
            cellsNumber = cells;
            number = n;
        }

        public void damaged()
        {
            hits++;
        }

        public State state()
        {
            if (hits == 0)
                return (State)number;
            if (cellsNumber > hits)
                return State.hurt;
            else return State.killed;
        }
    }
}
