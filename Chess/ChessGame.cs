using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Serializable]
    public class ChessGame
    {
        public Tile[,] Tiles { get; set; }
        public Tile? SelectedTile { get; set; }
        public GameState State { get; set; } = GameState.WhiteTurn;
        public bool EnPassant { get; set; }

        public ChessGame()
        {
            Tiles = new Tile[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tile tile = new Tile(i, j);

                    Tiles[i, j] = tile;

                    if ((i + j) % 2 == 0) tile.TileColor = Color.Brown;
                    else tile.TileColor = Color.White;
                }
            }
            Setup();
        }

        public bool CurrentPlayerKingInCheckMate()
        {
            ChessGame originalGame = Helper.Clone(this);
            PlayerColor currentPlayerColor = this.State == GameState.WhiteTurn ? PlayerColor.White : PlayerColor.Black;

            foreach (var tile in this.Tiles)
            {
                if (tile.Piece == null || tile.Piece.PlayerColor != currentPlayerColor) continue;

                foreach (var move in tile.Piece!.GetLegalMoves(tile.File, tile.Rank, originalGame))
                {
                    ChessGame gameCopy = Helper.Clone(this);
                    int moveXvalue = (int)move.GetValue(0, 0)!;
                    int moveYvalue = (int)move.GetValue(0, 1)!;
                    if (tile.Piece!.Move(tile.File, tile.Rank, moveXvalue, moveYvalue, gameCopy)) return false;
                }
            }

            return true;
        }

        public bool CurrentPlayerKingInCheck()
        {
            PlayerColor currentPlayerColor = this.State == GameState.WhiteTurn ? PlayerColor.White : PlayerColor.Black;

            Tile? currentPlayerKingTile = null;

            foreach (var tile in this.Tiles)
            {
                if (tile.Piece?.PlayerColor == currentPlayerColor && tile.Piece?.GetType() == typeof(King))
                    currentPlayerKingTile = tile;
            }

            foreach (var tile in this.Tiles)
            {
                // check if tile has a piece
                if (tile.Piece == null) continue;

                // check if enemy tile
                if (tile.Piece?.PlayerColor == currentPlayerColor) continue;



                if (tile.Piece!.GetLegalMoves(tile.File, tile.Rank, this)
                    .Any(x => (int)x.GetValue(0, 0)! == currentPlayerKingTile!.File && (int)x.GetValue(0, 1)! == currentPlayerKingTile!.Rank))
                    return true;
            }
            return false;
        }

        public void SwitchTurns()
        {
            State = State == GameState.WhiteTurn ? GameState.BlackTurn : GameState.WhiteTurn;
        }

        private void Setup()
        {
            // setup pawns
            for (int i = 0; i < 8; i++)
            {
                // black
                Tiles[i, 1].Piece = new Pawn(
                    new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "pawn")), 80, 80),
                    PlayerColor.Black);

                // white
                Tiles[i, 6].Piece = new Pawn(
                    new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "pawn")), 80, 80),
                    PlayerColor.White);

                if (i == 0 || i == 7)
                {
                    // black
                    Tiles[i, 0].Piece = new Rook(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "rook")), 80, 80),
                        PlayerColor.Black);


                    // white
                    Tiles[i, 7].Piece = new Rook(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "rook")), 80, 80),
                        PlayerColor.White);
                }

                if (i == 1 || i == 6)
                {
                    // black
                    Tiles[i, 0].Piece = new Knight(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "knight")), 80, 80),
                        PlayerColor.Black);


                    // white
                    Tiles[i, 7].Piece = new Knight(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "knight")), 80, 80),
                        PlayerColor.White);
                }

                if (i == 2 || i == 5)
                {
                    // black
                    Tiles[i, 0].Piece = new Bishop(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "bishop")), 80, 80),
                        PlayerColor.Black);


                    // white
                    Tiles[i, 7].Piece = new Bishop(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "bishop")), 80, 80),
                        PlayerColor.White);
                }

                if (i == 4)
                {
                    // black
                    Tiles[i, 0].Piece = new King(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "king")), 80, 80),
                        PlayerColor.Black);


                    // white
                    Tiles[i, 7].Piece = new King(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "king")), 80, 80),
                        PlayerColor.White);
                }

                if (i == 3)
                {
                    // black
                    Tiles[i, 0].Piece = new Queen(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.Black, "queen")), 80, 80),
                        PlayerColor.Black);


                    // white
                    Tiles[i, 7].Piece = new Queen(
                        new Bitmap(Image.FromFile(GetImageUrl(PlayerColor.White, "queen")), 80, 80),
                        PlayerColor.White);
                }
            }

        }

        private string GetImageUrl(PlayerColor color, string pieceType)
        {
            string baseUrl = @"C:\Users\cedrics\source\repos\Chess\Chess\images\#type#_#color#.png";
            string colorString = color.ToString().ToLower();

            return baseUrl.Replace("#color#", colorString).Replace("#type#", pieceType);
        }
    }
}
