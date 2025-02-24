using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace game_of_life2
{
    public partial class FormGame : Form
    {
        class CellFG
        {
            private int aloneTime { set; get; }
            public int[] coords;

            public int AloneTime
            {
                set
                {
                    if (value == 0)
                    {
                        aloneTime = 0;
                    }
                    else
                    {
                        aloneTime++;
                    }
                }

                get { return aloneTime; }
            }

            public CellFG(int aloneTime, int[] coords)
            {
                this.aloneTime = aloneTime;
                this.coords = coords;
            }
        }

        private PictureBox[,] fields;
        private Button timeStart;
        private Label alive;
        private int x;
        private int y;
        private int numOfCells;
        private System.Timers.Timer timer;
        private int possibleAloneTime;
        static Random rnd = new Random();
        static List<CellFG> cells = new List<CellFG>();
        static int time = 0;

        public FormGame(int x, int y, int num, int ticks)
        {
            this.x = x;
            this.y = y;
            this.numOfCells = num;
            this.possibleAloneTime = ticks;
            Text = "Élettér";
            fields = new PictureBox[y, x];
            CreateCell(numOfCells, -2, -2);

            timeStart = new Button
            {
                Width = 70,
                Height = 50,
                Top = 10,
                Left = x * 10 + 15,
                Text = "Idő elindítás",
                Parent = this,
            };
            timeStart.Click += TimeStartStop;

            alive = new Label
            {
                Width = 80,
                Height = 80,
                Top = 120,
                Left = x * 10 + 15,
                Parent = this
            };

            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(RunGameOfLife);
            timer.Interval = 1000;

            MakeMap();
        }

        private void MakeMap()
        {
            this.ClientSize = new Size(x * 10 + 100, y * 10);
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    PictureBox picb = new PictureBox
                    {
                        Width = 10,
                        Height = 10,
                        Top = i * 10,
                        Left = j * 10,
                        BackColor = Color.Black,
                        Parent = this,
                    };

                    fields[i, j] = picb;
                }
            }

            Button newGame = new Button
            {
                Width = 70,
                Height = 50,
                Top = 60,
                Left = x * 10 + 15,
                Text = "Új",
                Parent = this,
            };
            newGame.Click += NewGame;

            this.Visible = true;
        }

        public void CreateCell(int createbleNum, int parentCoordX, int parentCoordY)
        {
            int[][] freeCoords = new int[createbleNum][];
            if (parentCoordX == -2)
            {
                freeCoords = RndCellCoords();
            }
            else
            {     
                List<(int, int)> free = FreeFields(parentCoordX, parentCoordY);
                for (int i = 0; i < createbleNum; i++)
                {
                    int coordX;
                    int coordY;
                    int newCoordId = rnd.Next(0, free.Count);
                    (coordX, coordY) = free[newCoordId];
                    freeCoords[i] = new int[] { coordY, coordX };
                }
            }
            for (int i = 0; i < createbleNum; i++)
            {
                CellFG c = new CellFG(0, freeCoords[i]);
                cells.Add(c);
            }
        }

        private void TurnEvents()
        {
            List<int> deletableInd = new List<int>();
            int needed = 7;
            for (int i = 0; i < cells.Count; i++)
            {
                int numOfFreeFields = FreeFields(cells[i].coords[1], cells[i].coords[0]).Count;
                if (numOfFreeFields == 8)
                {
                    cells[i].AloneTime = 1;
                    if (cells[i].AloneTime > possibleAloneTime)
                    {
                        deletableInd.Add(i);
                    }
                }
                else if (numOfFreeFields == needed)
                {
                    CreateCell(1, cells[i].coords[1], cells[i].coords[0]);
                }
                else if (numOfFreeFields < needed)
                {
                        deletableInd.Add(i);
                }
            }
            for (int i = 0; i < deletableInd.Count; i++)
            {
                int ind = deletableInd[i] - i;
                fields[cells[ind].coords[0], cells[ind].coords[1]].BackColor = Color.Black;
                cells.RemoveAt(ind);
            }
        }

        public void RunGameOfLife(object source, ElapsedEventArgs e)
        {
            time++;
            for (int i = 0; i < cells.Count; i++)
            {
                //move
                int coordX = cells[i].coords[1];
                int coordY = cells[i].coords[0];
                List<(int, int)> free = FreeFields(coordX, coordY);

                fields[coordY, coordX].BackColor = Color.Black;

                int newCoordId = rnd.Next(0, free.Count);
                (coordX, coordY) = free[newCoordId];

                cells[i].coords[1] = coordX;
                cells[i].coords[0] = coordY;
                fields[coordY, coordX].BackColor = Color.White;
            }
            TurnEvents();
        }

        public int[][] RndCellCoords()
        {
            int[] coordX = new int[numOfCells];
            int[] coordY = new int[numOfCells];
            int indX = 0;
            int indY = 0;
            while (indX < coordX.Length - 1 || indY < coordY.Length - 1)
            {
                int xCoord = rnd.Next(0, x);
                int yCoord = rnd.Next(0, y);

                if (indX < coordX.Length - 1)
                {
                    if (!coordX.Contains(xCoord))
                    {
                        coordX[indX] = xCoord;
                        indX++;
                    }
                    else if (coordY[FindIndex(coordX, xCoord)] != yCoord)
                    {
                        coordX[indX] = xCoord;
                        indX++;
                    }
                }

                if (indY < coordY.Length - 1)
                {
                    if (!coordY.Contains(yCoord))
                    {
                        coordY[indY] = yCoord;
                        indY++;
                    }
                    else if (coordX[FindIndex(coordY, yCoord)] != xCoord)
                    {
                        coordY[indY] = yCoord;
                        indY++;
                    }
                }
            }

            int[][] freeCoords = new int[numOfCells][];
            for (int i = 0; i < coordY.Length; i++)
            {
                freeCoords[i] = new int[] { coordY[i], coordX[i] };
            }

            return freeCoords;
        }

        private List<(int, int)> FreeFields(int x, int y)
        {
            List<(int, int)> freeNeighbors = new List<(int, int)>();

            int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = (x + dx[i] + this.x) % this.x;
                int newY = (y + dy[i] + this.y) % this.y;

                if (fields[newY, newX].BackColor == Color.Black)
                    freeNeighbors.Add((newX, newY));
            }

            return freeNeighbors;
        }

        private int FindIndex(int[] list, int prop)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == prop)
                {
                    return i;
                }
            }
            return -1;
        }

        private void TimeStartStop(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                timer.Start();
                timeStart.Text = "Idő megállítás";
            }
            else
            {
                timer.Stop();
                int aliveCnt = 0;
                for (int i = 0; i < fields.GetLength(0); i++)
                {
                    for (int j = 0; j < fields.GetLength(1); j++)
                    {
                        if (fields[i,j].BackColor == Color.White)
                        {
                            aliveCnt++;
                        }
                    }
                }
                double percent = Convert.ToDouble(aliveCnt) / (x * y) * 100;
                timeStart.Text = "Idő elindítás";
                alive.Text = $"Élő sejtek: {aliveCnt},\nami az élettér {percent:f2}%-a\nEltelt idő:\n{time} gen";
            }
        }

        private void NewGame(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
            this.Visible = false;
            cells.Clear();
            time = 0;
            DialogResult = DialogResult.OK;
        }
    }
}