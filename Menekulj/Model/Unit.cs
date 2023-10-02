using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Unit
    {

        public Position Position { get; private set; }
        public Position PrevPosition { get; private set; }
        public bool Dead { get; protected set; } = false;


        GameModel? game;
        public Unit(GameModel game, byte row = 0, byte col = 0)
        {
            this.game = game;

            this.Position = new Position(row, col);
            this.PrevPosition = new Position(row, col);
        }
        [JsonConstructor]
        public Unit(Position Position, Position PrevPosition, bool Dead)
        {
            this.Position = Position;
            this.PrevPosition = PrevPosition;
            this.Dead = Dead;
        }

        public void SetGame(GameModel game)
        {
            this.game = game;
        }

        public void MoveTo(byte newRow, byte newCol)
        {
            this.PrevPosition.SetPosition(this.Position);
            this.Position.SetPosition(newRow, newCol);
        }

        public void Move(Direction dir)
        {
            this.PrevPosition.SetPosition(this.Position);

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

        public void Die()
        {
            this.Dead = true;
        }
    }
}
