namespace Chess
{
    public partial class ChessBoard : Form
    {
        const int tileSize = 80;
        public ChessBoard()
        {
            InitializeComponent();
        }

        static ChessGame game = new ChessGame();
        Panel[,] _chessBoardPanels = new Panel[game.Tiles.GetLength(0), game.Tiles.GetLength(1)];

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 680;
            this.Height = 680;
            this.Text = "Chess by Cedricsken";

            CreateGrid();
        }

        public void Panel_Click(object sender, EventArgs e)
        {
            // get correct coordinates to use within the game
            var x = (sender as Panel)!.Location.X / tileSize;
            var y = (sender as Panel)!.Location.Y / tileSize;

            Tile tile = game.Tiles[x, y];

            Piece? currPiece = tile.Piece;

            // check if a player piece was clicked
            if (currPiece == null || !currPiece.IsCurrentPlayerPiece(game.State))
            {
                // return if no piece was already selected to be moved
                if (game.SelectedTile == null) return;

                // try moving a selected piece
                Piece pieceToMove = game.SelectedTile.Piece!;
                if (pieceToMove.Move(game.SelectedTile.File, game.SelectedTile.Rank, x, y, game))
                {
                    UpdateGrid(x, y, game.SelectedTile.File, game.SelectedTile.Rank, pieceToMove);
                    DeselectTile();
                    game.SwitchTurns();

                    if (game.CurrentPlayerKingInCheck())
                    {
                        if(game.CurrentPlayerKingInCheckMate())
                        {
                            var winner = game.State == GameState.WhiteTurn ? "Black" : "White";
                            game.State = GameState.GameOver;

                            MessageBox.Show($"Checkmate! {winner} wins!");
                        }
                    }
                }
            }

            // select/deselect player piece
            else
            {
                if (game.SelectedTile == game.Tiles[x, y]) DeselectTile();
                else
                {
                    game.SelectedTile = game.Tiles[x, y];
                    SelectTile((sender as Panel)!);
                }

            }
        }

        public void DeselectTile()
        {
            game.SelectedTile = null;
            foreach (var control in Controls)
            {
                if (control.GetType() == typeof(Panel))
                {
                    (control as Panel)!.BackColor = (Color)(control as Panel)!.Tag;
                }
            }
        }

        public void SelectTile(Panel panel)
        {
            foreach (var control in Controls)
            {
                if (control.GetType() == typeof(Panel))
                {
                    (control as Panel)!.BackColor = (Color)(control as Panel)!.Tag;
                }
            }
            panel.BackColor = Color.Green;
        }

        private void UpdateGrid(int targetX, int targetY, int originalX, int originalY, Piece piece)
        {
            foreach (var tile in game.Tiles)
            {
                Controls.OfType<Panel>().First(x => x.Name == $"{tile.File}, {tile.Rank}").BackgroundImage =
                    tile.Piece == null ? null : tile.Piece.Image;
            }
        }

        private void CreateGrid()
        {


            for (int i = 0; i < game.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < game.Tiles.GetLength(1); j++)

                {
                    var newPanel = new Panel()
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(tileSize * i, tileSize * j),
                        Tag = game.Tiles[i, j].TileColor,
                        Name = $"{i}, {j}"
                    };

                    Controls.Add(newPanel);

                    _chessBoardPanels[i, j] = newPanel;

                    newPanel.BackColor = game.Tiles[i, j].TileColor;
                    newPanel.BackgroundImage = game.Tiles[i, j].Piece?.Image;
                    newPanel.Click += Panel_Click;
                }
            }
        }
    }

    public enum GameState
    {
        WhiteTurn,
        BlackTurn,
        GameOver
    }

    public enum PlayerColor
    {
        Black,
        White
    }
}