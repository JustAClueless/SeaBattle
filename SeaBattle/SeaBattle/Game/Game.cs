using System;
using System.Drawing;

namespace Sea_Battle
{
    public class Game
    {
        private Player player, computer;
        private const int n = 10;
        private Random r = new Random();
        private int playersGoals = 0;
        private int computersGoals = 0;
        Output output;

        string newLine = "\r\n ";
        Color miss = Color.PowderBlue;
        Color ship = Color.Blue;
        Color hit = Color.Black;
        Color empty = Color.White;

        public Game(Output o)
        {
            output = o;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    output.setCellFor1(i, j, "");
                    output.setCellFor1(i, j, "");
                    output.setColor1(i, j, empty);
                    output.setColor2(i, j, empty);
                }
            player = new Player();
            computer = new Player();
            setComputerField();
            output.setText("");
        }

        public void start()
        {
            output.clearSelection();
            output.setText("\r\nВыбирайте следующую точку");
        }

        public void shot(int x, int y)
        {
            State isShootedHere = computer.getHiddenCell(x, y);
            if (isShootedHere.Equals(State.missed) ||
                isShootedHere.Equals(State.killed) ||
                isShootedHere.Equals(State.hurt))
                return;
            computer.shoot(x, y);
            State t = computer.isShipHere(x, y);
            output.clearSelection();
            switch (t)
            {
                case State.empty:
                    output.addText(newLine + Properties.Resources.missPl);
                    output.setColor2(x, y, miss);
                    computer.setHiddenCell(x, y, State.missed);
                    shotByComputer();
                    break;
                case State.hurt: //ранил
                    output.addText(newLine + Properties.Resources.hurtPl + Properties.Resources.choose);
                    output.setColor2(x, y, hit);
                    computer.setHiddenCell(x, y, State.hurt);
                    playersGoals++;
                    break;
                case State.killed: //убил
                    output.addText(newLine + Properties.Resources.killPl + Properties.Resources.choose);
                    playersGoals++;
                    computer.setHiddenCell(x, y, State.killed);
                    computer.toFreeSpaceHidden();
                    display();
                    break;
            }
            isEnd();
        }

        public void shotByComputer()
        {
            State isShootedHere = State.missed;
            int x = 0, y = 0;
            int[] points = player.findPoints();
            if (points == null)
            {
                while (isShootedHere.Equals(State.missed) ||
                    isShootedHere.Equals(State.killed) ||
                    isShootedHere.Equals(State.hurt))
                {
                    x = r.Next(0, n);
                    y = r.Next(0, n);
                    isShootedHere = player.getHiddenCell(x, y);
                }
            }
            else
            {
                x = points[0];
                y = points[1];
            }
            player.shoot(x, y);
            State t = player.isShipHere(x, y);
            switch (t)
            {
                case State.empty: //мимо
                    output.addText(newLine + Properties.Resources.missComputer);
                    output.setColor1(x, y, miss);
                    player.setHiddenCell(x, y, State.missed);
                    break;
                case State.hurt: //ранил
                    output.addText(newLine + Properties.Resources.hurtComp);
                    output.setColor1(x, y, hit);
                    player.setHiddenCell(x, y, State.hurt);
                    computersGoals++;
                    isEnd();
                    shotByComputer();
                    break;
                case State.killed: //убил
                    output.addText(newLine + Properties.Resources.killComputer);
                    output.setColor1(x, y, hit);
                    computersGoals++;
                    player.setHiddenCell(x, y, State.killed);
                    player.toFreeSpaceHidden();
                    display();
                    isEnd();
                    shotByComputer();
                    break;
            }
        }

        public void display()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    //output.setCellFor1(i, j, Convert.ToString(player.getCell(i, j)));
                    //output.setCellFor2(i, j, Convert.ToString(computer.getCell(i, j)));
                    State t = player.getCell(i, j);
                    if (t.Equals(State.empty) || t.Equals(State.forbidden))
                        output.setColor1(i, j, empty);
                    else
                        output.setColor1(i, j, ship);
                    if (player.getHiddenCell(i, j).Equals(State.missed))
                        output.setColor1(i, j, miss);
                    else if (player.getHiddenCell(i, j).Equals(State.killed) || player.getHiddenCell(i, j).Equals(State.hurt))
                        output.setColor1(i, j, hit);


                    t = computer.getHiddenCell(i, j);
                    switch (t)
                    {
                        case State.empty:
                            output.setColor2(i, j, empty);
                            break;
                        case State.missed:
                            output.setColor2(i, j, miss);
                            break;
                        case State.hurt:
                            output.setColor2(i, j, hit);
                            break;
                        case State.killed:
                            output.setColor2(i, j, hit);
                            break;
                    }
                }
        }

        public void placePlayerShip(int x0, int y0, int x, int y)
        {
            player.placeIfCanBe(x0, y0, x, y);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (player.getCell(i, j) > (State)n)
                    {
                        output.setColor1(i, j, ship);
                        output.clearSelection();
                    }
                }
            if (player.isReady())
            {
                output.setText("\r\nКорабли расставлены, игра начинается! Кликнете по вражескому полю, чтобы произвести выстрел.");
            }
            else output.setText(player.toWriteShips());
        }

        private void setComputerField()
        {
            computer.toPlaceShips();
            display();
        }

        public void randomP1()
        {
            player.toPlaceShips();
            start();
            display();
        }

        public void clear()
        {
            player = new Player();
            output.clear(empty);
            output.setText(player.toWriteShips());
        }

        public bool isReadyToPlay()
        {
            return player.isReady();
        }

        public bool isEnd()
        {
            if (computersGoals >= 20)
            {
                output.setResult("                         Поражение!\r\n               Компьютер вас обыграл");
                return true;
            }
            else if (playersGoals >= 20)
            {
                output.setResult("                         Победа!\r\nПоздравлям, вы обыграли компьютер");
                return true;
            }
            else return false;
        }
    }
}
