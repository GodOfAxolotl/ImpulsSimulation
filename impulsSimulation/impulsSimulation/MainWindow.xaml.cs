using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace impulsSimulation
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Session session;

        double box1XPos = 300;
        double box2XPos = 600;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += loop;

            session = new Session(10, 100, 0, 0, box1XPos, box2XPos);

           /*session = new Session(Convert.ToDouble(massBox1Input.Text),
                                  Convert.ToDouble(massBox2Input.Text),
                                  Convert.ToDouble(gesBox1Input.Text),
                                  Convert.ToDouble(gesBox2Input.Text),
                                  Canvas.GetLeft(box1),
                                  Canvas.GetLeft(box2));*/
            this.DataContext = session;
            syncValues();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            syncValues();
            timer.Start();
            session.start();

            if(Globals.accurateMaths)
            {
                timer.Interval = TimeSpan.FromMilliseconds(0.001);
            } else
            {
                timer.Interval = TimeSpan.FromMilliseconds(1);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(box1, box1XPos);
            Canvas.SetLeft(box2, box2XPos);
            session.reset();
            timer.Stop();
        }

        private void loop(object sender, EventArgs e)
        {
            session.loop();
            Canvas.SetLeft(box1, session.box1.xPos);
            Canvas.SetLeft(box2, session.box2.xPos);
        }




        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            syncValues();
        }

        private void syncValues()
        {
            try
            {
                session.box1.mass = Convert.ToDouble(massBox1Input.Text);
                session.box2.mass = Convert.ToDouble(massBox2Input.Text);
                session.box1.vel = Convert.ToDouble(gesBox1Input.Text);
                session.box2.vel = Convert.ToDouble(gesBox2Input.Text);
                session.friction = Convert.ToDouble(frictionInput.Text);
            } 
            catch(Exception e)
            {
                gesBox1Input.Text = "0";
                gesBox2Input.Text = "0";
                try
                {
                    session.box1.mass = Convert.ToUInt64(massBox1Input.Text);
                    session.box2.mass = Convert.ToUInt64(massBox2Input.Text);
                    session.box1.vel = Convert.ToDouble(gesBox1Input.Text);
                    session.box2.vel = Convert.ToDouble(gesBox2Input.Text);
                    session.friction = Convert.ToDouble(frictionInput.Text);
                }
                catch (Exception ee)
                {
                    gesBox1Input.Text = "0";
                    gesBox2Input.Text = "0";
                }
            }

        }

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newWindowHeight = e.NewSize.Height;
            Globals.windowWidth = e.NewSize.Width;
            double prevWindowHeight = e.PreviousSize.Height;
            double prevWindowWidth = e.PreviousSize.Width;
        }

        private void massBox1Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            //syncValues();
        }

        private void leftWallCollision_Checked(object sender, RoutedEventArgs e)
        {
            Globals.leftwall = true;
        }

        private void rightWallCollision_Checked(object sender, RoutedEventArgs e)
        {
            Globals.rightwall = true;
        }

        private void leftWallCollision_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.leftwall = false;
        }

        private void rightWallCollision_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.rightwall = false;
        }

        private void mathematicCollisionOn_Checked(object sender, RoutedEventArgs e)
        {
            Globals.accurateMaths = true;
        }

        private void mathematicCollisionOn_Unchecked(object sender, RoutedEventArgs e)
        {
            Globals.accurateMaths = false;

        }
    }


    public static class Globals
    {
        public static double windowWidth = 1000;
        public static bool leftwall = true;
        public static bool rightwall = true;

        public static bool accurateMaths = false;
    }
}
