using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class Pawn : Piece
    {
        public Pawn(Image image, PlayerColor color)
            : base(image, color) { }

        public override List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game)
        {
            var legalMoves = new List<int[,]>();

            // Add forward moves
            if (this.PlayerColor == PlayerColor.Black)
            {
                legalMoves.Add(new int[,] { { currentX, currentY + 1 } });
                if (!this.HasMoved) legalMoves.Add(new int[,] { { currentX, currentY + 2 } });
            }
            else if (this.PlayerColor == PlayerColor.White)
            {
                legalMoves.Add(new int[,] { { currentX, currentY - 1 } });
                if (!this.HasMoved) legalMoves.Add(new int[,] { { currentX, currentY - 2 } });
            }

            // remove occupied squares in front of pawns && out of bounds moves
            List<int[,]> movesToRemove = new List<int[,]>();
            foreach (var move in legalMoves)
            {
                int moveXvalue = (int)move.GetValue(0, 0)!;
                int moveYvalue = (int)move.GetValue(0, 1)!;
                if (IsOutOfBounds(move) || game.Tiles[moveXvalue, moveYvalue].Piece != null)
                {
                    movesToRemove.Add(move);
                }
            }

            // Add diagonal captures
            foreach (var move in GetDiagonalCaptureMoves(currentX, currentY, game))
            {
                legalMoves.Add(move);
            }

            foreach (var move in movesToRemove)
            {
                legalMoves.Remove(move);
            }

            return legalMoves;
        }

        public List<int[,]> GetDiagonalMoves(int currentX, int currentY, ChessGame game)
        {
            List<int[,]> diagonalMoves = new List<int[,]>();

            // Get all diagonal forward moves
            if (this.PlayerColor == PlayerColor.Black)
            {
                diagonalMoves.Add(new int[,] { { currentX + 1, currentY + 1 } });
                diagonalMoves.Add(new int[,] { { currentX - 1, currentY + 1 } });
            }
            if (this.PlayerColor == PlayerColor.White)
            {
                diagonalMoves.Add(new int[,] { { currentX + 1, currentY - 1 } });
                diagonalMoves.Add(new int[,] { { currentX - 1, currentY - 1 } });
            }

            return diagonalMoves;
        }

        public List<int[,]> GetDiagonalCaptureMoves(int currentX, int currentY, ChessGame game)
        {
            List<int[,]> legalMoves = new List<int[,]>();
            List<int[,]> diagonalMoves = GetDiagonalMoves(currentX, currentY, game);

            // Filter out illegal diagonal moves
            foreach (var move in diagonalMoves)
            {
                int xValue = (int)move.GetValue(0, 0)!;
                int yValue = (int)move.GetValue(0, 1)!;

                if (!IsOutOfBounds(move) && game.Tiles[xValue, yValue].Piece != null && game.Tiles[xValue, yValue].Piece?.PlayerColor != this.PlayerColor) 
                    legalMoves.Add(move);
            }

            return legalMoves;
        }

        public override bool Move(int currentX, int currentY, int targetX, int targetY, ChessGame game)
        {
            if (!game.EnPassant) return base.Move(currentX, currentY, targetX, targetY, game);

            bool isDiagonalMove = GetDiagonalMoves(currentX, currentY, game)
                .Any(x => (int)x.GetValue(0, 0)! == targetX && (int)x.GetValue(0, 1)! == targetY);

            bool targetSquareIsFree = game.Tiles[targetX, targetY].Piece == null;
            
            bool passingPawn = game.Tiles[targetX, currentY].Piece?.GetType() == typeof(Pawn);

            if (isDiagonalMove && targetSquareIsFree && passingPawn)
            {
                // handle move logic
                game.EnPassant = false;

                // move piece to target square
                game.Tiles[currentX, currentY].Piece = null;
                game.Tiles[targetX, targetY].Piece = this;

                // capture for en passant
                game.Tiles[targetX, currentY].Piece = null;

                HasMoved = true;
                return true;
            }
            else return base.Move(currentX, currentY, targetX, targetY, game);
        }
    }
}
