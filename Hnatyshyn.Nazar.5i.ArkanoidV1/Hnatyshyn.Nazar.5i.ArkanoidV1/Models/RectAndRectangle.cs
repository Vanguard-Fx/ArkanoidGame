using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hnatyshyn.Nazar._5i.ArkanoidV1.Models
{
    class RectAndRectangle
    {
        private double width;
        public double Width { get { return width; } }

        private double height;
        public double Height { get { return height; } }

        private double posX;
        public double PosX { get { return posX; } set { posX = value; Rectangle.Margin = new Thickness(posX, posY, 0, 0); Rect.Location = new Point(posX, posY); } }

        private double posY;
        public double PosY { get { return posY; } set { posY = value; Rectangle.Margin = new Thickness(posX, posY, 0, 0); Rect.Location = new Point(posX, posY); } }

        public Rectangle Rectangle;
        public Rect Rect;
        public RectAndRectangle(double PosX, double PosY, double Height, double Width, Brush Color) // Costruttore
        {
            Rectangle = new Rectangle();
            Rect = new Rect();
            this.posX = PosX;
            this.posY = PosY;
            this.height = Height;
            this.width = Width;

            Rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            Rectangle.VerticalAlignment = VerticalAlignment.Top;
            Rectangle.Height = Height;
            Rectangle.Width = Width;
            Rect.Height = Height;
            Rect.Width = Width;
            Rectangle.Fill = Color;
            Rectangle.Margin = new Thickness(PosX, PosY, 0, 0);
            Rect.Location = new Point(PosX, PosY);

            //Rectangle = new Rectangle();
            //Rectangle.Width = Width;
            //Rectangle.Height = Height;
            //Rectangle.Fill = Color;
            //Rectangle.Margin = new Thickness(PosX, PosY, 0, 0);

            //Rect = new Rect();
            //Rect.Width = Width;
            //Rect.Height = Height;
            //Rect.Location = new Point(PosX, PosY);
        }

        public RectAndRectangle(Rectangle Rectangle) : this(Rectangle.Margin.Left, Rectangle.Margin.Top, Rectangle.Height, Rectangle.Width, Rectangle.Fill)
        {

        }

        public void UpdatePosition(double PosX)
        {
            this.posX = PosX;
            Rectangle.Margin = new Thickness(PosX, PosY, 0, 0);
            Rect.Location = new Point(PosX, PosY);
        }
        public void UpdatePosition(double PosX, double PosY)
        {
            this.posX = PosX;
            this.posY = PosY;
            Rectangle.Margin = new Thickness(PosX, PosY, 0, 0);
            Rect.Location = new Point(PosX, PosY);
        }
    }
}