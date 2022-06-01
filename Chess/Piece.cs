using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public abstract class Piece
    {
        public Image Image { get; set; }
        public PlayerColor PlayerColor { get; set; }
        public bool HasMoved { get; set; }

        public Piece(Image image, PlayerColor color)
        {
            Image = image;
            PlayerColor = color;
        }

        public bool IsCurrentPlayerPiece(GameState state)
        {
            if (this.PlayerColor == PlayerColor.White) return state == GameState.WhiteTurn;
            else return state == GameState.BlackTurn;
        }

        public virtual bool Move(int currentX, int currentY, int targetX, int targetY, ChessGame game)
        {
            if (!IsLegalMove(currentX, currentY, targetX, targetY, game)) return false;

            // clone game to check if king will be in check
            ChessGame gameCopy = Helper.Clone(game);

            gameCopy.EnPassant = false;

            gameCopy.Tiles[currentX, currentY].Piece = null;
            gameCopy.Tiles[targetX, targetY].Piece = this;

            if (gameCopy.CurrentPlayerKingInCheck())
            {
                return false;
            }

            // handle move logic
            game.EnPassant = false;

            // move piece to target square and capture if necessary
            game.Tiles[currentX, currentY].Piece = null;
            game.Tiles[targetX, targetY].Piece = this;

            if (CheckEnPassant(currentY, targetX, targetY, game))
            {
                game.EnPassant = true;
            }

            HasMoved = true;
            return true;
        }

        public bool CheckEnPassant(int currentY, int targetX, int targetY, ChessGame game)
        {
            int leftSq = Math.Max(targetX - 1, 0);
            int rightSq = Math.Min(targetX + 1, 7);

            Piece? leftPiece = game.Tiles[leftSq, targetY].Piece;
            Piece? rightPiece = game.Tiles[rightSq, targetY].Piece;

            // check if this is a pawn and has moved forward 2 squares
            if (this.GetType() == typeof(Pawn) && Math.Abs(targetY - currentY) == 2)
            {
                // check left and right squares and make sure its not this (caused by out of bounds prevention)
                if (leftPiece?.GetType() == typeof(Pawn) && leftPiece != this && leftPiece.PlayerColor != this.PlayerColor) 
                    return true;

                if (rightPiece?.GetType() == typeof(Pawn) && rightPiece != this && rightPiece.PlayerColor != this.PlayerColor)
                    return true;
            }
            return false;
        }

        public bool IsLegalMove(int currentX, int currentY, int targetX, int targetY, ChessGame game)
        {
            return GetLegalMoves(currentX, currentY, game).Any(x => (int)x.GetValue(0, 0)! == targetX && (int)x.GetValue(0, 1)! == targetY);
        }

        public bool IsOutOfBounds(int[,] move)
        {
            int moveXvalue = (int)move.GetValue(0, 0)!;
            int moveYvalue = (int)move.GetValue(0, 1)!;

            return moveXvalue > 7 || moveXvalue < 0 || moveYvalue > 7 || moveYvalue < 0;
        }

        public bool IsOccupiedByOwnPiece(int[,] move, ChessGame game)
        {
            int moveXvalue = (int)move.GetValue(0, 0)!;
            int moveYvalue = (int)move.GetValue(0, 1)!;

            return game.Tiles[moveXvalue, moveYvalue].Piece?.PlayerColor == this.PlayerColor;
        }

        public abstract List<int[,]> GetLegalMoves(int currentX, int currentY, ChessGame game);
    }
}
