using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Rook : Piece
    {
        public Rook(Image image, PlayerColor color) : base(image, color)
        {
        }

        public override List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game)
        {
            var legalMoves = new List<int[,]>();
            var illegalMoves = new List<int[,]>();

            // left and right moves
            for (int i = currentX + 1; i <= 7; i++)
            {
                if (game.Tiles[i, currentY].Piece != null)
                {
                    if (game.Tiles[i, currentY].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { i, currentY } });

                    break;
                }
                else legalMoves.Add(new int[,] { { i, currentY } });
            }
            for (int i = currentX - 1; i >= 0; i--)
            {
                if (game.Tiles[i, currentY].Piece != null)
                {
                    if (game.Tiles[i, currentY].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { i, currentY } });

                    break;
                }
                else legalMoves.Add(new int[,] { { i, currentY } });
            }

            // up and down moves
            for (int i = currentY + 1; i <= 7; i++)
            {
                if (game.Tiles[currentX, i].Piece != null)
                {
                    if (game.Tiles[currentX, i].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { currentX, i } });

                    break;
                }
                else legalMoves.Add(new int[,] { { currentX, i } });
            }
            for (int i = currentY - 1; i >= 0; i--)
            {
                if (game.Tiles[currentX, i].Piece != null)
                {
                    if (game.Tiles[currentX, i].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { currentX, i } });

                    break;
                }
                else legalMoves.Add(new int[,] { { currentX, i } });
            }

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
