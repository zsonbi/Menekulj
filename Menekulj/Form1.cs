using Menekulj.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Menekulj
{

    public partial class MenekuljWindow : Form
    {

        GameController controller;
        Task gameTask;
        List<Control> elements = new List<Control>();
        Control[,] viewCells;
        private int width;
        private int height;
        private const int padAmount = 20;
        private System.Windows.Forms.Timer timer;

        public MenekuljWindow()
        {
            InitializeComponent();
            width = this.Width - 40;
            height = this.Height - 40;
        }

        private void NewGameBtn_Click(object sender, EventArgs e)
        {
            NewGameBtn.Hide();
            SmallRadio.Hide();
            MediumRadio.Hide();
            BigRadio.Hide();

            if (SmallRadio.Checked)
            {
                CreateNewGame(11, 10);
            }
            else if(MediumRadio.Checked)
            {
                CreateNewGame(15, 20);
            }
            else
            {
                CreateNewGame(21, 30);
            }


        }

        private void CreateNewGame(byte boardSize, uint mineCount)
        {
            controller = new GameController(boardSize, mineCount);

            //  controller.StartGame();
            CreateView(boardSize);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = GameController.DelayAmount;
            timer.Tick += Update;
            timer.Start();
        }

        private void CreateView(byte boardSize)
        {
            if (controller == null)
            {
                throw new NoGameCreatedException();
            }

            int elementSize = Math.Min(width / boardSize, height / boardSize);



            SuspendLayout();

            foreach (var item in elements)
            {
                Controls.Remove(item);
            }

            elements.Clear();
            viewCells = new Control[controller.MatrixSize, controller.MatrixSize];

            int counter = 0;
            for (int i = 0; i < controller.MatrixSize; i++)
            {
                for (int j = 0; j < controller.MatrixSize; j++)
                {
                    Button cellButton = new Button();

                    // 
                    // NewGameBtn
                    // 
                    cellButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
                    cellButton.Location = new Point(padAmount + j * elementSize, padAmount + i * elementSize);
                    cellButton.Name = "cell" + i + ";" + j;
                    cellButton.Size = new Size(elementSize - 1, elementSize - 1);
                    cellButton.TabIndex = 0;
                    cellButton.BackgroundImageLayout = ImageLayout.Stretch;
                    // cellButton.Visible=false;
                    switch (controller.Cells[i, j])
                    {
                        case Cell.Empty:
                            cellButton.Text = "";

                            break;
                        case Cell.Player:
                            cellButton.BackgroundImage = new Bitmap("./Images/player.png");

                            break;
                        case Cell.Enemy:
                           // cellButton.Text = "E";
                            cellButton.BackgroundImage = new Bitmap("./Images/enemy.png");


                            break;
                        case Cell.Mine:
                          //  cellButton.Text = "M";
                            cellButton.BackgroundImage = new Bitmap("./Images/mine.png");
                            cellButton.BackgroundImageLayout = ImageLayout.Stretch;
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

            //foreach (var cell in elements)
            //{
            //    cell.Visible=true;
            //}


            ResumeLayout(false);
        }

        private void Update(object? sender, EventArgs args)
        {
            if (!controller.IsOver())
            {
                controller.Tick(sender, args);
                UpdateView();

            }

            if (controller.IsOver())
            {
                timer.Stop();
                string message;
                if (controller.PlayerWon)
                {
                    message = "You won! Want to try again?";
                }
                else
                {
                    message = "Game over! You died :C Want to try again?";

                }
                if (MessageBox.Show(message, "Result", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    CreateNewGame(controller.MatrixSize, controller.MineCount);
                }
                else
                {
                    Close();
                }
            }

        }

        private void UpdateView()
        {
            foreach (var enemy in controller.Enemies)
            {

                viewCells[enemy.prevPosition.Row, enemy.prevPosition.Col].BackgroundImage=null;
                if (!enemy.Dead)
                {  // viewCells[enemy.Position.Row, enemy.Position.Col].Text = "E";
                    viewCells[enemy.Position.Row, enemy.Position.Col].BackgroundImage = new Bitmap("./Images/enemy.png");

                }
            }


            //viewCells[controller.Player.prevPosition.Row, controller.Player.prevPosition.Col].Text = "";
            //viewCells[controller.Player.Position.Row, controller.Player.Position.Col].Text = "P";
            viewCells[controller.Player.prevPosition.Row, controller.Player.prevPosition.Col].BackgroundImage = null;
            viewCells[controller.Player.Position.Row, controller.Player.Position.Col].BackgroundImage = new Bitmap("./Images/player.png") ;
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (controller == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            //capture up arrow key
            if (keyData == Keys.Up || keyData == Keys.W)
            {
                controller.ChangePlayerDirection(Direction.Up);
                return true;
            }
            //capture down arrow key
            if (keyData == Keys.Down || keyData == Keys.S)
            {
                controller.ChangePlayerDirection(Direction.Down);

                return true;
            }
            //capture left arrow key
            if (keyData == Keys.Left || keyData==Keys.A)
            {
                controller.ChangePlayerDirection(Direction.Left);

                return true;
            }
            //capture right arrow key
            if (keyData == Keys.Right || keyData==Keys.D)
            {
                controller.ChangePlayerDirection(Direction.Right);

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}