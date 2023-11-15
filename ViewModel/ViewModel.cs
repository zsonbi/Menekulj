using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menekulj.Model;

using System.Windows;
using System.Windows.Input;


using Microsoft.Win32;
using System.ComponentModel;
using ViewModel;

namespace Menekulj.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private GameModel gameModel;

        private EventHandler gameOver;
        //  public DelegateCommand ExitGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand? SaveGameCommand { get; private set; }
        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand? PauseCommand { get; private set; }
        public DelegateCommand? ResumeCommand { get; private set; }

        public DelegateCommand? ChangeDirectionCommand { get; private set; }

        public DelegateCommand? StartGameCommand { get; private set; }

        public Player Player { get => gameModel.Player; }
        public ObservableCollection<Enemy> Enemies { get => gameModel.Enemies; }

        public ObservableCollection<ViewModelCell> ViewModelCells{ get; set;} = new ObservableCollection<ViewModelCell>();

        public byte MatrixSize { get => gameModel.MatrixSize; }
        public bool GameIsCreated { get => gameModel is not null; }
        public bool Running { get => gameModel.Running; }
        public bool PlayerWon { get => gameModel.PlayerWon; }

        public ViewModel( EventHandler gameOver)
        {
            this.gameModel = new GameModel(10, 10);
            NewGameCommand = new DelegateCommand(new Action<object?>(NewGame));
            LoadGameCommand = new DelegateCommand(new Action<object?>(LoadGame));

            this.gameOver = gameOver;
            this.ViewModelCells.Add(new ViewModelCell(0,0,-1));
        }


        private void BindCommandsToGameModel()
        {

            PauseCommand = new DelegateCommand((gameModel) => gameModel is not null, new Action<object?>(Pause));
            ResumeCommand = new DelegateCommand((gameModel) => gameModel is not null, new Action<object?>(Resume));
            SaveGameCommand = new DelegateCommand((gameModel) => gameModel is not null, new Action<object?>(SaveGame));
            ChangeDirectionCommand = new DelegateCommand((gameModel) => gameModel is not null, new Action<object?>(ChangeDirection));
            StartGameCommand = new DelegateCommand((gameModel) => gameModel is not null, new Action<object?>(StartGame));
        }

        //****************************************************************************************
        //Actions
        private void Pause(object? obj)
        {
            gameModel.Pause();
        }

        private void Resume(object? obj)
        {
            gameModel.Resume();
        }

        private void StartGame(object? obj)
        {
            _ = Task.Run(() => this.gameModel!.StartGame());

        }

        private void ChangeDirection(object? obj)
        {
            if (obj is Direction)
            {
                gameModel.ChangePlayerDirection((Direction)obj);
            }

        }

        private void NewGame(object? obj)
        {
            if (obj is int[])
            {
                uint[] arr = (uint[])obj;

                if (arr.Length < 2)
                {
                    throw new ArgumentException("The array is too short");
                }
                if (arr[0] >= 255)
                {
                    throw new ArgumentException("The map is too big");
                }

                CreateNewGame((byte)arr[0], arr[1]);
            }
            else if (gameModel is not null)
            {
                CreateNewGame(gameModel.MatrixSize, gameModel.MineCount);
            }
        }

        private void SaveGame(object? obj)
        {
            if (obj is string)
            {
                gameModel.SaveGame((string)obj).GetAwaiter();
            }

        }

        private void LoadGame(object? obj)
        {
            if (obj is GameModel)
            {
                CreateNewGame(gameModel: (GameModel)obj);
            }
        }


        //*********************************************************************************************
        //Actions end
        //********************************************************************************************

        //   Rectangle[,]? viewCells;



        public Cell GetCell(int row, int col)
        {


            return this.gameModel == null ? Cell.Empty : this.gameModel.GetCell(row, col);
        }




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
            //  CreateView();
            BindCommandsToGameModel();
            this.gameModel!.UpdateView += UpdateView;
            this.gameModel!.GameOver += gameOver;

            this.ViewModelCells.Clear();
         
            for (int i = 0; i < this.gameModel.MatrixSize; i++)
            {
                for (int j = 0; j < this.gameModel.MatrixSize; j++)
                {
                    ViewModelCell vMCell = new ViewModelCell(i, j, i * this.gameModel.MatrixSize + j);
                    vMCell.CellType = (int)this.gameModel.GetCell(i, j);
                    this.ViewModelCells.Add(vMCell);
                }
            }

            //await this.gameModel!.StartGame();
        }



        //private void CreateView()
        //{
        //    if (gameModel == null)
        //    {
        //        throw new NoGameCreatedException();
        //    }


        //    //viewCells = new Rectangle[gameModel.MatrixSize, gameModel.MatrixSize];
        //    //Board.Children.Clear();
        //    //Board.ColumnDefinitions.Clear();
        //    //Board.RowDefinitions.Clear();

        //    //for (int i = 0; i < gameModel.MatrixSize; i++)
        //    //{
        //    //    Board.ColumnDefinitions.Add(new ColumnDefinition());
        //    //    Board.RowDefinitions.Add(new RowDefinition());
        //    //}



        //    //Creates the cells for the game where the game object will be rendered
        //    for (int i = 0; i < gameModel.MatrixSize; i++)
        //    {
        //        for (int j = 0; j < gameModel.MatrixSize; j++)
        //        {
        //            ////Rectangle cell = new Rectangle();
        //            //cell.Name = "cell" + i + "_" + j;
        //            //cell.Stroke = Brushes.Black;
        //            //switch (gameModel.GetCell(i, j))
        //            //{
        //            //    case Cell.Empty:
        //            //        cell.Fill = Brushes.White;
        //            //        break;

        //            //    case Cell.Player:
        //            //        cell.Fill = playerImg;
        //            //        break;

        //            //    case Cell.Enemy:
        //            //        cell.Fill = gabcsika;
        //            //        break;

        //            //    case Cell.Mine:
        //            //        cell.Fill = mine;
        //            //        break;

        //            //    default:
        //            //        break;
        //            //}
        //            //cell.Stretch = Stretch.Fill;

        //            //Grid.SetColumn(cell, j);
        //            //Grid.SetRow(cell, i);
        //            //Board.Children.Add(cell);
        //            //viewCells[i, j] = cell;
        //        }
        //    }
        //    //Board.Visibility = Visibility.Visible;

        //}


        //***************************************************************************************************
        //model Events

        public void UnitPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "player")
            {

            }
            else if (e.PropertyName == "enemy")
            {

            }
            else
            {
                throw new NotImplementedException("This property is not implemented");

            }


        }

        public void GameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {


            if (e.PropertyName == "cells")
            {

            }
            else
            {
                throw new NotImplementedException("This property is not implemented");
            }


        }

        //private void GameOver(object? sender, EventArgs args)
        //{

        //    string message;
        //    if (gameModel!.PlayerWon)
        //    {
        //        message = "You won! Want to try again?";
        //    }
        //    else
        //    {
        //        message = "Game over! You died :C Want to try again?";

        //    }
        //    if (MessageBox.Show(message, "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {

        //        Application.Current.Dispatcher.Invoke(() => CreateNewGame(gameModel.MatrixSize, gameModel.MineCount));
        //    }
        //    else
        //    {
        //        //Application.Current.Dispatcher.Invoke(() => Close());
        //    }
        //}

        private void UpdateView(object? sender, EventArgs args)
        {
            if (this.ViewModelCells == null)
            {
                throw new NullReferenceException("ViewModelCells");
            }

            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }

            foreach (var enemy in gameModel.Enemies)
            {

                this.ViewModelCells[enemy.PrevPosition.Row * gameModel.MatrixSize + enemy.PrevPosition.Col].CellType =(int) Cell.Empty;
                if (!enemy.Dead)
                {
                    this.ViewModelCells[enemy.Position.Row * gameModel.MatrixSize + enemy.Position.Col].CellType = (int)Cell.Enemy;
                }
            }

            this.ViewModelCells[gameModel.Player.PrevPosition.Row * gameModel.MatrixSize + gameModel.Player.PrevPosition.Col].CellType = (int)Cell.Empty;
            this.ViewModelCells[gameModel.Player.Position.Row * gameModel.MatrixSize + gameModel.Player.Position.Col].CellType = (int)Cell.Player;

            OnPropertyChanged(nameof(ViewModelCells));
        }


        ////*************************************************************************************************

        //private async Task<bool> LoadGame()
        //{

        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.DefaultExt = "json";

        //    if (dialog.ShowDialog()!.Value)
        //    {

        //        GameModel loadedModel;
        //        try
        //        {
        //            //loadedModel = await Persistance.LoadStateAsync(dialog.FileName);
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("This save file is invalid or corrupted");
        //            return false;
        //        }



        //        //CreateNewGame(gameModel: loadedModel);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}


        ////**********************************************************************************************************
        ////Events
        ////**********************************************************************************************************

        //private void NewGameBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    //SaveGameBtn.Visibility = Visibility.Visible;
        //    //PauseBtn.Visibility = Visibility.Visible;
        //    //MainMenu.Visibility = Visibility.Hidden;
        //    //MainMenu.Background = null;
        //    //MenuStackPanel.Background = Brushes.Gray;
        //    //if (SmallBoardRadio.IsChecked!.Value)
        //    //{
        //    //    CreateNewGame(11, 7);
        //    //}
        //    //else if (MediumBoardRadio.IsChecked!.Value)
        //    //{
        //    //    CreateNewGame(15, 14);
        //    //}
        //    //else
        //    //{
        //    //    CreateNewGame(21, 21);
        //    //}
        //}












     




    }
}
