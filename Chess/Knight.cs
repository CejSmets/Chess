using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Knight : Piece
    {
        public Knight(Image image, PlayerColor color) : base(image, color)
        {
        }

        public override List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game)
        {
            var legalMoves = new List<int[,]>();
            var illegalMoves = new List<int[,]>();

            // up moves
            legalMoves.Add(new int[,] { { currentX - 1, currentY + 2 } });
            legalMoves.Add(new int[,] { { currentX + 1, currentY + 2 } });

            // down moves
            legalMoves.Add(new int[,] { { currentX - 1, currentY - 2 } });
            legalMoves.Add(new int[,] { { currentX + 1, currentY - 2 } });

            // left moves
            legalMoves.Add(new int[,] { { currentX - 2, currentY - 1 } });
            legalMoves.Add(new int[,] { { currentX - 2, currentY + 1 } });

            // right moves
            legalMoves.Add(new int[,] { { currentX + 2, currentY - 1 } });
            legalMoves.Add(new int[,] { { currentX + 2, currentY + 1 } });

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
