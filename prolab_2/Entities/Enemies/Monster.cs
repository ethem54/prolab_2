using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class FlyingMonster : Enemy
    {
        public FlyingMonster(string id, double x, double y) : base(id, "Flying Monster", x, y, 50, 0, 8, 15, true, 5)
        { 
            var shape = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.LawnGreen,
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            SetupVisual(shape);
        }
    }
}