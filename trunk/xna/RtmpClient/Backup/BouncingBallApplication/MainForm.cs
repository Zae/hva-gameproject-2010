using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FluorineFx;
using FluorineFx.Net;

//Based on the http://blogs.msdn.com/vancem/archive/2006/10/23/intro-to-programming-excercise-bouncing-balls.aspx sample

namespace BouncingBallApplication
{
    public partial class MainForm : Form
    {
        Ball _ball;
        Rectangle _ballBox;
        public static Random random = new Random();

        NetConnection _netConnection;
        RemoteSharedObject _sharedObject;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _ballBox = ClientRectangle;
            _ballBox.Y += toolStrip1.Size.Height;
            _ballBox.Height -= toolStrip1.Size.Height;

            Point start = new Point(_ballBox.Left + _ballBox.Width / 2, _ballBox.Top + _ballBox.Height / 2);
            _ball = new Ball(start, new Point(random.Next(-8, 8), random.Next(-5, 5)), random.Next(25, 100));
            Invalidate();

            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _ball.Update(this, _ballBox);
            if (_sharedObject != null && _sharedObject.Connected)
            {
                ASObject ballCoordinates = new ASObject();
                ballCoordinates["x"] = _ball.Bounds.X;
                ballCoordinates["y"] = _ball.Bounds.Y;
                _sharedObject["ballCoordinates"] = ballCoordinates;
            }
        }

        private void _toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            _netConnection = new NetConnection();
            _netConnection.OnConnect += new ConnectHandler(_netConnection_OnConnect);
            _netConnection.Connect("rtmp://localhost:1937/SharedBall");
        }

        void _netConnection_OnConnect(object sender, EventArgs e)
        {
            _sharedObject = RemoteSharedObject.GetRemote("BallControl", _netConnection.Uri.ToString(), false);
            _sharedObject.Connect(_netConnection);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            _ball.Draw(g);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _ballBox = ClientRectangle;
            _ballBox.Y += toolStrip1.Size.Height;
            _ballBox.Height -= toolStrip1.Size.Height;
            Invalidate();
        }

    }

    class Ball
    {
        /// <summary>
        /// To keep the animation reasonable, we limit the speed of the ball.  
        /// </summary>
        public static int MaxSpeed = 10;

        public Ball(Point start, Point startVelocity, int size)
        {
            if (size > 200)
                size = 200;

            _bounds = new Rectangle(start.X - (size / 2), start.Y - (size / 2), size, size);
            _velocity = startVelocity;
            _angleVelocity = random.Next(-200, 200) / 200.0F;
            ClipSpeed(MaxSpeed);
        }

        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Draw a ball.
        /// </summary>
        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Gray, _bounds);
        }

        public void Update(Control drawingControl, Rectangle region)
        {
            // Since the ball will move, anything in the old region needs to be repainted
            drawingControl.Invalidate(_bounds);
            _bounds.Offset(_velocity);

            // Update the spin 
            _angle += _angleVelocity;
            if (_angle >= 360.0F)
                _angle -= 360.0F;

            // See if we hit a wall.
            if (_bounds.Left <= region.Left)     // Bounce off the left wall
            {
                _bounds.X = region.Left;
                _velocity.X = -_velocity.X;
            }
            else if (_bounds.Right >= region.Right)
            {
                _bounds.X = region.Right - _bounds.Width;
                _velocity.X = -_velocity.X;
            }

            if (_bounds.Top <= region.Top)
            {
                _bounds.Y = region.Top;
                _velocity.Y = -_velocity.Y;
            }
            else if (_bounds.Bottom >= region.Bottom)
            {
                _bounds.Y = region.Bottom - _bounds.Height;
                _velocity.Y = -_velocity.Y;
            }

            // Indicate that anything in the new position needs to be repainted. 
            drawingControl.Invalidate(_bounds);
        }

        /// <summary>
        /// Returns the center of the ball
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point(_bounds.X + _bounds.Width / 2, _bounds.Y + _bounds.Height / 2);
            }
        }


        private void ClipSpeed(int max)
        {
            float speed = Length(_velocity);
            int maxSpeed = max;
            if (speed > maxSpeed)
                _velocity = Point.Round(Scale(_velocity, maxSpeed / speed));
        }

        private static float Length(PointF p)
        {
            return ((float)Math.Sqrt(p.X * p.X + p.Y * p.Y));
        }

        private static PointF Scale(PointF p, float scale)
        {
            return new PointF(p.X * scale, p.Y * scale);
        }

        private static PointF Add(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        private static float Dot(PointF p1, PointF p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        private static int Max(int x, int y)
        {
            if (x > y)
                return x;
            else
                return y;
        }
        private static int Min(int x, int y)
        {
            if (x < y)
                return x;
            else
                return y;
        }

        /// <summary>
        /// Keep 'x' within -limit to +limit inclusive
        /// </summary>
        private static float Limit(float x, float limit)
        {
            if (x < -limit)
                return -limit;
            if (x > +limit)
                return +limit;
            return x;
        }


        float _angle;                
        float _angleVelocity;
        Rectangle _bounds;
        Point _velocity;
        public static Random random = new Random(); // need some random numbers
    }

}