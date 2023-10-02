using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Unit
    {

        public Position Position { get; private set; }
        public Position prevPosition { get; private set; }
        public bool Dead { get; protected set; } = false;


        GameController game;
        public Unit(GameController game, byte row = 0, byte col = 0)
        {
            this.game = game;

            this.Position = new Position(row, col);
            this.prevPosition = new Position(row, col);
        }

        public void MoveTo(byte newRow, byte newCol)
        {
            this.prevPosition.SetPosition(this.Position);
            this.Position.SetPosition(newRow, newCol);
        }

        public void Move(Direction dir)
        {
            this.prevPosition.SetPosition(this.Position);

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
