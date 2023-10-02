using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Enemy : Unit
    {

        public Enemy(GameController game, byte row, byte col) : base(game, row, col)
        {
        }

        public Direction CalculateMoveDir(Position playerPos)
        {
            Direction dir = Direction.Left;
            float least = playerPos.DistanceTo(this.Position.Row, this.Position.Col - 1);

            float newLeast = playerPos.DistanceTo(this.Position.Row - 1, this.Position.Col);

            if (least > newLeast)
            {
                dir = Direction.Up;
                least = newLeast;
            }
            newLeast = playerPos.DistanceTo(this.Position.Row, this.Position.Col + 1);
            if (least > newLeast)
            {
                dir = Direction.Right;
                least = newLeast;
            }
            newLeast = playerPos.DistanceTo(this.Position.Row + 1, this.Position.Col);
            if (least > newLeast)
            {
                dir = Direction.Down;
                least = newLeast;
            }

            return dir;

        }

    }
}
