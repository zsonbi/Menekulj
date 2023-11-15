using Menekulj.Model;
using Menekulj.Persistance;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Menekulj.ViewModel;
namespace WPFView
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        private Menekulj.ViewModel.ViewModel viewModel;

        // GameModel? gameModel;
        Rectangle[,]? viewCells;

        ImageBrush gabcsika;
        ImageBrush player;
        ImageBrush mine;

        public View()
        {
            InitializeComponent();
            gabcsika = new ImageBrush(new BitmapImage(new Uri("./Images/enemy.png", UriKind.Relative)));
            player = new ImageBrush(new BitmapImage(new Uri("./Images/player.png", UriKind.Relative)));
            mine = new ImageBrush(new BitmapImage(new Uri("./Images/mine.png", UriKind.Relative)));
            viewModel = new Menekulj.ViewModel.ViewModel(new EventHandler(UpdateView),new EventHandler(GameOver));

        }


        private void CreateView()
        {
            viewCells = new Rectangle[viewModel.MatrixSize, viewModel.MatrixSize];
            Board.Children.Clear();
            Board.ColumnDefinitions.Clear();
            Board.RowDefinitions.Clear();
            for (int i = 0; i < viewModel.MatrixSize; i++)
            {
                Board.ColumnDefinitions.Add(new ColumnDefinition());
                Board.RowDefinitions.Add(new RowDefinition());
            }

            //Creates the cells for the game where the game object will be rendered
            for (int i = 0; i < viewModel.MatrixSize; i++)
            {
                for (int j = 0; j < viewModel.MatrixSize; j++)
                {
                    Rectangle cell = new Rectangle();
                    cell.Name = "cell" + i + "_" + j;
                    cell.Stroke = Brushes.Black;
                    switch (viewModel.GetCell(i, j))
                    {
                        case Cell.Empty:
                            cell.Fill = Brushes.White;
                            break;

                        case Cell.Player:
                            cell.Fill = player;
                            break;

                        case Cell.Enemy:
                            cell.Fill = gabcsika;
                            break;

                        case Cell.Mine:
                            cell.Fill = mine;
                            break;

                        default:
                            break;
                    }
                    cell.Stretch = Stretch.Fill;
                    Grid.SetZIndex(cell,-1);
                    Grid.SetColumn(cell, j);
                    Grid.SetRow(cell, i);
                    Board.Children.Add(cell);
                    viewCells[i, j] = cell;
                }
            }
            Board.Visibility = Visibility.Visible;

        }



        private void GameOver(object? sender, EventArgs args)
        {

            string message;
            if ( viewModel!.PlayerWon)
            {
                message = "You won! Want to try again?";
            }
            else
            {
                message = "Game over! You died :C Want to try again?";

            }
            if (MessageBox.Show(message, "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModel.NewGameCommand.Execute(sender);
                Application.Current.Dispatcher.Invoke(()=> CreateView());
                viewModel.StartGameCommand?.Execute(null);

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

            if (!viewModel.GameIsCreated)
            {
                throw new NoGameCreatedException();
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var enemy in viewModel.Enemies)
                {

                    viewCells[enemy.PrevPosition.Row, enemy.PrevPosition.Col].Fill = Brushes.White;
                    if (!enemy.Dead)
                    {
                        viewCells[enemy.Position.Row, enemy.Position.Col].Fill = gabcsika;
                    }
                }

                viewCells[viewModel.Player.PrevPosition.Row, viewModel.Player.PrevPosition.Col].Fill = Brushes.White;
                viewCells[viewModel.Player.Position.Row, viewModel.Player.Position.Col].Fill = player;
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

                viewModel.LoadGameCommand.Execute(loadedModel);
                Application.Current.Dispatcher.Invoke(()=> CreateView());
                viewModel.StartGameCommand?.Execute(null);

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
                viewModel.NewGameCommand.Execute(new uint[] { 11, 7 });
            }
            else if (MediumBoardRadio.IsChecked!.Value)
            {
                viewModel.NewGameCommand.Execute(new uint[] { 15, 14 });
            }
            else
            {
                viewModel.NewGameCommand.Execute(new uint[] { 21, 21 });
            }
            CreateView();
            viewModel.StartGameCommand?.Execute(null);

        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!viewModel.GameIsCreated)
            {
                return;
            }

            //capture up arrow key
            if (e.Key == Key.Up || e.Key == Key.W)
            {
                viewModel.ChangeDirectionCommand?.Execute(Direction.Up);
                return;
            }
            //capture down arrow key
            if (e.Key == Key.Down || e.Key == Key.S)
            {
                viewModel.ChangeDirectionCommand?.Execute(Direction.Down);
                return;
            }
            //capture left arrow key
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                viewModel.ChangeDirectionCommand?.Execute(Direction.Left);
                return;
            }
            //capture right arrow key
            if (e.Key == Key.Right || e.Key == Key.D)
            {
                viewModel.ChangeDirectionCommand?.Execute(Direction.Right);
                return;
            }
        }


        


        private void SaveGameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.GameIsCreated)
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
                    viewModel.SaveGameCommand?.Execute($"{saveFileDialog.FileName}");
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

        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.GameIsCreated)
            {
                throw new NoGameCreatedException();
            }

            viewModel.ResumeCommand?.Execute(sender);
            MainMenu.Visibility = Visibility.Hidden;
        }


        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!viewModel.GameIsCreated)
            {
                throw new NoGameCreatedException();
            }
            if (!viewModel.Running)
            {
                ResumeBtn_Click(sender, e);
                return;
            }

            ResumeBtn.Visibility = Visibility.Visible;

            viewModel.PauseCommand?.Execute(sender);

            MainMenu.Visibility = Visibility.Visible;
        }


        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
