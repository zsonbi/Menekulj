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
using System.Windows.Controls.Primitives;

namespace WPFView
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Window
    {
        private Menekulj.ViewModel.ViewModel viewModel;


        public View()
        {
            InitializeComponent();

            viewModel = new Menekulj.ViewModel.ViewModel(new EventHandler(GameOver));
            this.DataContext = viewModel;
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
                Application.Current.Dispatcher.Invoke(() => {  
                viewModel.NewGameCommand.Execute(null);
                viewModel.StartGameCommand?.Execute(null);
                ItemControl.UpdateLayout();
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
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
                viewModel.StartGameCommand?.Execute(null);
                ItemControl.UpdateLayout();

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
            MainMenu.Background = null;
            MenuStackPanel.Background = Brushes.DarkGray;

            viewModel.NewGameCommand.Execute(null);
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
            MenuStackPanel.Background = Brushes.DarkGray;
            MainMenu.Background = null;

            await LoadGame();
        }

      


        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}
