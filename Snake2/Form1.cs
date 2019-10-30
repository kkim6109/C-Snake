using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake2
{
    public partial class Form1 : Form
    {
        bool[,] grid;
        int snakeLength = 0;
        Point[] snakePositions;
        char[] directionHistory;
        string nextDirection;
        Graphics graphics;
        SolidBrush square;
        SolidBrush apple;
        SolidBrush blank;
        SolidBrush gray;
        Pen bigBox;
        Point previousPosition;
        Point applePosition;
        string lastDirection;
        int difficulty;
        string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\My Games\\EFSnake");
        Point[] walls;
        int wallAmt;
        int pointE;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string str = System.IO.File.ReadAllText(path + "\\Highscore.kev");
                label2.Text = str;
            }
            catch
            { }
        }

        private void restartGame()
        {
            wallAmt = 0;
            pointE = 0;
            walls = new Point[441];
            graphics.FillRectangle(blank, new Rectangle(0, 0, 645, 700));
            timer1.Enabled = false;
            nextDirection = " ";
            lastDirection = "";
            directionHistory = new char[441];
            grid = new bool[21, 21];
            snakeLength = 1;
            snakePositions = new Point[441];
            snakePositions[0] = new Point(10, 10);
            Random s = new Random();
            applePosition = new Point((int)(s.NextDouble() * 21), (int)(s.NextDouble() * 21));
            while (snakePositions[0] == applePosition)
            {
                applePosition = new Point((int)(s.NextDouble() * 21), (int)(s.NextDouble() * 21));
            }
            graphics.FillRectangle(square, new Rectangle(snakePositions[0].X * 30, snakePositions[0].Y * 30, 30, 30));
            graphics.FillRectangle(apple, new Rectangle(applePosition.X * 30, applePosition.Y * 30, 30, 30));
            graphics.FillRectangle(gray, new Rectangle(0, 631, 645, 30));
            label1.Text = "001";
            try
            {
                if (difficulty == 1)
                {
                    string str;
                    try
                    {
                        str = System.IO.File.ReadAllText(path + "\\Highscore.kev");
                    }
                    catch
                    {
                        str = "001";
                    }
                    label2.Text = str;
                    label2.ForeColor = Color.Blue;
                    label1.ForeColor = Color.Blue;
                }
                else if (difficulty == 0)
                {
                    string str;
                    try
                    {
                        str = System.IO.File.ReadAllText(path + "\\EHighscore.kev");
                    }
                    catch
                    {
                        str = "001";
                    }
                    label2.Text = str;
                    label1.ForeColor = Color.Cyan;
                    label2.ForeColor = Color.Cyan;
                }
                else
                {
                    string str;
                    try
                    {
                        str = System.IO.File.ReadAllText(path + "\\HHighscore.kev");
                    }
                    catch
                    {
                        str = "001";
                    }
                    label2.Text = str;
                    label2.ForeColor = Color.Red;
                    label1.ForeColor = Color.Red;
                }
            }
            catch
            { }
        }

        int oldSnake;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Point oldApple = applePosition;
            oldSnake = snakeLength;
            if (graphics == null)
            {
                difficulty = 1;
                graphics = this.CreateGraphics();
                bigBox = new Pen(Form1.ActiveForm.BackColor, 2);
                square = new SolidBrush(Color.Black);
                apple = new SolidBrush(Color.Red);
                blank = new SolidBrush(Form1.ActiveForm.BackColor);
                gray = new SolidBrush(Color.DimGray);
                restartGame();
            }
            else
            {
                previousPosition = snakePositions[snakeLength - 1];
                for (int i = snakeLength - 1; i > 0; i--)
                {
                    snakePositions[i] = snakePositions[i - 1];
                    directionHistory[i] = directionHistory[i - 1];
                }
                char direct;
                if (nextDirection.Length > 0)
                {
                    direct = nextDirection[0];
                }
                else
                {
                    direct = lastDirection[0];
                }
                directionHistory[0] = direct;
                if (direct == 'L')
                {
                    snakePositions[0] = new Point(snakePositions[0].X - 1, snakePositions[0].Y);
                }
                else if (direct == 'R')
                {
                    snakePositions[0] = new Point(snakePositions[0].X + 1, snakePositions[0].Y);
                }
                else if (direct == 'D')
                {
                    snakePositions[0] = new Point(snakePositions[0].X, snakePositions[0].Y + 1);
                }
                else if (direct == 'U')
                {
                    snakePositions[0] = new Point(snakePositions[0].X, snakePositions[0].Y - 1);
                }
                if (nextDirection.Length > 0)
                {
                    nextDirection = nextDirection.Remove(0, 1);
                }
                bool continueThis = true;
                bool continueIt = true;
                for (int i = 0; i < snakeLength - 1; i++)
                {
                    for (int j = i + 1; j < snakeLength; j++)
                    {
                        if (snakePositions[i] == snakePositions[j])
                        {
                            continueIt = false;
                        }
                    }
                }
                if (difficulty == 2)
                {
                    for (int i = 0; i < wallAmt; i++)
                    {
                        if (walls[i] == snakePositions[0])
                        {
                            continueIt = false;
                        }
                    }
                }
                if (snakePositions[0].X < 0 || snakePositions[0].X > 20 || snakePositions[0].Y < 0 || snakePositions[0].Y > 20 || !continueIt)
                {
                    if (difficulty == 0 && continueIt)
                    {
                        if (snakePositions[0].X < 0)
                        {
                            snakePositions[0] = new Point(20, snakePositions[0].Y);
                        }
                        else if (snakePositions[0].X > 20)
                        {
                            snakePositions[0] = new Point(0, snakePositions[0].Y);
                        }
                        else if (snakePositions[0].Y < 0)
                        {
                            snakePositions[0] = new Point(snakePositions[0].X, 20);
                        }
                        else if (snakePositions[0].Y > 20)
                        {
                            snakePositions[0] = new Point(snakePositions[0].X, 0);
                        }
                    }
                    else
                    {
                        continueThis = false;
                        restartGame();
                    }
                }
                if (continueThis)
                {
                    if (snakePositions[0] == applePosition)
                    {
                        bool loop = true;
                        while (snakeLength < 441 && loop)
                        {
                            Random s = new Random();
                            applePosition = new Point((int)(s.NextDouble() * 21), (int)(s.NextDouble() * 21));
                            loop = false;
                            for (int i = 0; i < snakeLength; i++)
                            {
                                if (snakePositions[i] == applePosition)
                                {
                                    loop = true;
                                }
                            }
                            if (difficulty == 2)
                            {
                                for (int i = 0; i < wallAmt; i++)
                                {
                                    if (applePosition == walls[i])
                                    {
                                        loop = true;
                                    }
                                }
                            }
                        }
                        snakeLength++;
                        string result = snakeLength + "";
                        for (int i = result.Length; i < 3; i++)
                        {
                            result = "0" + result;
                        }
                        label1.Text = result;
                        if (int.Parse(label2.Text) < snakeLength)
                        {
                            label2.Text = label1.Text;
                            if (!System.IO.Directory.Exists(path))
                                System.IO.Directory.CreateDirectory(path);
                            if (difficulty == 1)
                                System.IO.File.WriteAllText(path + "\\Highscore.kev", label2.Text);
                            else if (difficulty == 0)
                                System.IO.File.WriteAllText(path + "\\EHighscore.kev", label2.Text);
                            else
                                System.IO.File.WriteAllText(path + "\\HHighscore.kev", label2.Text);
                        }
                        if (difficulty == 2)
                        {
                            pointE++;
                            if (pointE == 3)
                            {
                                pointE = 0;
                                wallAmt++;
                                loop = true;
                                while (snakeLength < 441 - wallAmt && loop)
                                {
                                    Random s = new Random();
                                    walls[wallAmt - 1] = new Point((int)(s.NextDouble() * 21), (int)(s.NextDouble() * 21));
                                    loop = false;
                                    for (int i = 0; i < snakeLength; i++)
                                    {
                                        if (snakePositions[i] == walls[wallAmt - 1])
                                        {
                                            loop = true;
                                        }
                                    }
                                    if (walls[wallAmt - 1] == applePosition)
                                    {
                                        loop = true;
                                    }
                                }
                                graphics.FillRectangle(square, new Rectangle(walls[wallAmt - 1].X * 30, walls[wallAmt - 1].Y * 30, 30, 30));
                            }
                        }
                    }
                    int speed;
                    try
                    {
                        speed = int.Parse(textBox1.Text);
                    }
                    catch
                    {
                        speed = 2;
                    }
                    if (direct == 'R')
                    {
                        for (int i = 29; i >= 0; i--)
                        {
                            System.Threading.Thread.Sleep(speed);
                            animation(i);
                            graphics.FillRectangle(square, new Rectangle(snakePositions[0].X * 30 - i, snakePositions[0].Y * 30, 30, 30));
                        }
                    }
                    else if (direct == 'L')
                    {
                        for (int i = 29; i >= 0; i--)
                        {
                            System.Threading.Thread.Sleep(speed);
                            animation(i);
                            graphics.FillRectangle(square, new Rectangle(snakePositions[0].X * 30 + i, snakePositions[0].Y * 30, 1, 30));
                        }
                    }
                    else if (direct == 'D')
                    {
                        for (int i = 29; i >= 0; i--)
                        {
                            System.Threading.Thread.Sleep(speed);
                            animation(i);
                            graphics.FillRectangle(square, new Rectangle(snakePositions[0].X * 30, snakePositions[0].Y * 30 - i, 30, 30));
                        }
                    }
                    else if (direct == 'U')
                    {
                        for (int i = 29; i >= 0; i--)
                        {
                            System.Threading.Thread.Sleep(speed);
                            animation(i);
                            graphics.FillRectangle(square, new Rectangle(snakePositions[0].X * 30, (snakePositions[0].Y) * 30 + i, 30, 1));
                        }
                    }
                }
                graphics.FillRectangle(apple, new Rectangle(applePosition.X * 30, applePosition.Y * 30, 30, 30));
            }
        }

        private void animation(int i)
        {
            int amt = 1;
            bool doIt = true;
            for (int j = 0; j < snakeLength - 1; j++)
            {
                if (snakePositions[j] == previousPosition)
                {
                    doIt = false;
                }
            }
            if (doIt)
            {
                if (directionHistory[oldSnake - amt] == 'R')
                    graphics.DrawLine(bigBox, new Point((previousPosition.X + 1) * 30 - i, previousPosition.Y * 30), new Point((previousPosition.X + 1) * 30 - i, previousPosition.Y * 30 + 30));
                else if (directionHistory[oldSnake - amt] == 'L')
                    graphics.DrawLine(bigBox, new Point(previousPosition.X * 30 + i, previousPosition.Y * 30), new Point(previousPosition.X * 30 + i, previousPosition.Y * 30 + 30));
                else if (directionHistory[oldSnake - amt] == 'D')
                    graphics.DrawLine(bigBox, new Point(previousPosition.X * 30, (previousPosition.Y + 1) * 30 - i), new Point(previousPosition.X * 30 + 30, (previousPosition.Y + 1) * 30 - i));
                else if (directionHistory[oldSnake - amt] == 'U')
                    graphics.DrawLine(bigBox, new Point(previousPosition.X * 30, (previousPosition.Y) * 30 + i), new Point(previousPosition.X * 30 + 30, (previousPosition.Y) * 30 + i));
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.P:
                    if (!nextDirection.Equals(" "))
                        timer1.Enabled = !timer1.Enabled;
                    break;

                case Keys.Left:
                    if (!lastDirection.Equals("L") && (snakeLength == 1 || !lastDirection.Equals("R")))
                    {
                        nextDirection += "L";
                        lastDirection = "L";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.Right:
                    if (!lastDirection.Equals("R") && (snakeLength == 1 || !lastDirection.Equals("L")))
                    {
                        nextDirection += "R";
                        lastDirection = "R";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.Down:
                    if (!lastDirection.Equals("D") && (snakeLength == 1 || !lastDirection.Equals("U")))
                    {
                        nextDirection += "D";
                        lastDirection = "D";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.Up:
                    if (!lastDirection.Equals("U") && (snakeLength == 1 || !lastDirection.Equals("D")))
                    {
                        nextDirection += "U";
                        lastDirection = "U";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.A:
                    if (!lastDirection.Equals("L") && (snakeLength == 1 || !lastDirection.Equals("R")))
                    {
                        nextDirection += "L";
                        lastDirection = "L";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.D:
                    if (!lastDirection.Equals("R") && (snakeLength == 1 || !lastDirection.Equals("L")))
                    {
                        nextDirection += "R";
                        lastDirection = "R";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.S:
                    if (!lastDirection.Equals("D") && (snakeLength == 1 || !lastDirection.Equals("U")))
                    {
                        nextDirection += "D";
                        lastDirection = "D";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;

                case Keys.W:
                    if (!lastDirection.Equals("U") && (snakeLength == 1 || !lastDirection.Equals("D")))
                    {
                        nextDirection += "U";
                        lastDirection = "U";
                        nextDirection = nextDirection.Replace(" ", "");
                    }
                    timer1.Enabled = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Normal_Click(object sender, EventArgs e)
        {
            difficulty = 1;
            restartGame();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {
            difficulty = 0;
            restartGame();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            difficulty = 1;
            restartGame();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            difficulty = 0;
            restartGame();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            difficulty = 1;
            restartGame();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            difficulty = 0;
            restartGame();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            difficulty = 2;
            restartGame();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            difficulty = 2;
            restartGame();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (textBox1.Text[i] < '0' || textBox1.Text[i] > '9')
                {
                    textBox1.Text = textBox1.Text.Remove(i, 1);
                    i--;
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > 502 && e.X < 563 && e.Y > 636 && e.Y < 656)
            {
                textBox1.Enabled = true;
            }
            else if (textBox1.Enabled)
            {
                textBox1.Enabled = false;
                restartGame();
                if (textBox1.Text.Length > 0)
                {
                    timer1.Interval = int.Parse(textBox1.Text);
                }
            }
        }
    }
}
