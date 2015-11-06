using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hnatyshyn.Nazar._5i.ArkanoidV1.Models
{
    class Ball
    {
        private double width;
        public double Width { get { return width; } }

        private double height;
        public double Height { get { return height; } }

        private double posX;
        public double PosX {
            get { return posX; } set { posX = value; Ellipse.Margin = new Thickness(posX, posY, 0, 0); Rect.Location = new Point(posX, posY); } }

        private double posY;
        public double PosY { get { return posY; } set { posY = value; Ellipse.Margin = new Thickness(posX, posY, 0, 0); Rect.Location = new Point(posX, posY); } }
        public Brush Color { get; set; }

        private double speed;
        public double Speed { get { return speed; } set { if (value >= 0) { speed = value; } } }

        public Ellipse Ellipse;
        public Rect Rect;


        public double direction_X { get; set; }
        public double direction_Y { get; set; }


        public Ball(double PosX, double PosY, double Height, double Width, Brush Color, float direction_X, float direction_Y, int speed) // Costruttore
        {
            Ellipse = new Ellipse();
            Rect = new Rect();
            Ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            Ellipse.VerticalAlignment = VerticalAlignment.Top;
            Ellipse.Effect = new System.Windows.Media.Effects.DropShadowEffect();

            Reset(PosX, PosY, Height, Width, Color, direction_X, direction_Y, speed);
        }
        public void MoveBall()
        {
            this.posX += direction_X * this.speed;
            this.posY += direction_Y * this.speed;
            Ellipse.Margin = new Thickness(posX, posY, 0, 0);
            Rect.Location = new Point(posX, posY);
            //UpdateData();
        }
        public void Reset(double PosX, double PosY, double Height, double Width, Brush Color, float direction_X, float direction_Y, int speed)
        {
            this.posX = PosX;
            this.posY = PosY;
            this.height = Height;
            this.width = Width;
            this.Color = Color;
            this.direction_X = direction_X;
            this.direction_Y = direction_Y;
            this.speed = speed;

            Ellipse.Width = Width;
            Ellipse.Height = Height;
            Ellipse.Fill = Color;
            Ellipse.Margin = new Thickness(PosX, PosY, 0, 0);


            Rect.Width = Width;
            Rect.Height = Height;
            Rect.Location = new Point(PosX, PosY);
        }

        public void BorderCollision(double Win_Width)
        {
            if ((Ellipse.Margin.Left) <= 0 || (Ellipse.Margin.Left + 38) >= Win_Width)
                direction_X = -direction_X;
            if ((Ellipse.Margin.Top) <= 0 /*|| (ball.Margin.Top + 38) > this.Height*/)
                direction_Y = -direction_Y;
        }

        public bool LeavesScreen(double Win_Height)
        {
            if (Ellipse.Margin.Top > Win_Height)
                return true;
            return false;
        }

        public bool IntersectsWithPaddle(Rect Paddle)
        {
            if (this.Rect.IntersectsWith(Paddle))
            {
                if (direction_X > 0)
                    if (this.Rect.Right - Paddle.Left <= speed)
                        direction_X = -direction_X;
                if (direction_X < 0)
                    if (Paddle.Right - this.Rect.Left <= speed)
                        direction_X = -direction_X;
                if (direction_Y > 0)
                    if (this.Rect.Bottom - Paddle.Top <= speed)
                    {
                        direction_Y = -direction_Y;
                        double distance = (this.Rect.Location.X + this.width / 2) - (Paddle.Location.X + Paddle.Width / 2);
                            direction_X = distance / (Paddle.Width / 3);
                    }

                return true;
            }
            return false;
        }
        public bool IntersectsWithBrick(Rect Brick)
        {
            if (this.Rect.IntersectsWith(Brick))
            {
                if (direction_X > 0)
                    if (this.Rect.Right - Brick.Left <= speed)
                        direction_X = -direction_X;
                if (direction_X < 0)
                    if (Brick.Right - this.Rect.Left <= speed)
                        direction_X = -direction_X;
                if (direction_Y > 0)
                    if (this.Rect.Bottom - Brick.Top <= speed)
                        direction_Y = -direction_Y;
                if (direction_Y < 0)
                    if (Brick.Bottom - this.Rect.Top <= speed)
                        direction_Y = -direction_Y;
                return true;
            }
            return false;
        }
    }
}