using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Enemy : Unit
    {

        public Enemy(GameModel game, byte row, byte col) : base(game, row, col)
        {
        }

        [JsonConstructor]
        public Enemy(Position Position, Position PrevPosition, bool Dead) : base(Position, PrevPosition, Dead)
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
