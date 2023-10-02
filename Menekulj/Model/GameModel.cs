using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class GameModel
    {

        public static readonly int GameSpeed = 400; //Millis for a move to happen
        private static readonly Random rnd = new Random(); //Random for the mine spawning
        private System.Timers.Timer timer; //Timer for the game on an additional thread (Deprecated)
        
        /// <summary>
        /// The number of mines in the game
        /// </summary>
        public uint MineCount { get; private set; }
        /// <summary>
        /// The cells of the game
        /// </summary>
        public Cell[,] Cells { get; private set; }
        /// <summary>
        /// The player
        /// </summary>
        public Player Player { get; private set; }
        /// <summary>
        /// Enemies (the ones which are dead does not move or need to be shown)
        /// </summary>
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        /// <summary>
        /// Get if the player won (Should only be checked if we're certain that the game is over)
        /// </summary>
        public bool PlayerWon { get => !Player.Dead; }
        /// <summary>
        /// The size of the board
        /// </summary>
        public byte MatrixSize { get; private set; }
        /// <summary>
        /// Get if the game is running (only checks if it was started with StartGame) (Deprecated)
        /// </summary>
        public bool Running { get; private set; } = false;

        /// <summary>
        /// Creates a new game model
        /// </summary>
        /// <param name="mapSize">The size of the map it will be (mapSize x mapSize)</param>
        /// <param name="mineCount">The number of mines to spawn</param>
        /// <param name="enemyCount"></param>
        /// <exception cref="TooManyMinesException"></exception>
        public GameModel(byte mapSize, uint mineCount)
        {
            if (mineCount > mapSize * mapSize - 3)
            {
                throw new TooManyMinesException("Can't place this many mines (reduce the mine count, reduce the enemy count or increase the map size)");
            }

            this.MineCount = mineCount;
            this.MatrixSize = mapSize;

            CreateGameBoard();
        }

        /// <summary>
        /// Loads a game model from a saved state
        /// </summary>
        /// <param name="saveGameState">The saved state</param>
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
            this.Cells=new Cell[this.MatrixSize,this.MatrixSize];
            //Convert the 1d array into a 2d one
            for (int i = 0; i < this.MatrixSize; i++)
            {
                for (int j = 0; j < this.MatrixSize; j++)
                {
                    this.Cells[i, j] = saveGameState.Cells[i*this.MatrixSize+j];
                }
            }

        }

        /// <summary>
        /// Creates the game's objects and places them at the correct positions
        /// </summary>
        public void CreateGameBoard()
        {
            //If it was called again
            this.Enemies.Clear();

            this.Cells = new Cell[this.MatrixSize, this.MatrixSize];
            this.Player = new Player(this, 0, 0);
            this.Enemies.Add(new Enemy(this, (byte)(this.MatrixSize - 1), 0));
            this.Enemies.Add(new Enemy(this, (byte)(this.MatrixSize - 1), (byte)(this.MatrixSize - 1)));

            //Put the player and the enemies into the cells 2d array
            UpdateCells();

            //Get where it is possible to put mines so we don't get into a near infinite loop or place it directly on the player or enemy
            List<Position> possibleMineSpots = new List<Position>();
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

            //Place the mines randomly
            for (int i = 0; i < MineCount; i++)
            {
                int index = rnd.Next(possibleMineSpots.Count);
                Cells[possibleMineSpots[index].Row, possibleMineSpots[index].Col] = Cell.Mine;
            }
        }

        /// <summary>
        /// Get the mine positions
        /// </summary>
        /// <returns>A list of the positions</returns>
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

        /// <summary>
        /// A single tick of the game move every entity once
        /// </summary>
        /// <param name="sender">The object which triggered this event</param>
        /// <param name="args">What was the event</param>
        public void Tick(object? sender, EventArgs args)
        {
            HandleMovement();

            UpdateCells();

            if (IsOver())
            {
                this.Running = false;
                //If the game was ran by the StartGame() method 
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
            timer.Interval = GameSpeed;
            timer.Elapsed += Tick;
            timer.Start();
            Running = true;
        }

        /// <summary>
        /// Check if the game is over
        /// </summary>
        /// <returns>true - one side has won, false - the game is still going</returns>
        public bool IsOver()
        {
            return this.Player.Dead || this.Enemies.Count(x => !x.Dead) == 0;
        }

        public void ChangePlayerDirection(Direction dir)
        {
            this.Player.SetDirection ( dir);
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

        /// <summary>
        /// Moves every entity on the board by one
        /// </summary>
        private void HandleMovement()
        {
            Player.Move();

            foreach (var enemy in Enemies.Where(x => !x.Dead))
            {
                enemy.Move(enemy.CalculateMoveDir(Player.Position));
            }

        }

        /// <summary>
        /// Update the entities position on the cells 2d array
        /// </summary>
        /// <returns>true - player has died, false - the player is still alive</returns>
        private bool UpdateCells()
        {
            //Update the enemies
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

            //Update the player
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
