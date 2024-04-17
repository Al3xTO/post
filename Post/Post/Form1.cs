using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Post
{
    public partial class Form1 : Form, IObserver
    {
        private bool isDragging = false;
        private Point lastLocation;
        private int randomNumber;
        private Timer timer1;
        private Timer timer2;
        private Subject subject = new Subject();

        public Form1()
        {
            InitializeComponent();

            subject.Subscribe(this);

            pictureBox7.MouseDown += PictureBox7_MouseDown;
            pictureBox7.MouseMove += PictureBox7_MouseMove;
            pictureBox7.MouseUp += PictureBox7_MouseUp;

            pictureBox1.MouseEnter += PictureBox_MouseEnter;
            pictureBox2.MouseEnter += PictureBox_MouseEnter;
            pictureBox3.MouseEnter += PictureBox_MouseEnter;
            pictureBox4.MouseEnter += PictureBox_MouseEnter;
            pictureBox5.MouseEnter += PictureBox_MouseEnter;
            pictureBox6.MouseEnter += PictureBox_MouseEnter;

            this.MouseUp += Form1_MouseUp;

            timer1 = new Timer();
            timer1.Interval = 100;
            timer1.Tick += Timer1_Tick;

            timer2 = new Timer();
            timer2.Interval = 2000;
            timer2.Tick += Timer2_Tick;

            GenerateRandomNumber();
            labelpoint.Text = "0xp";
        }

        public void Update(int randomNumber)
        {
            labelhouse.Text = $"Доставити у дім: {randomNumber}";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            pictureBox7.Location = new Point(2000, 2000);
            timer1.Stop();
            timer2.Start();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            pictureBox7.Location = new Point(13, 200);
            timer2.Stop();
            GenerateRandomNumber();
        }

        private void PictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastLocation = e.Location;
        }

        private void PictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                pictureBox7.Left += e.X - lastLocation.X;
                pictureBox7.Top += e.Y - lastLocation.Y;
            }
        }

        private void PictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;

            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox != pictureBox7 && pictureBox.Bounds.Contains(pictureBox7.Location))
                {
                    if (randomNumber == int.Parse(pictureBox.Name.Substring(10)))
                    {
                        labelpoint.Text = (int.Parse(labelpoint.Text.Substring(0, labelpoint.Text.Length - 2)) + 10).ToString() + "xp";
                        label10.Text = "Посилку доставлено успішно";
                    }
                    else
                    {
                        labelpoint.Text = (int.Parse(labelpoint.Text.Substring(0, labelpoint.Text.Length - 2)) - 5).ToString() + "xp";
                        label10.Text = "Посилка прийшла не на ту адресу";
                    }
                    break;
                }
            }

            timer1.Start();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void GenerateRandomNumber()
        {
            Random random = new Random();
            randomNumber = random.Next(1, 7);
            labelhouse.Text = $"Доставити у дім: {randomNumber}";

            subject.Notify(randomNumber);
        }
    }

    public interface IObserver
    {
        void Update(int randomNumber);
    }

    public class Subject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify(int randomNumber)
        {
            foreach (var observer in observers)
            {
                observer.Update(randomNumber);
            }
        }
    }
}

