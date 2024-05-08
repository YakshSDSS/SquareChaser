using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Threading;

namespace SquareChaser
{
    public partial class squareChaser : Form
    {
        Random randGen = new Random();
   
        bool wPressed = false;
        bool aPressed = false;
        bool sPressed = false;
        bool dPressed = false;
        bool upPressed = false;
        bool leftPressed = false;
        bool downPressed = false;
        bool rightPressed = false;

        int player1Speed = 2;
        int player2Speed = 2;
        int player1Score = 0;
        int player2Score = 0;

        SolidBrush greenBrush = new SolidBrush(Color.Lime);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Pen blackPen = new Pen(Color.Black, 2);

        Rectangle pointSquare = new Rectangle(0, 0, 5, 5);
        Rectangle speedCircle = new Rectangle(0, 0, 5, 5);
        Rectangle player1 = new Rectangle(90, 190, 20, 20);
        Rectangle player2 = new Rectangle(290, 190, 20, 20);

        new SoundPlayer speedSound = new SoundPlayer(Properties.Resources.speedSound);
        new SoundPlayer pointSound = new SoundPlayer(Properties.Resources.pointSound);
        new SoundPlayer introSound = new SoundPlayer(Properties.Resources.introSound);

        public squareChaser()
        {
            InitializeComponent();

            //Spawn speed boost and point square randomly.
            RandomizePointSquare();
            RandomizeSpeedBoost();

            //Play intro sound. 
            introSound.Play();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Remove boundaries so when players go out of screen they come out from the other side.
            if (player1.Y < 0)
            {
                player1.Y = player1.Y + this.Height;
            }
            if (player1.Y > this.Height - player1.Height)
            {
                player1.Y = player1.Y - this.Height;
            }
            if (player1.X < 0)
            {
                player1.X = player1.X + this.Width;
            }
            if (player1.X > this.Width - player1.Width)
            {
                player1.X = player1.X - this.Width;
            }
            if (player2.Y < 0)
            {
                player2.Y = player2.Y + this.Height;
            }
            if (player2.Y > this.Height - player2.Height)
            {
                player2.Y = player2.Y - this.Height;
            }
            if (player2.X < 0)
            {
                player2.X = player2.X + this.Width;
            }
            if (player2.X > this.Width - player2.Width)
            {
                player2.X = player2.X - this.Width;
            }
            //Move player 1.
            if (wPressed == true)
            {
                player1.Y = player1.Y - player1Speed;
            }
            if (aPressed == true)
            {
                player1.X = player1.X - player1Speed;
            }
            if (sPressed == true)
            {
                player1.Y = player1.Y + player1Speed;
            }
            if (dPressed == true)
            {
                player1.X = player1.X + player1Speed;
            }
            //Move player 2.
            if (upPressed == true)
            {
                player2.Y = player2.Y - player2Speed;
            }
            if (leftPressed == true)
            {
                player2.X = player2.X - player2Speed;
            }
            if (downPressed == true)
            {
                player2.Y = player2.Y + player2Speed;
            }
            if (rightPressed == true)
            {
                player2.X = player2.X + player2Speed;
            }
            //Make player 1 & 2 faster when they hit speed boost. 
            if (player1.IntersectsWith(speedCircle) && player1Speed < 8)
            {
                player1Speed++;
                RandomizeSpeedBoost();
                speedSound.Play();
            }
            if (player2.IntersectsWith(speedCircle) && player2Speed < 8)
            {
                player2Speed++;
                RandomizeSpeedBoost();
                speedSound.Play();
            }
            //Give points and randomize spawn of point squares.
            if (player1.IntersectsWith(pointSquare))
            {
                player1Score++;
                RandomizePointSquare();
                player1ScoreLabel.Text = $"{player1Score}";
                pointSound.Play();
            }
            if (player2.IntersectsWith(pointSquare))
            {
                player2Score++;
                RandomizePointSquare();
                player2ScoreLabel.Text = $"{player2Score}";
                pointSound.Play();
            }
            //Check for a winner and stop the game.
            if (player1Score == 5)
            {
                winnerLabel.Text = $"Player 1 wins \n{player1Score} - {player2Score}";
                gameTimer.Stop();
            }
            if (player2Score == 5)
            {
                winnerLabel.Text = $"Player 2 wins \n{player2Score} - {player1Score}";
                gameTimer.Stop();
            }

            Refresh();
        }

        private void squareChaser_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.A:
                    aPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.D:
                    dPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Left:
                    leftPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Right:
                    rightPressed = true;
                    break;
            }
        }

        private void squareChaser_KeyUp(object sender, KeyEventArgs e)
        {     
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.A:
                    aPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.D:
                    dPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Left:
                    leftPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.Right:
                    rightPressed = false;
                    break;
            }
        }

        private void squareChaser_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(whiteBrush, pointSquare);
            e.Graphics.FillEllipse(yellowBrush, speedCircle);
            e.Graphics.FillRectangle(orangeBrush, player1);
            e.Graphics.FillRectangle(greenBrush, player2);
            e.Graphics.DrawRectangle(blackPen, player1);
            e.Graphics.DrawRectangle(blackPen, player2);
        }

        public void RandomizeSpeedBoost()
        {
            speedCircle.X = randGen.Next(this.Width - speedCircle.Width);
            speedCircle.Y = randGen.Next(this.Height - speedCircle.Height);
        }
        public void RandomizePointSquare()
        {
            pointSquare.X = randGen.Next(this.Width - pointSquare.Width);
            pointSquare.Y = randGen.Next(this.Height - pointSquare.Height);
        }
    }
}
