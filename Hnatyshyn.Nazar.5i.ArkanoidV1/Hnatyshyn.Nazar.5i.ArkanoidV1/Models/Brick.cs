using System.Windows.Media;
using System.Windows.Shapes;

namespace Hnatyshyn.Nazar._5i.ArkanoidV1.Models
{
    class Brick : RectAndRectangle
    {
        private int score;
        public int Score { get { return score; } }
        public Brick(double PosX, double PosY, double Height, double Width, Brush Color, int Score) : base(PosX, PosY, Height, Width, Color) // Costruttore
        {
            this.score = Score;
        }

        public Brick(Rectangle Rectangle) : base(Rectangle)
        {

        }
    }
}