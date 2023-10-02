﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class GameModel
    {
        private const byte enemyCount = 2;
        public static readonly int DelayAmount = 400;
        public byte MatrixSize { get; private set; }
        private static readonly Random rnd = new Random();
        public Cell[,] Cells { get; private set; }
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        public uint MineCount{ get;private set;}
        public Direction LookingDirection { get; private set;} = Direction.Right;
        private System.Timers.Timer timer;



        public bool PlayerWon { get => !Player.Dead; }


        public bool Running { get; private set; } = false;

        public GameModel(byte n, uint mineCount, uint enemyCount = 2)
        {
            if (mineCount > n * n - 1 - enemyCount)
            {
                throw new TooManyMinesException("Can't place this many mines (reduce the mine count, reduce the enemy count or increase the map size)");
            }

            this.MineCount = mineCount;
            this.MatrixSize = n;

            NewGame();
        }


        public GameModel(Persistance.SaveGameState saveGameState)
        {
            this.Enemies=saveGameState.Enemies;
            foreach (var enemy in Enemies)
            {
                enemy.SetGame(this);
            }
            this.Player =saveGameState.Player;
            this.Player.SetGame(this);
            this.MatrixSize=saveGameState.MatrixSize;
            this.MineCount=saveGameState.MineCount;
            this.LookingDirection = saveGameState.LookingDirection;
            this.Cells=new Cell[this.MatrixSize,this.MatrixSize];
            for (int i = 0; i < this.MatrixSize; i++)
            {
                for (int j = 0; j < this.MatrixSize; j++)
                {
                    this.Cells[i, j] = saveGameState.Cells[i*this.MatrixSize+j];
                }
            }

        }

        public void NewGame()
        {
            this.Cells = new Cell[this.MatrixSize, this.MatrixSize];
            this.Enemies.Clear();
            this.Player = new Player(this, 0, 0);
            this.Enemies.Add(new Enemy(this, (byte)(this.MatrixSize - 1), 0));
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


            for (int i = 0; i < MineCount; i++)
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
                if (timer!=null&&timer.Enabled)
                    timer.Stop();
            }

        }

        /// <summary>
        /// Run the game on a separate thread
        /// </summary>
        /// <exception cref="AlreadyRunningException">Throws exception if the game is already running</exception>
        public void StartGame()
        {
            if (Running)
            {
                throw new AlreadyRunningException();
            }
            timer = new System.Timers.Timer();

            timer.Interval = DelayAmount;
            timer.Elapsed += Tick;
            timer.Start();

            Running = true;


        }

        public bool IsOver()
        {
            return this.Player.Dead || this.Enemies.Count(x => !x.Dead) == 0;
        }

        public void ChangePlayerDirection(Direction dir)
        {
            this.LookingDirection = dir;
        }

        public async Task SaveGame(string fileName)
        {
           await Persistance.Persistance.SaveStateAsync(fileName,this);
        }

        /// <summary>
        /// Pauses the game should only be used when the game is started with StartGame method otherwise it will do nothing
        /// </summary>
        public void Pause()
        {
            if(Running)
            {
                Running=false;
                if (timer != null)
                {
                    timer.Stop();
                }
            }
        }

        /// <summary>
        /// Resumes the game should only be used when the game was started with StartGame method otherwise it will do nothing
        /// </summary>
        public void Resume()
        {
            if (!Running && timer !=null)
            {
                this.Running = true;
           
                    timer.Start();
                
            }
        }


        private void HandleMovement()
        {
            Player.Move(LookingDirection);

            foreach (var enemy in Enemies.Where(x => !x.Dead))
            {
                enemy.Move(enemy.CalculateMoveDir(Player.Position));
            }

        }

        private bool UpdateCells()
        {
            foreach (var enemy in Enemies)
            {
                Cells[enemy.PrevPosition.Row, enemy.PrevPosition.Col] = Cell.Empty;
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Dead)
                {
                    continue;
                }

                switch (Cells[Enemies[i].Position.Row, Enemies[i].Position.Col])
                {
                    case Cell.Empty:
                        Cells[Enemies[i].Position.Row, Enemies[i].Position.Col] = Cell.Enemy;
                        break;
                    case Cell.Player:
                        this.Player.Die();
                        return true;
                        break;
                    case Cell.Enemy:
                        Cells[Enemies[i].Position.Row, Enemies[i].Position.Col] = Cell.Enemy;
                        break;
                    case Cell.Mine:
                        Enemies[i].Die();
                        //Enemies.RemoveAt(i);
                        //--i;
                        break;
                    default:
                        break;
                }
            }


            if (Cells[Player.Position.Row, Player.Position.Col] == Cell.Empty || Cells[Player.Position.Row, Player.Position.Col] == Cell.Player)
            {
                Cells[Player.PrevPosition.Row, Player.PrevPosition.Col] = Cell.Empty;
                Cells[Player.Position.Row, Player.Position.Col] = Cell.Player;
            }
            else
            {
                this.Player.Die();
                return true;
            }


            return false;
        }

    }
}
