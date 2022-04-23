using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace brick_buster_game
{
    public partial class MainForm : Form
    {
        int windowWidth, windowHeight;
        int barOffsetX, life;
        int ballSpeedX, ballSpeedY;
        int row, col, brickWidth, brickHeight, colSpace, count;
        readonly Random myRandomNumberGenerator = new Random();
        bool gameOver, win;
        PictureBox[,] bricks;
        SoundPlayer start;
        SoundPlayer boom;
        SoundPlayer harmed;
        SoundPlayer end;
        public MainForm()
        {
            InitializeComponent();
            windowWidth = ClientRectangle.Width;
            windowHeight = ClientRectangle.Height;
            row = 5;
            col = 10;
            brickWidth = 64;
            brickHeight = 32;
            colSpace = (windowWidth - brickWidth * col) / (col + 1);
            pictureBox4.BringToFront();
            pictureBox5.BringToFront();
            label1.Left = (windowWidth - 434) / 2;
            label1.Top = windowHeight / 4;
            pictureBox7.Left = (windowWidth - 200) / 2;
            pictureBox7.Top = windowHeight / 8;
            button1.Left = (windowWidth - 347) / 2;
            button1.Top = windowHeight / 2;
            start = new SoundPlayer(Properties.Resources.start);
            boom = new SoundPlayer(Properties.Resources.boom1);
            harmed = new SoundPlayer(Properties.Resources.harmed);
            end = new SoundPlayer(Properties.Resources.end);
            SetUpGame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Controls.Remove(bricks[i, j]);            
                }
            }
            label1.Visible = false;
            pictureBox7.Visible = false;
            button1.Visible = false;
            SetUpGame();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!gameOver && !win)
            {
                start.Play();
                timer1.Enabled = !timer1.Enabled;
            }
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            barOffsetX = e.X;
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            pictureBox5.Left += (e.X - barOffsetX);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox6.Visible = false;

            if (count == 0)  // Win
            {
                win = true;
                timer1.Enabled = false;
                label1.Visible = true;
                button1.Visible = true;
            }

            pictureBox4.Left += ballSpeedX;
            pictureBox4.Top += ballSpeedY;
            if (pictureBox4.Bounds.Right >= windowWidth)
            {
                ballSpeedX = myRandomNumberGenerator.Next(5) - 36;
            } else if (pictureBox4.Bounds.Left <= 0)
            {
                ballSpeedX = myRandomNumberGenerator.Next(5) + 32;
            } else if (pictureBox4.Bounds.Top <= 0)
            {
                ballSpeedY = myRandomNumberGenerator.Next(5) + 24;
            } else if (pictureBox4.Bounds.IntersectsWith(pictureBox5.Bounds))
            {
                ballSpeedY = myRandomNumberGenerator.Next(5) - 28;
            } else if (pictureBox4.Bounds.Top >= windowHeight)
            {
                life--;
                switch (life)
                {
                    case 2:
                        pictureBox1.Visible = false;
                        break;
                    case 1:
                        pictureBox2.Visible = false;
                        break;
                    case 0:
                        pictureBox3.Visible = false;
                        break;
                    default: //Game Over
                        end.Play();
                        gameOver = true;
                        timer1.Enabled = false;
                        pictureBox7.Visible = true;
                        button1.Visible = true;
                        return;
                }
                harmed.Play();
                timer1.Enabled = false;
                pictureBox4.Left = (windowWidth - 36) / 2;
                pictureBox4.Top = (windowHeight - 36) / 2;
                ballSpeedX = 2;
                ballSpeedY = 24;
            } else
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        PictureBox brick = bricks[i, j];
                        if (brick.Visible)
                        {
                            if (pictureBox4.Bounds.IntersectsWith(brick.Bounds))
                            {
                                boom.Play();
                                brick.Visible = false;
                                ballSpeedX = myRandomNumberGenerator.Next(2) == 0 ? -36 : 36;
                                ballSpeedY *= -1;
                                ballSpeedY += myRandomNumberGenerator.Next(2) == 0 ? -3 : 3;
                                pictureBox6.Location = brick.Location;
                                pictureBox6.Visible = true;
                                count--;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void SetUpGame()
        {
            gameOver = false;
            win = false;
            life = 3;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            count = row * col;
            ballSpeedX = 2;
            ballSpeedY = 24;
            pictureBox4.Left = (windowWidth - 36) / 2;
            pictureBox4.Top = (windowHeight - 36) / 2;
            bricks = new PictureBox[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    bricks[i, j] = new PictureBox
                    {
                        Top = brickHeight + i * 48,
                        Left = colSpace + (colSpace + brickWidth) * j,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = new Size(brickWidth, brickHeight),
                        Image = Properties.Resources.brick 
                    };
                    Controls.Add(bricks[i, j]);
                }
            }
        }

    }
}
