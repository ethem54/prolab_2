using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class ArcherTower : Tower
    {
        public ArcherTower(string id, double x, double y)
            // Ref: Maliyet=80, Hasar=15, Hız=1.2s. (WPF Menzil: 150px)
            : base(id, "Archer Tower", x, y, 80, 150, 10, 1.4)
        {
            Appearance = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Blue,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            UpdateVisual();
        }
    }
}