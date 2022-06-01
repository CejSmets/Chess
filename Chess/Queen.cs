using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Queen : Piece
    {
        public Queen(Image image, PlayerColor color) : base(image, color)
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
