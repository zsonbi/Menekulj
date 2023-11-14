using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Menekulj.Model;
using Microsoft.Win32;

namespace Menekulj.ViewModel
{
    public class ViewModel
    {
        private GameModel? gameModel;

        public DelegateCommand ExitGameCommand {  get; private set; }
        public DelegateCommand LoadGameCommand {  get; private set; }
        public DelegateCommand SaveGameCommand {  get; private set; }
        public DelegateCommand NewGameCommand{get; private set; }

   

       public Player player;

        public ObservableCollection<Enemy> enemies;

        public ViewModel()
        {
            this.gameModel = new GameModel(10, 10);
            gabcsika = new ImageBrush(new BitmapImage(new Uri("./Images/enemy.png", UriKind.Relative)));
            playerImg = new ImageBrush(new BitmapImage(new Uri("./Images/player.png", UriKind.Relative)));
            mine = new ImageBrush(new BitmapImage(new Uri("./Images/mine.png", UriKind.Relative)));
        }






     //   Rectangle[,]? viewCells;

        private const int PadAmount = 20;
        private const int TopBarAmount = 70;
        ImageBrush gabcsika;
        ImageBrush playerImg;
        ImageBrush mine;


        private void CreateNewGame(byte boardSize = 0, uint mineCount = 0, GameModel? gameModel = null)
        {
            if (gameModel != null)
            {
                this.gameModel = gameModel;

            }
            else
            {
                this.gameModel = new GameModel(boardSize, mineCount);
            }
            CreateView();

            this.gameModel!.UpdateView += UpdateView;
            this.gameModel!.GameOver += GameOver;
            //await this.gameModel!.StartGame();
            _ = Task.Run(() => this.gameModel!.StartGame());
        }



        private void CreateView()
        {
            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }


            //viewCells = new Rectangle[gameModel.MatrixSize, gameModel.MatrixSize];
            //Board.Children.Clear();
            //Board.ColumnDefinitions.Clear();
            //Board.RowDefinitions.Clear();

            //for (int i = 0; i < gameModel.MatrixSize; i++)
            //{
            //    Board.ColumnDefinitions.Add(new ColumnDefinition());
            //    Board.RowDefinitions.Add(new RowDefinition());
            //}



            //Creates the cells for the game where the game object will be rendered
            for (int i = 0; i < gameModel.MatrixSize; i++)
            {
                for (int j = 0; j < gameModel.MatrixSize; j++)
                {
                    ////Rectangle cell = new Rectangle();
                    //cell.Name = "cell" + i + "_" + j;
                    //cell.Stroke = Brushes.Black;
                    //switch (gameModel.GetCell(i, j))
                    //{
                    //    case Cell.Empty:
                    //        cell.Fill = Brushes.White;
                    //        break;

                    //    case Cell.Player:
                    //        cell.Fill = playerImg;
                    //        break;

                    //    case Cell.Enemy:
                    //        cell.Fill = gabcsika;
                    //        break;

                    //    case Cell.Mine:
                    //        cell.Fill = mine;
                    //        break;

                    //    default:
                    //        break;
                    //}
                    //cell.Stretch = Stretch.Fill;

                    //Grid.SetColumn(cell, j);
                    //Grid.SetRow(cell, i);
                    //Board.Children.Add(cell);
                    //viewCells[i, j] = cell;
                }
            }
            //Board.Visibility = Visibility.Visible;

        }



        private void GameOver(object? sender, EventArgs args)
        {

            string message;
            if (gameModel!.PlayerWon)
            {
                message = "You won! Want to try again?";
            }
            else
            {
                message = "Game over! You died :C Want to try again?";

            }
            if (MessageBox.Show(message, "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                Application.Current.Dispatcher.Invoke(() => CreateNewGame(gameModel.MatrixSize, gameModel.MineCount));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
        }

        private void UpdateView(object? sender, EventArgs args)
        {
            if (viewCells == null)
            {
                throw new NullReferenceException();
            }

            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var enemy in gameModel.Enemies)
                {

                    viewCells[enemy.PrevPosition.Row, enemy.PrevPosition.Col].Fill = Brushes.White;
                    if (!enemy.Dead)
                    {
                        viewCells[enemy.Position.Row, enemy.Position.Col].Fill = gabcsika;
                    }
                }

                viewCells[gameModel.Player.PrevPosition.Row, gameModel.Player.PrevPosition.Col].Fill = Brushes.White;
                viewCells[gameModel.Player.Position.Row, gameModel.Player.Position.Col].Fill = playerImg;
            });
        }

        private async Task<bool> LoadGame()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "json";

            if (dialog.ShowDialog()!.Value)
            {

                GameModel loadedModel;
                try
                {
                    loadedModel = await Persistance.LoadStateAsync(dialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("This save file is invalid or corrupted");
                    return false;
                }



                CreateNewGame(gameModel: loadedModel);
                return true;
            }
            else
            {
                return false;
            }

        }


        //**********************************************************************************************************
        //Events
        //**********************************************************************************************************

        private void NewGameBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveGameBtn.Visibility = Visibility.Visible;
            PauseBtn.Visibility = Visibility.Visible;
            MainMenu.Visibility = Visibility.Hidden;
            MainMenu.Background = null;
            MenuStackPanel.Background = Brushes.Gray;
            if (SmallBoardRadio.IsChecked!.Value)
            {
                CreateNewGame(11, 7);
            }
            else if (MediumBoardRadio.IsChecked!.Value)
            {
                CreateNewGame(15, 14);
            }
            else
            {
                CreateNewGame(21, 21);
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameModel == null)
            {
                return;
            }

            //capture up arrow key
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                gameModel.ChangePlayerDirection(Direction.Up);
                return;
            }
            //capture down arrow key
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                gameModel.ChangePlayerDirection(Direction.Down);

                return;
            }
            //capture left arrow key
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                gameModel.ChangePlayerDirection(Direction.Left);

                return;
            }
            //capture right arrow key
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                gameModel.ChangePlayerDirection(Direction.Right);

                return;
            }
        }


        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (gameModel is null)
            {
                throw new NoGameCreatedException();
            }

            gameModel!.Resume();
            MainMenu.Visibility = Visibility.Hidden;


        }



        private async void SaveGameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (gameModel == null)
            {
                MessageBox.Show("No game is running");
                return;
            }



            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "json";
            //saveFileDialog.Filter ="(*.json)";
            if (saveFileDialog.ShowDialog()!.Value)
            {
                try
                {
                    await gameModel.SaveGame($"{saveFileDialog.FileName}");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while saving the game");
                }


            }
        }

        private async void LoadGameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (await LoadGame())
            {
                PauseBtn.Visibility = Visibility.Visible;
                SaveGameBtn.Visibility = Visibility.Visible;
                MainMenu.Visibility = Visibility.Hidden;
                MainMenu.Background = null;
                MenuStackPanel.Background = Brushes.Gray;
            }
            else
            {

            }

        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }
            if (!gameModel.Running)
            {
                ResumeBtn_Click(sender, e);
                return;
            }

            ResumeBtn.Visibility = Visibility.Visible;

            gameModel.Pause();
            MainMenu.Visibility = Visibility.Visible;
        }


        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }




    }
}
