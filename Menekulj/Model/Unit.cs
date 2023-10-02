using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public abstract class Unit
    {
        /// <summary>
        /// The 2d position of the unit
        /// </summary>
        public Position Position { get; private set; }
        /// <summary>
        /// The previous 2d position of the unit
        /// </summary>
        public Position PrevPosition { get; private set; }
        /// <summary>
        /// Is the unit dead
        /// </summary>
        public bool Dead { get; protected set; } = false;
        //Reference to the game which houses the unit
        protected GameModel? game;

        /// <summary>
        /// Creates a new Unit
        /// </summary>
        /// <param name="game">Reference to the game which houses the unit</param>
        /// <param name="row">The current row position of the unit</param>
        /// <param name="col">The current column position of the unit</param>
        public Unit(GameModel game, byte row = 0, byte col = 0)
        {
            this.game = game;
            this.Position = new Position(row, col);
            //Also set the previous position to it to avoid nullpointerexception
            this.PrevPosition = new Position(row, col);
        }

        /// <summary>
        /// Creates a new unit from the json
        /// Don't forget to set the game reference after creating a unit this way!
        /// </summary>
        /// <param name="Position">The current position of the unit</param>
        /// <param name="PrevPosition">The previous position of the unit</param>
        /// <param name="Dead">Is the unit alive</param>
        [JsonConstructor]
        public Unit(Position Position, Position PrevPosition, bool Dead)
        {
            this.Position = Position;
            this.PrevPosition = PrevPosition;
            this.Dead = Dead;
        }

        /// <summary>
        /// Sets the game reference
        /// </summary>
        /// <param name="game">Reference to the game</param>
        internal void SetGame(GameModel game)
        {
            this.game = game;
        }

        /// <summary>
        /// Move to a specific (row,column)
        /// </summary>
        /// <param name="newRow">The row to move to</param>
        /// <param name="newCol">The column to move to</param>
        internal void MoveTo(byte newRow, byte newCol)
        {
            this.PrevPosition.SetPosition(this.Position);
            this.Position.SetPosition(newRow, newCol);
        }

        /// <summary>
        /// Move towards a direction
        /// </summary>
        /// <param name="dir">Direction to move towards</param>
        internal void Move(Direction dir)
        {
            if (game == null)
            {
                throw new NullReferenceException("Game was not set for the unit");
            }

            //Change the previous position
            this.PrevPosition.SetPosition(this.Position);
            //Move towards the appropiate (row,col)
            switch (dir)
            {
                case Direction.Left:
                    if (this.Position.Col - 1 >= 0)
                    {
                        this.Position.SetCol(this.Position.Col-1);
                    }
                    break;
                case Direction.Up:
                    if (this.Position.Row - 1 >=0)
                    {
                        this.Position.SetRow(this.Position.Row-1);
                    }
                    break;
                case Direction.Right:
                    if(this.Position.Col+1 < game.MatrixSize)
                    {
                        this.Position.SetCol(this.Position.Col+1);
                    }
                    break;
                case Direction.Down:
                    if (this.Position.Row + 1 < game.MatrixSize)
                    {
                        this.Position.SetRow(this.Position.Row+1);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Kill it
        /// (Change Dead to true)
        /// </summary>
        internal void Die()
        {
            this.Dead = true;
        }
    }
}
