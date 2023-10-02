using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    public class Position
    {
       public int Row{get; private set; }
       public int Col{get; private set; }

        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public void SetPosition(int  row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public void SetPosition(Position pos)
        {
            this.Row = pos.Row;
            this.Col = pos.Col;
        }

        public void SetCol(int col)
        {
            this.Col=col;
        }

        public void SetRow(int row)
        {
            this.Row=row;
        }

        public float CalcDistance(Position other)
        {
            return (float)Math.Sqrt(Math.Pow( this.Row-other.Row,2)+Math.Pow(this.Col-other.Col,2));
        }

        public float DistanceTo(int row, int col)
        {
            return (float)Math.Sqrt(Math.Pow(this.Row - row, 2) + Math.Pow(this.Col - col, 2));

        }
    }
}
