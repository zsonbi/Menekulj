using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menekulj.Model
{
    /// <summary>
    /// Cell types for the board
    /// </summary>
    public enum Cell : byte
    {
        Empty=0,
        Player=1,
        Enemy=2,
        Mine=3

    }
}
