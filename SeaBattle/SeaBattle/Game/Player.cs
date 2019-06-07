using System;

namespace Sea_Battle
{
    public class Player
    {
        private Ship[] ships1 = new Ship[4] { new Ship(1, 11), new Ship(1, 12), new Ship(1, 13), new Ship(1, 14) }; //однопалубные
        private Ship[] ships2 = new Ship[3] { new Ship(2, 21), new Ship(2, 22), new Ship(2, 23) }; //двухпалубные
        private Ship[] ships3 = new Ship[2] { new Ship(3, 31), new Ship(3, 32) }; //трехпалубные
        private Ship[] ships4 = new Ship[1] { new Ship(4, 41) }; //четырёхпалубные

        private State[,] realField = new State[n, n];
        private State[,] hiddenField = new State[n, n];
        private byte[] allShips = new byte[4] { 0, 0, 0, 0 };
        private byte[] max = new byte[4] { 4, 3, 2, 1 };
        private const int n = 10;

        public Player()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    hiddenField[i, j] = State.empty;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    realField[i, j] = State.empty;
        }

        public void toFreeSpaceHidden()
        {
            State[,] field = new State[12, 12];
            for (int i = 0; i < n + 2; i++)
                for (int j = 0; j < n + 2; j++)
                {
                    if (i == 0 || i == 11 || j == 0 || j == 11)
                    {
                        field[i, j] = State.empty;
                    }
                    else
                    {
                        field[i, j] = hiddenField[i - 1, j - 1];
                    }
                }
            for (int i = 1; i < n + 1; i++)
                for(int j = 1; j < n + 1; j++)
                {
                    if (field[i, j].Equals(State.hurt) || field[i, j].Equals(State.killed))
                    {
                        if (WhichShip(i - 1, j - 1).state() == State.killed)
                        {
                            if (field[i - 1, j - 1] == State.empty) field[i - 1, j - 1] = State.missed;
                            if (field[i + 1, j + 1] == State.empty) field[i + 1, j + 1] = State.missed;
                            if (field[i - 1, j + 1] == State.empty) field[i - 1, j + 1] = State.missed;
                            if (field[i + 1, j - 1] == State.empty) field[i + 1, j - 1] = State.missed;
                            if (field[i + 1, j] == State.empty) field[i + 1, j] = State.missed;
                            if (field[i - 1, j] == State.empty) field[i - 1, j] = State.missed;
                            if (field[i, j + 1] == State.empty) field[i, j + 1] = State.missed;
                            if (field[i, j - 1] == State.empty) field[i, j - 1] = State.missed;
                        }
                    }
                }
            for (int i = 0; i < n + 2; i++)
                for (int j = 0; j < n + 2; j++)
                {
                    if (!(i == 0 || i == 11 || j == 0 || j == 11))
                    {
                        hiddenField[i - 1, j - 1] = field[i, j];
                    }
                }
        }

        public int[] findPoints()
        {
            State[,] field = new State[12, 12];
            for (int i = 0; i < n + 2; i++)
                for (int j = 0; j < n + 2; j++)
                {
                    if (i == 0 || i == 11 || j == 0 || j == 11)
                    {
                        field[i, j] = State.empty;
                    }
                    else
                    {
                        field[i, j] = hiddenField[i - 1, j - 1];
                    }
                }
            for (int i = 1; i < n + 1; i++)
                for (int j = 1; j < n + 1; j++)
                {
                    if (hiddenField[i - 1, j - 1] == State.empty)
                        if (field[i + 1, j].Equals(State.hurt) || field[i - 1, j].Equals(State.hurt) || field[i, j + 1].Equals(State.hurt) ||
                            field[i, j - 1].Equals(State.hurt))
                        {
                            return new int[2] { i - 1, j - 1 };
                        }
                }
            return null;
        }

        public void toFreeSpaceReal()
        {
            State[,] field = new State[12, 12];
            for (int i = 0; i < n + 2; i++)
                for (int j = 0; j < n + 2; j++)
                {
                    if (i == 0 || i == 11 || j == 0 || j == 11)
                    {
                        field[i, j] = State.empty;
                    }
                    else
                    {
                        field[i, j] = realField[i - 1, j - 1];
                    }
                }
            for (int i = 1; i < n + 1; i++)
                for (int j = 1; j < n + 1; j++)
                {
                    if (realField[i - 1, j - 1] == State.empty)
                        if ((int)field[i + 1, j] > 10 || (int)field[i - 1, j] > 10 || (int)field[i, j + 1] > 10 ||
                            (int)field[i, j - 1] > 10 || (int)field[i + 1, j + 1] > 10 || (int)field[i - 1, j - 1] > 10 ||
                            (int)field[i + 1, j - 1] > 10 || (int)field[i - 1, j + 1] > 10)
                        {
                            realField[i - 1, j - 1] = State.forbidden;
                        }
                }
        }

        public State getCell(int i, int j)
        {
            return realField[i, j];
        }
        
        public State getHiddenCell(int i, int j)
        {
            return hiddenField[i, j];
        }

        public void setHiddenCell(int i, int j, State r)
        {
            hiddenField[i, j] = r;
        }

        public string toWriteShips()
        {
            string s = "Осталось расставить: \n  Однопалубных - " + (max[0] - allShips[0]) +
                "\n  Двухпалубных - " + (max[1] - allShips[1]) + "\n  Трёхпалубных - " + (max[2] - allShips[2]) +
                "\n  Четырёхпалубных - " + (max[3] - allShips[3]);
            return s;
        }

        public bool isReady()
        {
            bool ready = true;
            for (int i = 0; i < 4; i++)
                if (allShips[i] != max[i])
                    ready = false;
            return ready;
        }

        private ShipOrientation findOrientation(int x0, int y0, int x, int y)
        {
            if (x0 > x)
            {
                int t = x0;
                x0 = x;
                x = t;
            }
            if (y0 > y)
            {
                int t = y0;
                y0 = y;
                y = t;
            }
            int cellsNumber = Math.Max(x - x0, y - y0) + 1;
            return ((x - x0 + 1) == cellsNumber ? ShipOrientation.horizontal : ShipOrientation.vertical);
        }

        private int findCellNumber(int x0, int y0, int x, int y)
        {
            if (x0 > x)
            {
                int t = x0;
                x0 = x;
                x = t;
            }
            if (y0 > y)
            {
                int t = y0;
                y0 = y;
                y = t;
            }
            return Math.Max(x - x0, y - y0) + 1;
        }

        public void placeIfCanBe(int x0, int y0, int x, int y)
        {
            //есть ли такой корабль
            //не соприкасается ли
            if (x0 > x)
            {
                int t = x0;
                x0 = x;
                x = t;
            }
            if (y0 > y)
            {
                int t = y0;
                y0 = y;
                y = t;
            }
            int cellsNumber = findCellNumber(x0, y0, x, y);
            if (cellsNumber > 4)
                return;
            if (x - x0 != 0 && y - y0 != 0) return;
            ShipOrientation orientation = findOrientation(x0, y0, x, y);
            bool aviliable = true;
            if (orientation == 0)
            {
                if (y0 + cellsNumber > n) return;
                for (int i = 0; i < cellsNumber; i++) //проверка на соприкосновение
                {
                    if (realField[x0, y0 + i] > 0)
                    {
                        aviliable = false;
                        break;
                    }
                }
            }
            else
            {
                if (x0 + cellsNumber > n) return;
                for (int i = 0; i < cellsNumber; i++) //проверка на соприкосновение
                {
                    if (realField[x0 + i, y0] > 0)
                    {
                        aviliable = false;
                        break;
                    }
                }
            }
            //проверка на количество кораблей с таким кол-вом клеток
            if (aviliable && (allShips[cellsNumber - 1] + 1 <= max[cellsNumber - 1]))
            {
                allShips[cellsNumber - 1]++;
                toPlace(x0, y0, orientation, cellsNumber, (State)(cellsNumber * n + allShips[cellsNumber - 1]));
            }
        }

        void toPlace(int x, int y, ShipOrientation orientation, int cellsNumber, State sign)
        {
            //размещение корабря
            if (orientation == ShipOrientation.vertical)
            {
                for (int i = 0; i < cellsNumber; i++)
                {
                    realField[x, y + i] = sign;

                }
            }
            else
            {
                for (int i = 0; i < cellsNumber; i++)
                {
                    realField[x + i, y] = sign;
                }
            }
            toFreeSpaceReal();
        }

        public void toPlaceShips()
        {
            randomPlace(1, 1);
            randomPlace(1, 2);
            randomPlace(1, 3);
            randomPlace(1, 4);
            randomPlace(2, 1);
            randomPlace(2, 2);
            randomPlace(2, 3);
            randomPlace(3, 1);
            randomPlace(3, 2);
            randomPlace(4, 1);
            allShips = max;
        }

        public void randomPlace(int cellsNumber, int number)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int x, y;
            ShipOrientation orientation;
            Boolean aviliable;
            //ищем возможное положение
            do
            {
                aviliable = true;
                orientation = (ShipOrientation)r.Next(0, 2);//0-vertical, 1 - horizontal
                if (orientation == 0)
                {
                    x = r.Next(0, n);
                    y = r.Next(0, 11 - cellsNumber);
                    for (int i = 0; i < cellsNumber; i++) //проверка на соприкосновение
                    {
                        if (realField[x, y + i] > 0)
                        {
                            aviliable = false;
                            break;
                        }
                    }
                }
                else
                {
                    x = r.Next(0, 11 - cellsNumber);
                    y = r.Next(0, n);
                    for (int i = 0; i < cellsNumber; i++) //проверка на соприкосновение
                    {
                        if (realField[x + i, y] > 0)
                        {
                            aviliable = false;
                            break;
                        }
                    }
                }
            } while (aviliable != true);
            toPlace(x, y, orientation, cellsNumber, (State)(cellsNumber * n + number));

        }

        public State isShipHere(int i, int j)
        {
            if (WhichShip(i, j) != null)
            {
                return WhichShip(i, j).state();
            }
            else return State.empty;
        
        }

        private Ship WhichShip(int i, int j)
        {
            switch (realField[i, j])
            {
                case State.ship11:
                    return ships1[0];
                case State.ship12:
                    return ships1[1];
                case State.ship13:
                    return ships1[2];
                case State.ship14:
                    return ships1[3];
                case State.ship21:
                    return ships2[0];
                case State.ship22:
                    return ships2[1];
                case State.ship23:
                    return ships2[2];
                case State.ship31:
                    return ships3[0];
                case State.ship32:
                    return ships3[1];
                case State.ship41:
                    return ships4[0];
            }
            return null;
        }

        public void shoot(int i, int j)
        {
            if (WhichShip(i, j) != null)
            {
                WhichShip(i, j).damaged();
            }
        }
    }
}

