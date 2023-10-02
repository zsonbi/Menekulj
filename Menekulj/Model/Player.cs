using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Player : Unit
    {
        /// <summary>
        /// What direction does the player looking at
        /// </summary>
        public Direction LookingDirection { get; private set; } = Direction.Right;
        
        /// <summary>
        /// Constructs a new player
        /// </summary>
        /// <param name="game">Reference to the game which the player is in</param>
        /// <param name="row">The starting row position of the player</param>
        /// <param name="col">The starting column position of the player</param>
        public Player(GameModel game,byte row=0, byte col=0) : base(game,row,col)
        {
          
        }

        /// <summary>
        /// Constructs a new player from the read in json 
        /// Don't forget to set the game reference after creating a player this way!
        /// </summary>
        /// <param name="Position">The current position of the player</param>
        /// <param name="PrevPosition">The previous position of the player</param>
        /// <param name="Dead">Is the player dead</param>
        /// <param name="lookingDirection">The current looking direction of the player</param>
        [JsonConstructor]
        public Player(Position Position, Position PrevPosition,bool Dead,Direction lookingDirection) : base(Position, PrevPosition, Dead)
        {
            this.LookingDirection = lookingDirection;
        }

        /// <summary>
        /// Sets the player's lookingdirection
        /// </summary>
        /// <param name="lookingDirection">The new lookingdirection</param>
        internal void SetDirection(Direction lookingDirection)
        {
            this.LookingDirection = lookingDirection;
        }

        /// <summary>
        /// Move towards the player's looking direction
        /// </summary>
        internal void Move()
        {
            base.Move(LookingDirection);
        }
 
    }
}
