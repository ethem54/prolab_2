using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class Goblin : Enemy
    {
        public Goblin(string id, double x, double y)
            : base(id, "Goblin", x, y, 50, 0, 8, 10, false, 5)
        {
            // Önce şekli oluştur
            var shape = new Rectangle
            {
                Width = 25,
                Height = 25,
                Fill = Brushes.Orange,
                Stroke = Brushes.Black
            };

            // Sonra bu şekli Can Barı sistemine gönder
            SetupVisual(shape);
        }
    }
}