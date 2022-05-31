using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class King : Piece
    {
        public King(Image image, PlayerColor color) : base(image, color)
        {
        }

        public override List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game)
        {
            var legalMoves = new List<int[,]>();
            var illegalMoves = new List<int[,]>();

            // right
            legalMoves.Add(new int[,] { { currentX + 1, currentY } } );

            // left
            legalMoves.Add(new int[,] { { currentX - 1, currentY } } );

            // up
            legalMoves.Add(new int[,] { { currentX, currentY + 1 } } );

            // down
            legalMoves.Add(new int[,] { { currentX, currentY - 1 } } );

            // up left
            legalMoves.Add(new int[,] { { currentX - 1, currentY + 1 } } );

            // up right
            legalMoves.Add(new int[,] { { currentX + 1, currentY + 1 } } );

            // down left
            legalMoves.Add(new int[,] { { currentX - 1, currentY - 1 } } );

            // down right
            legalMoves.Add(new int[,] { { currentX + 1, currentY - 1 } } );

            // remove out of bounds and own piece captures
            foreach (var move in legalMoves)
            {
                int xValue = (int)move.GetValue(0, 0)!;
                int yValue = (int)move.GetValue(0, 1)!;

                if (IsOutOfBounds(move) || game.Tiles[xValue, yValue].Piece?.PlayerColor == this.PlayerColor)
                    illegalMoves.Add(move);
            }

            foreach (var move in illegalMoves)
                legalMoves.Remove(move);

            return legalMoves;
        }
    }
}
