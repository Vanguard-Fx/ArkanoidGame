using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hnatyshyn.Nazar._5i.ArkanoidV1.Models
{
    class Paddle : RectAndRectangle
    {
        public Paddle(double PosX, double PosY, double Height, double Width, Brush Color) : base(PosX, PosY, Height, Width, Color) // Costruttore
        {

        }

        public Paddle(Rectangle Rectangle) : base(Rectangle)
        {

        }
    }
}
