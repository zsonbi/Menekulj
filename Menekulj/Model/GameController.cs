using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class GameController
    {
        private const byte enemyCount = 2;
        private const int delayAmount = 200;
        public byte MatrixSize { get; private set; }
        private static readonly Random rnd = new Random();
        public Cell[,] Cells{ get; private set;}
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        private uint mineCount;
        private Direction lookingDirection = Direction.Right;
        private System.Timers.Timer timer;


        public bool Running { get; private set; } = false;

        public GameController(byte n, uint mineCount, uint enemyCount = 2)
        {
            if (mineCount > n * n - 1 - enemyCount)
            {
                throw new TooManyMinesException("Can't place this many mines (reduce the mine count, reduce the enemy count or increase the map size)");
            }

            this.mineCount = mineCount;
            this.MatrixSize = n;

            NewGame();
        }

        public void NewGame()
        {
            this.Cells = new Cell[this.MatrixSize, this.MatrixSize];
            this.Enemies.Clear();
            this.Player = new Player(this, 0, 0);
            this.Enemies.Add(new Enemy(this,  (byte)(this.MatrixSize - 1),0));
            this.Enemies.Add(new Enemy(this, (byte)(this.MatrixSize - 1), (byte)(this.MatrixSize - 1)));

            List<Position> possibleMineSpots = new List<Position>();

            UpdateCells();

            for (int i = 0; i < this.MatrixSize; i++)
            {
                for (int j = 0; j < this.MatrixSize; j++)
                {
                    if (Cells[i, j] == Cell.Empty)
                    {
                        possibleMineSpots.Add(new Position(i, j));
                    }
                }
            }


            for (int i = 0; i < mineCount; i++)
            {
                int index = rnd.Next(possibleMineSpots.Count);
                Cells[possibleMineSpots[index].Row, possibleMineSpots[index].Col] = Cell.Mine;
            }
        }

        public List<Position> GetMinePositions()
        {
            List<Position> mines = new List<Position>();

            for (byte i = 0; i < MatrixSize; i++)
            {
                for (byte j = 0; j < MatrixSize; j++)
                {
                    if (this.Cells[i, j] == Cell.Mine)
                    {
                        mines.Add(new Position(i, j));
                    }   
                }
            }

            return mines;
        }

        public void Tick(object? sender, EventArgs args)
        {
            HandleMovement();

            UpdateCells();

            if (IsOver())
            {
                this.Running = false;
                timer.Stop();
            }

        }

        public void StartGame()
        {
            if (Running)
            {
                throw new AlreadyRunningException();
            }
            timer = new System.Timers.Timer();

            timer.Interval = 1000;
            timer.Elapsed += Tick;
            timer.Start();
            
            Running = true;
            

        }

        private bool IsOver()
        {
            return this.Player.Dead || this.Enemies.Count(x => !x.Dead) == 0;
        }

        private void HandleMovement()
        {
            Player.Move(lookingDirection);

            foreach (var enemy in Enemies.Where(x=>!x.Dead))
            {
                enemy.Move(enemy.CalculateMoveDir(Player.Position));
            }

        }

        private bool UpdateCells()
        {
            if (Cells[Player.Position.Row, Player.Position.Col] == Cell.Empty || Cells[Player.Position.Row, Player.Position.Col] == Cell.Player)
            {
                Cells[Player.prevPosition.Row, Player.prevPosition.Col] = Cell.Empty;
                Cells[Player.Position.Row, Player.Position.Col] = Cell.Player;
            }
            else
            {
                this.Player.Die();
                return true;
            }

            foreach (var enemy in Enemies)
            {
                Cells[enemy.prevPosition.Row, enemy.prevPosition.Col] = Cell.Empty;
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Dead)
                {
                    continue;
                }

                if (Cells[Enemies[i].Position.Row, Enemies[i].Position.Col] == Cell.Empty)
                {
                    Cells[Enemies[i].Position.Row, Enemies[i].Position.Col] = Cell.Enemy;
                }
                else
                {
                    if (Cells[Enemies[i].Position.Row, Enemies[i].Position.Col] == Cell.Player)
                    {
                        this.Player.Die();
                        return true;
                    }
                    else
                    {
                        Enemies[i].Die();
                        //Enemies.RemoveAt(i);
                        //--i;
                    }
                }
            }
            return false;
        }

    }
}
