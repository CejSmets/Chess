using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Tile
    {
        public int File { get; set; }
        public int Rank { get; set; }
        public Piece? Piece { get; set; } = null;
        public Color TileColor { get; set; }

        public Tile(int file, int rank)
        {
            this.File = file;
            this.Rank = rank;
        }
    }
}
