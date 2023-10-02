using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menekulj.Model;


namespace Menekulj.Persistance
{
    internal struct SaveGameState
    {
        public Player Player { get; private set;}        
        public List<Enemy> Enemies { get; private set;}
        public Cell[,] Cells { get; private set; }
        public uint MineCount { get; private set; }
        public Direction LookingDirection {get; private set; }
        public byte MatrixSize { get; private set; }

        public SaveGameState(GameModel modelToSave)
        {
            this.Player = modelToSave.Player;
            this.Enemies = modelToSave.Enemies;
            this.Cells = modelToSave.Cells;
            this.MineCount = modelToSave.MineCount;
            this.LookingDirection = modelToSave.LookingDirection;
            this.MatrixSize = modelToSave.MatrixSize;
        }
    }
}
