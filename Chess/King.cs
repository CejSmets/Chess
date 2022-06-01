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

            List<Tile> rookTiles = new List<Tile>();
            // castling
            foreach (var tile in game.Tiles)
            {
                if (tile.Piece != null && tile.Piece?.GetType() == typeof(Rook) && tile.Piece?.PlayerColor == this.PlayerColor)
                {
                    rookTiles.Add(tile);
                }
            }
            foreach (var rookTile in rookTiles)
            {
                Rook rook = (rookTile.Piece as Rook)!;

                if (rook.HasMoved || this.HasMoved) continue;
                if (rook.StartingFile == 0) legalMoves.Add(new int[,] { { currentX - 2, currentY } });
                if (rook.StartingFile == 7) legalMoves.Add(new int[,] { { currentX + 2, currentY } });
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

        public override bool Move(int currentX, int currentY, int targetX, int targetY, ChessGame game)
        {
            if (!IsLegalMove(currentX, currentY, targetX, targetY, game)) return false;

            // castling, check if all squares throughout are empty
            if (Math.Abs(targetX - currentX) > 1)
            {
                bool isShortCastle = targetX - currentX > 0;

                if (isShortCastle)
                {
                    for (int i = currentX + 1; i < targetX; i++)
                    {
                        if (game.Tiles[i, currentY].Piece != null) return false;
                    }

                    for (int i = currentX + 1; i <= targetX; i++)
                    {
                        // check all enemy pieces to see if they're attacking castling squares - can't castle through check

                        foreach (var tile in game.Tiles)
                        {
                            if (tile.Piece == null) continue;
                            if (tile.Piece?.PlayerColor == this.PlayerColor) continue;

                            foreach (var move in tile.Piece!.GetLegalMoves(tile.File, tile.Rank, game))
                            {
                                int moveXvalue = (int)move.GetValue(0, 0)!;
                                int moveYvalue = (int)move.GetValue(0, 1)!;

                                if (moveXvalue == i && moveYvalue == currentY) return false;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = currentX -1 ; i >= targetX; i--)
                    {
                        if (game.Tiles[i, currentY].Piece != null) return false;
                    }

                    for (int i = currentX - 1; i >= targetX; i--)
                    {
                        // check all enemy pieces to see if they're attacking castling squares - can't castle through check

                        foreach (var tile in game.Tiles)
                        {
                            if (tile.Piece == null) continue;
                            if (tile.Piece?.PlayerColor == this.PlayerColor) continue;

                            foreach (var move in tile.Piece!.GetLegalMoves(tile.File, tile.Rank, game))
                            {
                                int moveXvalue = (int)move.GetValue(0, 0)!;
                                int moveYvalue = (int)move.GetValue(0, 1)!;

                                if (moveXvalue == i && moveYvalue == currentY) return false;
                            }
                        }
                    }
                }

                

                // handle move logic
                game.EnPassant = false;

                // move piece to target square and capture if necessary
                game.Tiles[currentX, currentY].Piece = null;
                game.Tiles[targetX, targetY].Piece = this;


                Tile? rookTileToCastleWith = null;
                int fileToFind = 100;

                // short castle
                if (isShortCastle) fileToFind = 7;

                // long castle
                else fileToFind = 0;

                // find rook to castle with
                foreach (var tile in game.Tiles)
                {
                    if (tile.Rank == currentY && tile.Piece != null && tile.Piece!.GetType() == typeof(Rook) && (tile.Piece! as Rook)!.StartingFile == fileToFind)
                    {
                        rookTileToCastleWith = tile;
                        break;
                    }
                }

                Rook rookHolder = (rookTileToCastleWith!.Piece as Rook)!;
                rookTileToCastleWith.Piece = null;
                if (isShortCastle) game.Tiles[targetX - 1, currentY].Piece = rookHolder;
                else game.Tiles[targetX + 1, currentY].Piece = rookHolder;


                if (CheckEnPassant(currentY, targetX, targetY, game))
                {
                    game.EnPassant = true;
                }

                HasMoved = true;
                return true;
            }

            else return base.Move(currentX, currentY, targetX, targetY, game);
        }
    }
}
