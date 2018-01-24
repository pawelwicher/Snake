using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class FormSnake : Form
    {
        private Snake snake;
        private Snake.MoveDirection direction;
        private bool moveLock = false;
        private Bitmap bitmap;
        private int points;

        public FormSnake()
        {
            InitializeComponent();
            this.snake = new Snake();
            this.bitmap = new Bitmap(640, 400);
        }

        private void FormSnake_Load(object sender, EventArgs e)
        {
            Draw(GameStatus.None);
        }

        private void FormSnake_KeyDown(object sender, KeyEventArgs e)
        {
            if (!moveLock)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        direction = Snake.MoveDirection.Left;
                        moveLock = true;
                        break;
                    case Keys.Right:
                        direction = Snake.MoveDirection.Right;
                        moveLock = true;
                        break;
                    case Keys.Up:
                        direction = Snake.MoveDirection.Up;
                        moveLock = true;
                        break;
                    case Keys.Down:
                        direction = Snake.MoveDirection.Down;
                        moveLock = true;
                        break;
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (snake.IsAlive)
            {
                snake.Move(direction);
                moveLock = false;
                lblStatus.Text = string.Format("Score: {0}", snake.Points);

                if (snake.Points == 25)
                {
                    Draw(GameStatus.Finish);
                    snake.IsAlive = false;
                    timer.Stop();
                }
                else
                {
                    Draw(GameStatus.None);
                }

                if (snake.Points != points)
                {
                    points = snake.Points;
                    timer.Interval--;
                }
            }
            else
            {
                Draw(GameStatus.GameOver);
                timer.Stop();
            }
        }

        private void nowaGraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void Draw(GameStatus gameStatus)
        {
            if (bitmap != null)
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    for (int x = 0; x < Snake.xSize; x++)
                        for (int y = 0; y < Snake.ySize; y++)
                            switch (snake.Pool[x, y])
                            {
                                case Snake.FieldType.Border:
                                    g.DrawImage(RCS.brick, new Rectangle(x * 20 + 1, y * 20 + 1, 19, 19));
                                    break;
                                case Snake.FieldType.Tail:
                                    g.DrawImage(RCS.snaketail, new Rectangle(x * 20 + 1, y * 20 + 1, 19, 19));
                                    break;
                                case Snake.FieldType.Food:
                                    switch (snake.CurrentFood)
                                    {
                                        case Snake.FoodType.Food1:
                                            g.DrawImage(RCS.food1, new Rectangle(x * 20 + 1, y * 20 + 1, 19, 19));
                                            break;
                                        case Snake.FoodType.Food2:
                                            g.DrawImage(RCS.food2, new Rectangle(x * 20 + 1, y * 20 + 1, 19, 19));
                                            break;
                                        case Snake.FoodType.Food3:
                                            g.DrawImage(RCS.food3, new Rectangle(x * 20 + 1, y * 20 + 1, 19, 19));
                                            break;
                                    }
                                    break;
                            }

                    switch (gameStatus)
                    {
                        case GameStatus.Finish:
                            g.DrawImageUnscaled(RCS.finish, 190, 100);
                            break;
                        case GameStatus.GameOver:
                            g.DrawImageUnscaled(RCS.gameover, 190, 100);
                            break;
                    }
                }
                pbox.Image = bitmap;
            }
        }

        private void StartGame()
        {
            timer.Stop();
            snake.Start();
            direction = Snake.MoveDirection.Right;
            moveLock = false;
            timer.Interval = 100;
            timer.Start();
        }
    }
}