using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/TileRed.png", UriKind.Relative))

        };

        private readonly ImageSource[] blockImage = new ImageSource[]
        {
            new BitmapImage(new Uri("Images/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Images/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private Gamestatus gamestate = new Gamestatus();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCancas(gamestate.Gamegrid);
        }

        private Image[,] SetupGameCancas(Gamegrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(Gamegrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.Tileposition())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];

            }
        }

        private void DrawNextBlock(Queue blockQueue)
        {
            Block next = blockQueue.Nextblock;
            NextImage.Source = blockImage[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImage[0];
            }
            else
            {
                HoldImage.Source = blockImage[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gamestate.BlockDropDistance();

            foreach (Position p in block.Tileposition())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(Gamestatus gameState)
        {
            DrawGrid(gameState.Gamegrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.Queue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score : {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gamestate);
            while (!gamestate.Gameover)
            {
                int delay = Math.Max(minDelay, maxDelay - (gamestate.Score * delayDecrease));
                await Task.Delay(delay);
                gamestate.Movedown();
                Draw(gamestate);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score : {gamestate.Score}";
        }

        private KeyBindings keyBindings = new KeyBindings();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gamestate.Gameover)
            {
                return;
            }

            if (e.Key == keyBindings.MoveLeft)
            {
                gamestate.Moveleft();
            }
            else if (e.Key == keyBindings.MoveRight)
            {
                gamestate.Moveright();
            }
            else if (e.Key == keyBindings.MoveDown)
            {
                gamestate.Movedown();
            }
            else if (e.Key == keyBindings.RotateCW)
            {
                gamestate.RotateCW();
            }
            else if (e.Key == keyBindings.RotateCCW)
            {
                gamestate.RotateCCW();
            }
            else if (e.Key == keyBindings.HoldBlock)
            {
                gamestate.HoldBlock();
            }
            else if (e.Key == keyBindings.DropBlock)
            {
                gamestate.DropBlock();
            }

            Draw(gamestate);
        }


        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gamestate = new Gamestatus();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gamestate = new Gamestatus();
            MainMenu.Visibility = Visibility.Hidden;
            Game.Visibility = Visibility.Visible;
            GameLoop();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            Game.Visibility = Visibility.Hidden;
            GameOverMenu.Visibility = Visibility.Hidden;
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
            OptionMenu.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                MoveLeftComboBox.Items.Add(key);
                MoveRightComboBox.Items.Add(key);
                MoveDownComboBox.Items.Add(key);
                RotateCWComboBox.Items.Add(key);
                RotateCCWComboBox.Items.Add(key);
                HoldBlockComboBox.Items.Add(key);
                DropBlockComboBox.Items.Add(key);
            }

            MoveLeftComboBox.SelectedItem = keyBindings.MoveLeft;
            MoveRightComboBox.SelectedItem = keyBindings.MoveRight;
            MoveDownComboBox.SelectedItem = keyBindings.MoveDown;
            RotateCWComboBox.SelectedItem = keyBindings.RotateCW;
            RotateCCWComboBox.SelectedItem = keyBindings.RotateCCW;
            HoldBlockComboBox.SelectedItem = keyBindings.HoldBlock;
            DropBlockComboBox.SelectedItem = keyBindings.DropBlock;
        }


        private void SaveKeyBindings_Click(object sender, RoutedEventArgs e)
        {
            keyBindings.MoveLeft = (Key)MoveLeftComboBox.SelectedItem;
            keyBindings.MoveRight = (Key)MoveRightComboBox.SelectedItem;
            keyBindings.MoveDown = (Key)MoveDownComboBox.SelectedItem;
            keyBindings.RotateCW = (Key)RotateCWComboBox.SelectedItem;
            keyBindings.RotateCCW = (Key)RotateCCWComboBox.SelectedItem;
            keyBindings.HoldBlock = (Key)HoldBlockComboBox.SelectedItem;
            keyBindings.DropBlock = (Key)DropBlockComboBox.SelectedItem;

            MainMenu.Visibility = Visibility.Visible;
            OptionMenu.Visibility = Visibility.Hidden;
        }

        private void KeyBindingComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.SelectedItem = e.Key;
            e.Handled = true;
        }
    }
}
