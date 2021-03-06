using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Bishop : Piece
    {
        public Bishop(Image image, PlayerColor color) : base(image, color)
        {
        }

        public override List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game)
        {
            var legalMoves = new List<int[,]>();
            var illegalMoves = new List<int[,]>();

            // up right
            int x = currentX + 1;
            int y = currentY + 1;

            while (x <= 7 && y <= 7)
            {
                if (game.Tiles[x, y].Piece != null)
                {
                    if (game.Tiles[x, y].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { x, y } });

                    break;
                }
                else legalMoves.Add(new int[,] { { x, y } });

                x++;
                y++;
            }

            // down left
            x = currentX - 1;
            y = currentY - 1;

            while (x >= 0 && y >= 0)
            {
                if (game.Tiles[x, y].Piece != null)
                {
                    if (game.Tiles[x, y].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { x, y } });

                    break;
                }
                else legalMoves.Add(new int[,] { { x, y } });

                x--;
                y--;
            }

            // up left
            x = currentX - 1;
            y = currentY + 1;

            while (x >= 0 && y <= 7)
            {
                if (game.Tiles[x, y].Piece != null)
                {
                    if (game.Tiles[x, y].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { x, y } });

                    break;
                }
                else legalMoves.Add(new int[,] { { x, y } });

                x--;
                y++;
            }

            // down right
            x = currentX + 1;
            y = currentY - 1;

            while (x <= 7 && y >= 0)
            {
                if (game.Tiles[x, y].Piece != null)
                {
                    if (game.Tiles[x, y].Piece!.PlayerColor != this.PlayerColor)
                        legalMoves.Add(new int[,] { { x, y } });

                    break;
                }
                else legalMoves.Add(new int[,] { { x, y } });

                x++;
                y--;
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
