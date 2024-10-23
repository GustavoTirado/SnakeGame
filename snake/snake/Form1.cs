using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    public partial class Form1 : Form
    {
        private List<Point> snake;
        private Point food;
        private int direction;
        private int score;
        private Timer timer;
        private const int squareSize = 20;
        private const int initialLength = 5;
        private const int totalSquaresX = 20;
        private const int totalSquaresY = 15;
        private Label scoreLabel;
        private const int borderSize = 5; 

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            InitializeScoreLabel();
            InitializeGame();
        }

        private void InitializeScoreLabel()
        {
            scoreLabel = new Label
            {
                Name = "scoreLabel",
                Text = "Score: 0",
                ForeColor = Color.White,
                Location = new Point(10, 320),
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold)
            };
            Controls.Add(scoreLabel);
        }

        private void InitializeGame()
        {
            snake = new List<Point>();
            for (int i = 0; i < initialLength; i++)
            {
                snake.Add(new Point(100 - i * squareSize, 100));
            }
            direction = 1; 
            score = 0;
            GenerateFood();
            UpdateScoreLabel();

            timer = new Timer();
            timer.Interval = 100; 
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            Invalidate();
        }

        private void MoveSnake()
        {
            Point head = snake[0];


            switch (direction)
            {
                case 0: 
                    head.Y -= squareSize;
                    break;
                case 1: 
                    head.X += squareSize;
                    break;
                case 2: 
                    head.Y += squareSize;
                    break;
                case 3:
                    head.X -= squareSize;
                    break;
            }

            if (head == food)
            {
                score++;
                snake.Insert(0, head); 
                GenerateFood(); 
                UpdateScoreLabel();

                if (snake.Count >= totalSquaresX * totalSquaresY)
                {
                    timer.Stop();
                    MessageBox.Show("¡Ganaste!");
                    InitializeGame();
                }
            }
            else
            {
                snake.Insert(0, head); 
                snake.RemoveAt(snake.Count - 1);
            }

            if (IsGameOver(head))
            {
                timer.Stop();
                MessageBox.Show($"Game Over! Your score: {score}");
                InitializeGame();
            }
        }

        private bool IsGameOver(Point head)
        {

            if (head.X < 0 || head.X >= totalSquaresX * squareSize || head.Y < 0 || head.Y >= totalSquaresY * squareSize)
                return true;

        
            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i]) return true;
            }

            return false;
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            do
            {
                food = new Point(rand.Next(0, totalSquaresX) * squareSize,
                                 rand.Next(0, totalSquaresY) * squareSize);
            } while (snake.Contains(food)); 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(Pens.White, borderSize, borderSize, totalSquaresX * squareSize, totalSquaresY * squareSize);

            Pen gridPen = new Pen(Color.FromArgb(128, 255, 255, 255)); 

            for (int i = 0; i <= totalSquaresX; i++)
            {
                e.Graphics.DrawLine(gridPen,
                                     new Point(i * squareSize + borderSize, borderSize),
                                     new Point(i * squareSize + borderSize, totalSquaresY * squareSize + borderSize));
            }

            for (int j = 0; j <= totalSquaresY; j++)
            {
                e.Graphics.DrawLine(gridPen,
                                     new Point(borderSize, j * squareSize + borderSize),
                                     new Point(totalSquaresX * squareSize + borderSize, j * squareSize + borderSize));
            }

            gridPen.Dispose();

            for (int i = 0; i < snake.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Green, new Rectangle(snake[i].X + borderSize, snake[i].Y + borderSize, squareSize, squareSize));
            }

            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(food.X + borderSize, food.Y + borderSize, squareSize, squareSize));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (direction != 2) direction = 0; 
                    break;
                case Keys.Right:
                    if (direction != 3) direction = 1; 
                    break;
                case Keys.Down:
                    if (direction != 0) direction = 2; 
                    break;
                case Keys.Left:
                    if (direction != 1) direction = 3; 
                    break;
            }
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Score: {score}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
