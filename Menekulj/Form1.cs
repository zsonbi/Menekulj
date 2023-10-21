using Menekulj.Model;

namespace Menekulj
{

    public partial class View : Form
    {

        GameModel? gameModel;
        List<Control> elements = new List<Control>();
        Control[,]? viewCells;
        private int width;
        private int height;
        private const int PadAmount = 20;
        private const int TopBarAmount = 70;

        public View()
        {
            InitializeComponent();
            width = this.Width - PadAmount * 2;
            height = this.Height - PadAmount * 2 - TopBarAmount;

        }



        private async Task CreateNewGame(byte boardSize = 0, uint mineCount = 0, GameModel? gameModel = null)
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
            await this.gameModel!.StartGame();

        }



        private void CreateView()
        {
            PauseBtn.Visible = true;
            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }

            int elementSize = Math.Min(width / gameModel.MatrixSize, height / gameModel.MatrixSize);



            SuspendLayout();

            foreach (var item in elements)
            {
                Controls.Remove(item);
            }

            elements.Clear();
            viewCells = new Control[gameModel.MatrixSize, gameModel.MatrixSize];

            int counter = 0;
            //Creates the cells for the game where the game object will be rendered
            for (int i = 0; i < gameModel.MatrixSize; i++)
            {
                for (int j = 0; j < gameModel.MatrixSize; j++)
                {
                    //A single cell is a disabled button -,- cool stuff...
                    Button cellButton = new Button();
                    cellButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
                    cellButton.Location = new Point(PadAmount + j * elementSize, PadAmount + TopBarAmount / 2 + i * elementSize);
                    cellButton.Name = "cell" + i + ";" + j;
                    cellButton.Size = new Size(elementSize - 1, elementSize - 1);
                    cellButton.TabIndex = 0;
                    cellButton.BackgroundImageLayout = ImageLayout.Stretch;
                    switch (gameModel.GetCell(i, j))
                    {
                        case Cell.Empty:
                            break;

                        case Cell.Player:
                            cellButton.BackgroundImage = new Bitmap("./Images/player.png");
                            break;

                        case Cell.Enemy:
                            cellButton.BackgroundImage = new Bitmap("./Images/enemy.png");
                            break;

                        case Cell.Mine:
                            cellButton.BackgroundImage = new Bitmap("./Images/mine.png");
                            break;

                        default:
                            break;
                    }

                    cellButton.UseVisualStyleBackColor = true;

                    elements.Add(cellButton);
                    Controls.Add(cellButton);
                    viewCells[i, j] = cellButton;
                    cellButton.Enabled = false;
                    counter++;
                }
            }


            ResumeLayout(false);
        }



        private async void GameOver(object? sender, EventArgs args)
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
            if (MessageBox.Show(message, "Result", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
            {

                await CreateNewGame(gameModel.MatrixSize, gameModel.MineCount);
            }
            else
            {
                Close();
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
            foreach (var enemy in gameModel.Enemies)
            {

                viewCells[enemy.PrevPosition.Row, enemy.PrevPosition.Col].BackgroundImage = null;
                if (!enemy.Dead)
                {
                    viewCells[enemy.Position.Row, enemy.Position.Col].BackgroundImage = new Bitmap("./Images/enemy.png");
                }
            }

            viewCells[gameModel.Player.PrevPosition.Row, gameModel.Player.PrevPosition.Col].BackgroundImage = null;
            viewCells[gameModel.Player.Position.Row, gameModel.Player.Position.Col].BackgroundImage = new Bitmap("./Images/player.png");
        }

        private async Task<bool> LoadGame()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".json";

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                GameModel loadedModel;
                try
                {
                    loadedModel = await Persistance.Persistance.LoadStateAsync(dialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("This save file is invalid or corrupted");
                    return false;
                }
                if (gameModel == null)
                {
                    NewGameBtn.Hide();
                    SmallRadio.Hide();
                    MediumRadio.Hide();
                    BigRadio.Hide();
                    LoadGameBtn.Hide();
                    Closebtn.Hide();
                    this.BackgroundImage = null;
                }



                await CreateNewGame(gameModel: loadedModel);
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

        private async void NewGameBtn_Click(object sender, EventArgs e)
        {
            NewGameBtn.Hide();
            SmallRadio.Hide();
            MediumRadio.Hide();
            BigRadio.Hide();
            LoadGameBtn.Hide();
            Closebtn.Hide();
            this.BackgroundImage = null;

            if (SmallRadio.Checked)
            {
                await CreateNewGame(11, 7);
            }
            else if (MediumRadio.Checked)
            {
                await CreateNewGame(15, 14);
            }
            else
            {
                await CreateNewGame(21, 21);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameModel == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            //capture up arrow key
            if (keyData == Keys.Up || keyData == Keys.W)
            {
                gameModel.ChangePlayerDirection(Direction.Up);
                return true;
            }
            //capture down arrow key
            if (keyData == Keys.Down || keyData == Keys.S)
            {
                gameModel.ChangePlayerDirection(Direction.Down);

                return true;
            }
            //capture left arrow key
            if (keyData == Keys.Left || keyData == Keys.A)
            {
                gameModel.ChangePlayerDirection(Direction.Left);

                return true;
            }
            //capture right arrow key
            if (keyData == Keys.Right || keyData == Keys.D)
            {
                gameModel.ChangePlayerDirection(Direction.Right);

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ResumeBtn_Click(object sender, EventArgs e)
        {
            if (gameModel is null)
            {
                throw new NoGameCreatedException();
            }
            gameModel!.Resume();
            PausePanel.Visible = false;

        }



        private async void PauseSaveGameBtn_Click(object sender, EventArgs e)
        {
            if (gameModel == null)
            {
                MessageBox.Show("No game is running");
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await gameModel.SaveGame($"{folderBrowserDialog.SelectedPath}\\{DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss")}save.json");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while saving the game");
                }


            }

        }

        private async void PauseLoadGameBtn_Click(object sender, EventArgs e)
        {
            if (await LoadGame())
            {
                PausePanel.Visible = false;
            }
        }

        private async void LoadGameBtn_Click(object sender, EventArgs e)
        {


            await LoadGame();


        }

        private void pauseBtn_Click(object sender, EventArgs e)
        {
            if (gameModel == null)
            {
                throw new NoGameCreatedException();
            }
            gameModel.Pause();
            PausePanel.Visible = true;
        }

        private void Closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}